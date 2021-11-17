using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageCustomers
{
    public class PageCustomerGroups : PageCore
    {
        public List<CustomerGroupModel> customer_groups = new List<CustomerGroupModel>();
        public CustomerGroupModel model = new CustomerGroupModel();
        public int TotalRecord = 0;
        public bool ShowModal = false;
        public string ModalTitle = "";
        string controller_api = "CustomerGroup";
        public string StateKey
        {
            get
            {

                return "278484567Gs254254njht8kjhTonB3PCz2Ts" + gv.current_login_user.id; //Storage and Session Key  
            }
        }

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

                string url = $"{controller_api}?&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";

                return url + GetFilter(state.filters);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);
            if (state.page_title == "")
            {
                state.page_title = lang["Customer Group"];
                var default_view = gv.GetDefaultModuleView("page_customer_group");
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
            await LoadData(state.api_url);
            is_loading = false;
        }
        public async Task OnSave()
        {
            await LoadData();
            ShowModal = false;
            model = new CustomerGroupModel();
        }
        public async Task OnEdit(int id)
        {
            is_loading_data = true;
            var resp = await http.ApiGet(controller_api + $"({id})");
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<CustomerGroupModel>(resp.Content.ToString());
            }
            ModalTitle = lang["Edit"] + ":" + model.customer_group_name_en;
            ShowModal = true;
            is_loading_data = false;
        }
        public void AddNew()
        {
            ShowModal = true;
            ModalTitle = lang["Customer Group"];
        }
        public async Task Clone_Click(int id)
        {
            is_loading_data = true;
            var resp = await http.ApiPost(controller_api + "/clone/" + id);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<CustomerGroupModel>(resp.Content.ToString());
            }
            ModalTitle = lang["Clone"] + ":" + model.customer_group_name_en;
            ShowModal = true;
            is_loading_data = false;
        }
        public async Task ViewClick(ModuleViewModel m)
        {
            state.filters.Clear();
            state.filters = m.filters;
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
                customer_groups = JsonSerializer.Deserialize<List<CustomerGroupModel>>(resp.Content.ToString());
                TotalRecord = resp.Count;
                gv.customer_groups = customer_groups;
            }
            is_loading = false;
        }
        async Task ReloadCustomerGroup()
        {
            var resp_gv = await http.ApiGet("GlobalVariable?$expand=customer_groups");
            if (resp_gv.IsSuccess)
            {
                var data = JsonSerializer.Deserialize<GlobalVariableModel>(resp_gv.Content.ToString());
             
            }
        }
        public async Task OrderBy(string col_name = "")
        {      
            state.pager.order_by = col_name;
            state.pager.order_by_type = (state.pager.order_by_type == "asc" ? "desc" : "asc");   
            await LoadData();
        }
        public async Task OnToogleStatus(CustomerGroupModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }
        public async Task OnToogleStatusLabel(CustomerGroupModel p)
        {
            p.is_change_status = true;
            await SaveStatus(p);
            p.is_change_status = false;
        }
        public async Task SaveStatus(CustomerGroupModel p)
        {
            var customerGroup = new CustomerGroupModel();
            customerGroup = p;
            customerGroup.status = !customerGroup.status;
            string d =JsonSerializer.Serialize(customerGroup);
            var resp = await http.ApiPost(controller_api + "/save", customerGroup);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Change status successfully"], MatToastType.Success);
                if (customer_groups.Count() == 1 && state.pager.current_page > 1)
                {
                    state.pager.current_page = state.pager.current_page - 1;
                }
                await ReloadCustomerGroup();
                await LoadData();
            }
        }    
        public async Task OnDeleteRestore(CustomerGroupModel p,bool is_delete = true)
        {
            p.is_loading = true;
            string conf_title = $"{(is_delete?lang["Delete"] :lang["Restore"])} {lang["Customer Group"]}";
            string conf_message =  $"({p.customer_group_name_en}), {lang["Are you sure to"]} {(is_delete? lang["Delete"] :lang["Restore"])}?";
            string toast_msg =$"{lang["Record"]} ({p.customer_group_name_en}) {lang["Was"]} {(is_delete?lang["Deleted"] :lang["Restored"])}";   
            if (await js.Confirm(conf_title, conf_message))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add(toast_msg, MatToastType.Success);
                    if (customer_groups.Count() == 1 && state.pager.current_page > 0)
                    {
                        state.pager.current_page = state.pager.current_page - 1 ;
                    }
                    await  ReloadCustomerGroup();
                    await LoadData();
                }
            }
            p.is_loading = false;
        }    
        public async Task SelectChange(int perpage)
        {
            state.pager.per_page = perpage;
            state.pager.current_page = 1;
            await LoadData();
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
    }
}
