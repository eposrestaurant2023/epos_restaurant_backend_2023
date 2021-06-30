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

        public ProjectModel model { get; set; } = new ProjectModel();
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            await LoadProject();
            is_loading = false;
        }

        public async Task LoadProject()
        {
            var res = await http.ApiGetOData($"project({id})");
            if (res.IsSuccess)
            {
                model = JsonSerializer.Deserialize<ProjectModel>(res.Content.ToString());
            }
        }

    }
}
