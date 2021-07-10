using eModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using MudBlazor;

namespace eAdmin.Pages.PageProjects
{
    public class PageProjectDetails : PageCore
    {
        [Parameter] public string id { get; set; }
        [Inject] protected IDialogService Dialog { get; set; }

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
        
        public async Task DeleteProject_Click()
        {
            string message = "Are you sure you want to delete this Project?";
            if (model.is_deleted)
            {
                message = "Are you sure you want to restore this Project?";
            }

            bool? result = await Dialog.ShowMessageBox("Warning", message, yesText: model.is_deleted ? "Restore" : "Delete", cancelText: "Cancel");
            if (result != null)
            {
                is_loading = true;
                StateHasChanged();
                var resp = await http.ApiPost($"project/delete/{model.id}");
                if (resp.IsSuccess)
                {
                    if (model.is_deleted)
                    {
                        toast.Add("Delete Project Successfully", Severity.Success);
                    }
                    else
                    {
                        toast.Add("Restore Project successfully", Severity.Success);
                    }
                    await LoadProject();
                }
                else
                {
                    model.is_deleted = !model.is_deleted;
                    if (model.is_deleted)
                    {
                        toast.Add("Delete Project Fail", Severity.Warning);
                    }
                    else
                    {
                        toast.Add("Restore Project Fail", Severity.Warning);
                    }
                }
            }

            is_loading = false;
        }

   


    }
}
