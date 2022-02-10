using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using eAdmin;
using eAdmin.Shared;
using MudBlazor;
using eModels;
using eAdmin.Shared.Users;
using eAdmin.Shared.Components;
using eAdmin.Shared.ComLayout;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace eAdmin.Pages.PageEknowledgeBase
{
    public class PageAddEknowledgeBases:PageCore
    {
        [Parameter] public string id { get; set; }
        [Parameter] public string parent_id { get; set; }
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

        public Dictionary<string, object> editorConf = new Dictionary<string, object>{
    {"menubar", false},
     
  };

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

        public void Click_add()
        {  
            models.Add(new eKnowledgeBaseModel(Guid.Parse(parent_id)));
        }

        public void DeleteChil_Click(eKnowledgeBaseModel d)
        {
            model.children.Remove(d);
        }
      public void AddChild_Click()
        {
            model.children.Add(new eKnowledgeBaseModel());
        }

    }
}
