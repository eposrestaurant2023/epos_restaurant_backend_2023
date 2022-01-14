
using eModels;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MatBlazor;
using eAdmin.JSHelpers;

namespace eAdmin.Pages.PageReceipt
{
    public class ComRedeemBase : PageCore
    {
        public List<SaleProductModel> models = new();
        public SaleProductModel model = new();
        public int TotalRecord = 0;
        string controller_api = "saleproduct";
        public string StateKey
        {
            get
            {
                return "RedeemxXlPwdser293nLKTwgoZhToVFJscPE0" + gv.current_login_user.id; //Storage and Session Key  
            }
        }
        public string ControllerApi
        {
            get
            {
                if (string.IsNullOrEmpty(state.pager.order_by) || state.pager.order_by == "id")
                {
                    state.pager.order_by = "sale/working_date";
                    state.pager.order_by_type = "desc";
                }
                string url = $"{controller_api}?";
                url += $"$select=id,sale_id,total_amount,station_name_en,station_name_kh,station_id,quantity,price,seat_number,is_redeem_park,park_note,park_expired_date";
                url += $"&$expand=product($select=id,product_code,product_name_en,product_name_kh,photo),sale($select=id,customer_id,document_number,working_date,working_day_number,cashier_shift_number,sale_number,sale_type;$expand=sale_status,customer($select=id,customer_name_en,customer_name_kh,customer_code,photo),outlet($select=id,outlet_name_en,outlet_name_kh),business_branch($select=business_branch_name_en,business_branch_name_kh))";
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
                state.page_title = "Redeem List";
            }
            //Business Branch Filter
            if (state.filters.Where(r => r.key == "is_redeem_park").Count() == 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "is_redeem_park",
                    value1 = "true",
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
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
            if (state.filters.Where(r => r.key == "sale/business_branch_id").Any())
            {
                state.filters.RemoveAll(r => r.key == "sale/business_branch_id");
            }
            if (state.filters.Where(r => r.key == "sale/business_branch_id").Count() == 0)
            {
                //Business Branch Filter
                state.filters.Add(new FilterModel()
                {
                    key = "sale/business_branch_id",
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
            if (state.filters.Where(r => r.key == "sale/outlet_id").Any())
            {
                state.filters.RemoveAll(r => r.key == "sale/outlet_id");
            }
            if (state.filters.Where(r => r.key == "sale/outlet_id").Count() == 0)
            {
                //Outlet Filter
                state.filters.Add(new FilterModel()
                {
                    key = "sale/outlet_id",
                    value1 = gv.outlet_ids_filter(gv.business_branch_ids_filter_1),
                    filter_title = lang["Outlet"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.outlet_ids_filter(gv.business_branch_ids_filter_1),
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            if (string.IsNullOrEmpty(api_url))
            {
                api_url = $"{ControllerApi}";
                state.api_url = api_url;
                await SetState(StateKey, state);
            }

            var resp = await http.ApiGetOData(api_url);
            if (resp.IsSuccess)
            {
                models = JsonSerializer.Deserialize<List<SaleProductModel>>(resp.Content.ToString());
                TotalRecord = resp.Count;
            }
            is_loading = false;
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
                        key = "sale/working_date",
                        value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.start_date),
                        filter_title = lang["Redeem Date"],
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
                    key = "sale/working_date",
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
                    key = "sale/business_branch_id",
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
                    key = "sale/business_branch_id",
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
            if (state.multi_select_value_2 != null && state.multi_select_id_2.Any())
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
                    key = "sale/outlet_id",
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
                    key = "sale/outlet_id",
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
                if (k == "sale/business_branch_id" && state.multi_select_id_1 != null)
                {
                    state.multi_select_id_1.Clear();
                    state.multi_select_value_1.Clear();
                }

                // clear filter outlet
                if (k == "sale/outlet_id" && state.multi_select_id_2 != null)
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
                if (f.key == "sale/business_branch_id")
                {
                    state.multi_select_id_1.Clear();
                    state.multi_select_value_1.Clear();
                }


                // clear filter outlet
                if (f.key == "sale/outlet_id")
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

    }
}
