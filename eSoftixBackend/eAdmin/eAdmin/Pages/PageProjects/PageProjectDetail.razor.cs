using eModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace eAdmin.Pages.PageProjects
{
    public class PageProjectDetails : PageCore
    {
        [Parameter] public string id { get; set; }

        public string page_title { get {
                
                if (model != null)
                {
                    return $"{model.project_code} -{model.project_name}";
                }
                return "New Project";
            } }

        public ProjectModel model { get; set; } = new ProjectModel();
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            await LoadProject();
            is_loading = false;
        }

        public async Task LoadProject()
        {
            var res = await http.ApiGet($"project({id})?$expand=project_type,business_branches($expand=outlets($expand=stations),stock_locations)");
            if (res.IsSuccess)
            {
                model = JsonSerializer.Deserialize<ProjectModel>(res.Content.ToString());
            }
        }


        public async Task OnRefresh()
        {
            is_loading = true;
            await LoadProject();
            is_loading = false;
        }
        public async Task OnDeleteProject()
        {
            await Task.Delay(100);
        }
        
        public async Task DeleteProject_Click()
        {
            await Task.Delay(100);
        }

   


    }
}
