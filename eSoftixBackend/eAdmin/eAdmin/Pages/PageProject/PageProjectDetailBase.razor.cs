using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace eAdmin.Pages.PageProject
{
    public class PageProjectDetailBase : PageCore
    {
        [Parameter] public int id { get; set; }
        public ProjectModel model { get; set; }
        public bool ShowModal = false;
        public string ModalTitle = "";
        string controller_api = "project";
        public bool is_comment_loaded, show_business_brand, show_station;
        public bool sale_history_tab_click, sale_product_history_tab_click, payment_history_tab_click;
        List<HistoryModel> histories = new List<HistoryModel>();

        public string api_query
        {
            get
            {
                string query = $"{controller_api}({id})";
                query += $"?$expand=project_type,project_contacts";
                return query;
            }
        }


        protected override async Task OnParametersSetAsync()
        {
            is_loading = true;
            await LoadData();
        }

        public async Task LoadData()
        {
            is_loading = true;
            var resp = await http.ApiGet(api_query);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<ProjectModel>(resp.Content.ToString());
            }
            else
            {
                is_error = true;
            }

            is_loading = false;
        }

        public void OnEdit(int id)
        {
            is_loading_data = true;
            nav.NavigateTo($"project/edit/{id}");
            is_loading_data = false;
        }
        public async Task OnDelete(ProjectModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("Delete Project", "Are you sure you want to delete this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add("Delete Project successfully", MatBlazor.MatToastType.Success);
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task OnRestore(ProjectModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("Restore Project", "Are you sure you want to restore this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);

                if (resp.IsSuccess)
                {
                    await LoadData();
                }
                toast.Add("Restore Project successfully", MatBlazor.MatToastType.Success);
            }
            p.is_loading = false;
        }
        public void SaveComment(HistoryModel h)
        {
            histories.Add(h);
        }

        public async Task OnToogleStatus(ProjectModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }

        public async Task SaveStatus(ProjectModel p)
        {
            var project = new ProjectModel();
            project = p;
            project.status = !project.status;
            string d = JsonSerializer.Serialize(project);
            var resp = await http.ApiPost(controller_api + "/save", project);
            if (resp.IsSuccess)
            {
                toast.Add("Change status successfully", MatToastType.Success);
                await LoadData();
            }
        }




    }
}
