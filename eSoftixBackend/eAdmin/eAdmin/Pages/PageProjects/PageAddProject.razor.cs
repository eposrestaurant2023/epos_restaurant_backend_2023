using eModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageProjects
{
    public class PageAddProjects:PageCore
    {
        [Parameter] public string id { get; set; }
        [Parameter] public string clone_id { get; set; }
        public ProjectModel model { get; set; }
        public string page_title { get; set; }
        public string api_url
        {
            get
            {
                return $"project({id})?$expand=project_type($select=icon,color,project_type_name)";
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await LoadProject();
        }
        async Task LoadProject()
        {
            if (!is_loading)
            {
                is_loading = true;

                if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(clone_id))
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        var res = await http.ApiGet(api_url);
                        if (res.IsSuccess)
                        {
                            model = JsonSerializer.Deserialize<ProjectModel>(res.Content.ToString());
                            title = $"Edit : {model.project_name}";
                        }
                        else
                        {
                            toast.Add(res.Content.ToString(), Severity.Warning);
                        }
                    }
                    else
                    {

                        var res = await http.ApiPost($"project/clone/{clone_id}");
                        if (res.IsSuccess)
                        {
                            model = JsonSerializer.Deserialize<ProjectModel>(res.Content.ToString());
                            title = $"Clone : {model.project_name}";
                        }
                        else
                        {
                            toast.Add(res.Content.ToString(), Severity.Warning);
                        }
                    }
                }
                else
                {
                    model = new ProjectModel();
                }
                is_loading = false;
            }
        }
        public async Task Save_Click()
        {
            model.is_saving = true;
            var res = await http.ApiPost($"project/save", model);
            if (res.IsSuccess)
            {
                toast.Add("Save Successfull.", Severity.Success);
                var c = JsonSerializer.Deserialize<ProjectModel>(res.Content.ToString());
                nav.NavigateTo($"project/{c.id}");
            }
            else
            {
                toast.Add(res.Content.ToString(), Severity.Warning);
            }
            model.is_saving = false;
        }
    }
}
