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

namespace eAdmin.Pages.PageProjects
{
    public class PageProjectBase : PageCore
    {


        public List<ProjectModel> projects = new List<ProjectModel>();
        public ProjectModel model = new ProjectModel();

        public string StateKey = "PROJ84567Gs25245KJHGytkjhTonB3PCz2Ts"; //Storage and Session Key

        public int TotalRecord = 0;
        public bool ShowModal = false;
        public string ModalTitle = "";

        string controller_api = "Project";

        DateTime date = DateTime.Now;

        public string ControllerApi
        {
            get
            {
                if (string.IsNullOrEmpty(state.pager.order_by))
                {
                    state.pager.order_by = "id";
                    state.pager.order_by_type = "desc";
                }
                string url = $"{controller_api}?$expand=customer($select=id,customer_code,customer_name_en,photo,company_name,company_name_kh,),project_type($select=id,project_type_name,icon,color)&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";
                return url + GetFilter(state.filters);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);
            if (state.page_title == "")
            {
                state.page_title = "Project";
                var default_view = gv.GetDefaultModuleView("page_project");
                if (default_view != null)
                {
                    state.page_title = default_view.title;
                    state.filters = default_view.filters;
                }
            }
            if (state.filters.Count == 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "is_deleted",
                    value1 = "false"
                });
            }
            await LoadData();
            is_loading = false;
        }

        public async Task OnRefresh()
        {
            is_loading = true;
            await LoadData();
            is_loading = false;
        }
        public void Clone_Click(Guid id)
        {
            is_loading_data = true;
            nav.NavigateTo($"project/clone/{id}");
            is_loading_data = false;
        }

        public async Task ViewClick(ModuleViewModel m)
        {
            state.filters.Clear();
            state.filters = m.filters;
            state.pager.order_by = m.default_order_by;
            state.pager.order_by_type = m.default_order_by_type;
            state.page_title = m.title;
            state.pager.current_page = 1;
            await LoadData();
        }

        public async Task LoadData(string api_url = "")
        {
            is_loading = true;
            if (string.IsNullOrEmpty(api_url))
            {
                api_url = $"{ControllerApi}";
                state.api_url = api_url;
                await SetState(StateKey, state);
            }

            var resp = await http.ApiGetOData(api_url);
            if (resp.IsSuccess)
            {
                projects = JsonSerializer.Deserialize<List<ProjectModel>>(resp.Content.ToString());
                TotalRecord = resp.Count;
            }
            is_loading = false;
        }

        public async Task OrderBy(string col_name = "")
        {

            state.pager.order_by = col_name;
            state.pager.order_by_type = (state.pager.order_by_type == "asc" ? "desc" : "asc");

            await LoadData();
        }

        public async Task OnToogleStatus(ProjectModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }
        public async Task OnToogleStatusLabel(ProjectModel p)
        {
            p.is_change_status = true;
            await SaveStatus(p);
            p.is_change_status = false;
        }
        public async Task SaveStatus(ProjectModel p)
        {
            var project = new ProjectModel();
            project = p;
            project.status = !project.status;
            var resp = await http.ApiPost(controller_api + "/save", project);
            if (resp.IsSuccess)
            {
                toast.Add("Change status successfully", Severity.Success);
                if (projects.Count() == 1 && state.pager.current_page > 1)
                {
                    state.pager.current_page = state.pager.current_page - 1;
                }

                await LoadData();
            }
        }

        public async Task OnDelete(ProjectModel p)
        {
            p.is_loading = true;

            if (await alert.ShowMessageBox("Delete Project", "Are you sure you want to delete this record?") == true)
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add("Delete Project successfully", Severity.Success);
                    if (projects.Count() == 1 && state.pager.current_page > 0)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task OnRestore(ProjectModel p)
        {
            p.is_loading = true;
            if (await alert.ShowMessageBox("Restore project", "Are you sure you want to restore this record?") == true)

            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);

                if (resp.IsSuccess)
                {
                    if (projects.Count() == 1 && state.pager.current_page > 1)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
                toast.Add("Restore projct successfully", Severity.Success);
            }
            p.is_loading = false;
        }

        public async Task SelectChange(int perpage)
        {
            state.pager.per_page = perpage;
            state.pager.current_page = 1;
            await LoadData();
        }

        public async Task FilterClick(string keyword)
        {
            state.filters.RemoveAll(r => r.filter_info_text != "");
            SetFilterValue2(state.filters, "keyword", keyword);
            if (state.date_range.is_visible)
            {
                state.filters.Add(
                    new FilterModel()
                    {
                        key = "created_date",
                        value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.start_date),
                        filter_title = "Register Date",
                        filter_info_text = Convert.ToDateTime(state.date_range.start_date).ToString(gv.date_format) + " - " + Convert.ToDateTime(state.date_range.end_date).ToString(gv.date_format),
                        filter_operator = "Ge",
                        is_clear_all = true,
                        will_remove = true,
                        state_property_name = "date_range"
                    }
                );

                //end date
                state.filters.Add(new FilterModel()
                {
                    key = "created_date",
                    value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.end_date),
                    is_clear_all = true,
                    filter_operator = "Le",
                    will_remove = true,
                    state_property_name = "date_range"
                });
            }


            if (state.project_type != null && state.project_type.id > 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "project_type_id",
                    value1 = state.project_type.id.ToString(),
                    filter_title = "Project Type",
                    state_property_name = "project_type",
                    filter_info_text = state.project_type.project_type_name,
                    is_clear_all = true,
                    will_remove = true
                });
            }
            state.pager.current_page = 1;
            await LoadData();
        }

        public async Task RemoveFilter(FilterModel f)
        {
            is_loading = true;
            string[] remove_key = f.remove_key.Split(',');
            foreach (var k in remove_key)
            {
                state.filters.RemoveAll(r => r.key == k);
            }

            state.pager.current_page = 1;
            //gv.RemoveFilter
            RemoveFilter(state, f.state_property_name);
            await LoadData();
            is_loading = false;
        }

        public async Task RemoveAllFilter()
        {

            is_loading = true;
            SetFilterValue2(state.filters, "keyword", "");
            foreach (var f in state.filters.Where(r => r.is_clear_all == true))
            {
                RemoveFilter(state, f.state_property_name);
            }

            state.filters.RemoveAll(r => r.is_clear_all == true);
            state.pager.current_page = 1;
            await LoadData();
            is_loading = false;
        }

        public async Task ChangePager(int _page)
        {
            state.pager.current_page = _page;
            await LoadData();
        }

        public async Task OnSearch(string keyword)
        {
            state.pager = new PagerModel();
            SetFilterValue2(state.filters, "keyword", keyword);
            await LoadData();
        }
        public async Task OnRemoveKeyword()
        {
            state.pager = new PagerModel();
            SetFilterValue2(state.filters, "keyword", "");
            await LoadData();
        }

    }
}
