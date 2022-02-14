﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using eModels;
using System.Text.Json;

namespace eAdmin.Pages.PageEknowledgeBase
{
    public class PageAddEknowledgeBases:PageCore
    {
        [Parameter] public string id { get; set; }
        [Parameter] public string parent_id { get; set; }
        public Dictionary<string, object> editorConf = new Dictionary<string, object>{
    {"menubar", false},

  };

        public eKnowledgeBaseModel model { get; set; } = new eKnowledgeBaseModel();
        public List<eKnowledgeBaseModel> models = new List<eKnowledgeBaseModel>();
        public string page_title { get; set; }
        public string api_url
        {
            get
            {
                return $"eKnowledgeBase({id})?$expand=children";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(id))
            {
                await LoadData();
            }
            else
            {
                if (!string.IsNullOrEmpty(parent_id))
                {
                    model = new eKnowledgeBaseModel(Guid.Parse(parent_id));
                }
                
            }
            
        }
        async Task LoadData()
        {
            if (!is_loading)
            {
                is_loading = true;

                if (!string.IsNullOrEmpty(id))
                {
                    var res = await http.ApiGet(api_url);
                    if (res.IsSuccess)
                    {
                        model = JsonSerializer.Deserialize<eKnowledgeBaseModel>(res.Content.ToString());
                        page_title = $"Edit : {model.title_en}";
                        
                    }
                    else
                    {
                        toast.Add(res.Content.ToString(), Severity.Warning);
                    }
                 
                }
                else
                {
                    page_title = "New eKnowledge Base";
                    if (parent_id != null)
                    {
                        models.Add(new eKnowledgeBaseModel(Guid.Parse(parent_id)));
                    }
                   
                }

                is_loading = false;
            }
        }
        public async Task Save_Click()
        {
            is_loading = true;
            is_saving = true;
            var res = await http.ApiPost($"eKnowledgeBase/savesingle", model);
            if (res.IsSuccess)
            {
                toast.Add("Save Successfull.", Severity.Success);
                var c = JsonSerializer.Deserialize<eKnowledgeBaseModel>(res.Content.ToString());
                nav.NavigateTo($"eknowledgebase/{c.parent_id}");
            }
            else
            {
                toast.Add(res.Content.ToString(), Severity.Warning);
            }
            await LoadData();
            is_saving = false;
            is_loading = false;
        }

        public async Task Save_Close_Click()
        {
            is_loading = true;
            is_saving = true;
            var res = await http.ApiPost($"eKnowledgeBase/savesingle", model);
            if (res.IsSuccess)
            {
                toast.Add("Save Successfull.", Severity.Success);
                var c = JsonSerializer.Deserialize<eKnowledgeBaseModel>(res.Content.ToString());
            }
            else
            {
                toast.Add(res.Content.ToString(), Severity.Warning);
            }
            await LoadData();
            is_saving = false;
            is_loading = false;
        }


        public void Click_add()
        {  
            models.Add(new eKnowledgeBaseModel(Guid.Parse(parent_id)));
        }

        public void DeleteChil_Click(eKnowledgeBaseModel d)
        {
            if (d.id == Guid.Empty)
            {
                model.children.Remove(d);
            }else
            {
                d.is_deleted = true;
            }
        }

        public void Deleteimage_Click(eKnowledgeBaseModel d)
        {
            d.photo_kh = null;
       }

        public void Deleteimageen_Click(eKnowledgeBaseModel d)
        {
            d.photo_en = null;
        }

        public void AddChild_Click()
        {
            eKnowledgeBaseModel m = new eKnowledgeBaseModel();
            m.sort_order = model.children.Where(r=>r.is_deleted==false).Count()+1;
            model.children.Add(m);
        }

    }
}
