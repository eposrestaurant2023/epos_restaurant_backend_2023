
using eModels;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MatBlazor;
using eAdmin.JSHelpers;
using eShareModel;
namespace eAdmin.Pages.PageReceipt
{
    public class PageExpenseBase : PageCore
    {
        public List<ExpenseModel> models = new List<ExpenseModel>();
        public ExpenseModel model = new ExpenseModel();
        public int TotalRecord = 0;
        string controller_api = "expense";
        public string StateKey
        {
            get
            {

                return "lissdfdst9hUndmRGrRwdzVOID2sdfsdfds012u9sdfT3AEj" + gv.current_login_user.id; //Storage and Session Key  
            }
        }
        public string ControllerApi
        {
            get
            {
                if (string.IsNullOrEmpty(state.pager.order_by) || state.pager.order_by == "id")
                {
                    state.pager.order_by = "created_date";
                    state.pager.order_by_type = "desc";
                }
                string url = $"{controller_api}?";
               
                url += $"&keyword={GetFilterValue2(state.filters, "keyword", "")}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";

                return url + GetFilter(state.filters);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);
            if (string.IsNullOrEmpty(state.page_title))
            {
                state.page_title = "Expense";
                var default_view = gv.GetDefaultModuleView("page_expense");
                if (default_view != null)
                {
                    state.page_title = lang[default_view.title];
                    state.filters = default_view.filters;
                }
            }
            await LoadData();
            is_loading = false;
        }

        public async Task LoadData(string api_url = "")
        {
            is_loading = true;

            if (state.filters.Where(r => r.key == "is_deleted").Count() == 0)
            {
                //Business Branch Filter
                state.filters.Add(new FilterModel()
                {
                    key = "is_deleted",
                    value1 = "false",
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            if (state.filters.Where(r => r.key == "business_branch_id").Count() == 0)
            {
                //Business Branch Filter
                state.filters.Add(new FilterModel()
                {
                    key = "business_branch_id",
                    value1 = gv.business_branch_ids_filter_1,
                    filter_title = lang["Business Branch"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.business_branch_ids_filter_1,
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            //if (state.filters.Where(r => r.key == "outlet_id").Count() == 0)
            //{
            //    //Outlet Filter
            //    state.filters.Add(new FilterModel()
            //    {
            //        key = "outlet_id",
            //        value1 = gv.outlet_ids_filter(gv.business_branch_ids_filter_1),
            //        filter_title = lang["Outlet"],
            //        filter_operator = "multiple",
            //        state_property_name = "list_selected_values",
            //        filter_info_text = gv.outlet_ids_filter(gv.business_branch_ids_filter_1),
            //        is_clear_all = true,
            //        will_remove = true,
            //        is_show_on_infor = false
            //    });
            //}

            if (string.IsNullOrEmpty(api_url))
            {
                api_url = $"{ControllerApi}";
                state.api_url = api_url;
                await SetState(StateKey, state);
            }

            var resp = await http.ApiGetOData(api_url);
            if (resp.IsSuccess)
            {
                models = JsonSerializer.Deserialize<List<ExpenseModel>>(resp.Content.ToString());
                TotalRecord = resp.Count;
            }
            is_loading = false;
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


        public async Task FilterClick()
        {
            state.filters.RemoveAll(r => r.filter_info_text != "");
            //start date
            if (state.date_range.is_visible)
            {
                state.filters.Add(
                    new FilterModel()
                    {
                        key = "expense_date",
                        value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.start_date),
                        filter_title = lang["Working Date"],
                        filter_info_text = state.date_range.start_date.ToString(gv.date_format) + " - " + state.date_range.end_date.ToString(gv.date_format),
                        filter_operator = "Ge",
                        is_clear_all = true,
                        will_remove = true,
                        state_property_name = "date_range"
                    }
                );

                //end date
                state.filters.Add(new FilterModel()
                {
                    key = "expense_date",
                    value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.end_date),
                    is_clear_all = true,
                    filter_operator = "Le",
                    will_remove = true,
                    state_property_name = "date_range"
                });
            }

            // filter business
            string business_branch_ids = "";
            if (state.multi_select_value_1 != null && state.multi_select_value_1.Any())
            {

                foreach (var x in state.multi_select_value_1)
                {
                    business_branch_ids += x + ",";
                }
                if (!string.IsNullOrEmpty(business_branch_ids))
                {
                    business_branch_ids = business_branch_ids.Substring(0, business_branch_ids.Length - 1);
                }

                state.filters.Add(new FilterModel()
                {
                    key = "business_branch_id",
                    value1 = business_branch_ids,
                    filter_title = lang["Business Branch"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = business_branch_ids,
                    is_clear_all = true,
                    will_remove = true
                });
            }
            else
            {
                state.filters.Add(new FilterModel()
                {
                    key = "business_branch_id",
                    value1 = gv.business_branch_ids_filter_1,
                    filter_title = lang["Business Branch"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.business_branch_ids_filter_1,
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            // filter outlet
            if (state.multi_select_value_2 != null)
            {
                string value = "";
                foreach (var x in state.multi_select_value_2)
                {
                    value += x + ",";
                }
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                state.filters.Add(new FilterModel()
                {
                    key = "outlet_id",
                    value1 = value,
                    filter_title = lang["Outlet"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = value,
                    is_clear_all = true,
                    will_remove = true
                });
            }
            else
            {
                state.filters.Add(new FilterModel()
                {
                    key = "outlet_id",
                    value1 = gv.outlet_ids_filter(business_branch_ids),
                    filter_title = lang["Outlet"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.outlet_ids_filter(business_branch_ids),
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            //// Expense Category
            if (state.expense_category != null && state.expense_category.id != Guid.Empty)
            {
                state.filters.Add(new FilterModel()
            {
                    key = "expense_category_id",
                    value1 = state.expense_category.id.ToString(),
                   filter_title = lang["Expense Category"],
                   state_property_name = "expense category",
                   filter_info_text = state.expense_category.expense_category_name,
                    is_clear_all = true,
                  will_remove = true
                });
            }

            //// Expense Category
            if (state.expense_item != null && state.expense_item.id != Guid.Empty)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "expense_item_id",
                    value1 = state.expense_item.id.ToString(),
                    filter_title = lang["Expense Item"],
                    state_property_name = "expense item",
                    filter_info_text = state.expense_item.expense_item_name,
                    is_clear_all = true,
                    will_remove = true
                });
            }

            state.pager.current_page = 1;
            await LoadData();
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
        public async Task OrderBy(string col_name = "")
        {
            state.pager.order_by = col_name;
            state.pager.order_by_type = (state.pager.order_by_type == "asc" ? "desc" : "asc");
            await LoadData();
        }

        public async Task RemoveFilter(FilterModel f)
        {
            is_loading = true;
            string[] remove_key = f.remove_key.Split(',');
            foreach (var k in remove_key)
            {
                // clear filter business
                if (k == "business_branch_id" && state.multi_select_id_1 != null)
                {
                    state.multi_select_id_1.Clear();
                    state.multi_select_value_1.Clear();
                }


                // clear filter outlet
                if (k == "outlet_id" && state.multi_select_id_2 != null)
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }


                // clear filter expense category
                if (k == "expense_category_id" && state.multi_select_id_2 != null)
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }

                // clear filter expense item
                if (k == "expense_item_id" && state.multi_select_id_2 != null)
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }
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
            foreach (var f in state.filters.Where(r => r.is_clear_all == true))
            {
                // clear filter business
                if (f.key == "business_branch_id")
                {
                    state.multi_select_id_1.Clear();
                    state.multi_select_value_1.Clear();
                }


                // clear filter outlet
                if (f.key == "outlet_id")
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }

                // clear filter expense category
                if (f.key == "expense_category_id")
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }
                // clear filter expense item
                if (f.key == "expense_item_id")
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }

                RemoveFilter(state, f.state_property_name);
            }


            state.filters.RemoveAll(r => r.is_clear_all == true);
            state.pager.current_page = 1;
            await LoadData();
            is_loading = false;
        }

        public async Task OnToogleStatus(ExpenseModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }
        public async Task OnToogleStatusLabel(ExpenseModel p)
        {
            p.is_change_status = true;
            await SaveStatus(p);
            p.is_change_status = false;
        }
        public async Task SaveStatus(ExpenseModel p)
        {
            var expense = new ExpenseModel();
            expense = p;
            expense.status = !expense.status;
            var resp = await http.ApiPost(controller_api + "/save", expense);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Change status successfully"], MudBlazor.Severity.Success);
                if (models.Count() == 1 && state.pager.current_page > 1)
                {
                    state.pager.current_page = state.pager.current_page - 1;
                }

                await LoadData();
            }
        }

        public async Task OnDelete(ExpenseModel p)
        {
            p.is_loading = true;
            if (await js.Confirm(lang["Delete Record"], lang["Are you sure you want to delete this record?"]))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add(lang["Delete record successfully"], MudBlazor.Severity.Success);
                    if (models.Count() == 1 && state.pager.current_page > 0)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task OnRestore(ExpenseModel p)
        {
            p.is_loading = true;
            if (await js.Confirm(lang["Restore Recored"], lang["Are you sure you want to restore this record?"]))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);

                if (resp.IsSuccess)
                {
                    if (models.Count() == 1 && state.pager.current_page > 1)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
                toast.Add(lang["Restore record successfully"], MudBlazor.Severity.Success);
            }
            p.is_loading = false;
        }


    }
}
