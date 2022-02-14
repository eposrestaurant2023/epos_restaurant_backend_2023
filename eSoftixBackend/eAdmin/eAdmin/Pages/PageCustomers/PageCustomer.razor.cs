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

namespace eAdmin.Pages.PageCustomers
{
    public    class PageCustomerBase:PageCore
    {


        public List<CustomerModel> customers = new List<CustomerModel>();
        public CustomerModel model = new CustomerModel();

        public string StateKey = "278484567Gs25245KJHGytkjhTonB3PCz2Ts"; //Storage and Session Key

        public int TotalRecord = 0;
        public bool ShowModal = false;
        public string ModalTitle = "";

        string controller_api = "Customer";

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
                string url = $"{controller_api}?$expand=customer_group($select=customer_group_name_en,customer_group_name_kh)&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";
                return url + GetFilter(state.filters);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);
            if (state.page_title == "")
            {
                state.page_title = "Customer";
                var default_view = gv.GetDefaultModuleView("page_customer");
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
            nav.NavigateTo($"customer/clone/{id}");
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
                customers = JsonSerializer.Deserialize<List<CustomerModel>>(resp.Content.ToString());
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

        public async Task OnToogleStatus(CustomerModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }
        public async Task OnToogleStatusLabel(CustomerModel p)
        {
            p.is_change_status = true;
            await SaveStatus(p);
            p.is_change_status = false;
        }
        public async Task SaveStatus(CustomerModel p)
        {
            var customer = new CustomerModel();
            customer = p;
            customer.status = !customer.status;
            var resp = await http.ApiPost(controller_api + "/save", customer);
            if (resp.IsSuccess)
            {
                toast.Add("Change status successfully", Severity.Success);
                if (customers.Count() == 1 && state.pager.current_page > 1)
                {
                    state.pager.current_page = state.pager.current_page - 1;
                }

                await LoadData();
            }
        }

        public async Task OnDelete(CustomerModel p)
        {
            p.is_loading = true;
            
            if (await alert.ShowMessageBox("Delete Customer", "Are you sure you want to delete this record?")==true)
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add("Delete customer successfully", Severity.Success);
                    if (customers.Count() == 1 && state.pager.current_page > 0)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task OnRestore(CustomerModel p)
        {
            p.is_loading = true;
            if (await alert.ShowMessageBox("Restore Customer", "Are you sure you want to restore this record?") == true)
              
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);

                if (resp.IsSuccess)
                {
                    if (customers.Count() == 1 && state.pager.current_page > 1)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
                toast.Add("Restore customer successfully", Severity.Success);
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
                        filter_info_text = Convert.ToDateTime( state.date_range.start_date).ToString(gv.date_format) + " - " + Convert.ToDateTime(state.date_range.end_date).ToString(gv.date_format),
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


            if (state.customer_group != null && state.customer_group.id > 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "customer_group_id",
                    value1 = state.customer_group.id.ToString(),
                    filter_title = "Customer Group",
                    state_property_name = "customer_group",
                    filter_info_text = state.customer_group.customer_group_name_en,
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