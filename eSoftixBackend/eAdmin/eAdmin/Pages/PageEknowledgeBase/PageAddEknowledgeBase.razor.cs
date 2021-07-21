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
        public eKnowledgeBaseModel model { get; set; }
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
                await LoadProject();
            }
            else
            {
                if (!string.IsNullOrEmpty(parent_id))
                {
                    models.Add(new eKnowledgeBaseModel(Guid.Parse(parent_id)));
                }
                
            }
            
        }
        async Task LoadProject()
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
                        models.Add(model);
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
            is_saving = true;
          
            var res = await http.ApiPost($"eKnowledgeBase/save", models);
            if (res.IsSuccess)
            {
                toast.Add("Save Successfull.", Severity.Success);
                var c = JsonSerializer.Deserialize<List<eKnowledgeBaseModel>>(res.Content.ToString());
                
                nav.NavigateTo($"eknowledgebase/{c.FirstOrDefault().parent_id}");
            }
            else
            {
                toast.Add(res.Content.ToString(), Severity.Warning);
            }
            is_saving = false;
        }

        public void Click_add()
        {
            models.Add(new eKnowledgeBaseModel(Guid.Parse(parent_id)));
        }
    }
}
