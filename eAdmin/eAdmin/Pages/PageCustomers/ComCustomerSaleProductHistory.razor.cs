
using eModels;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MatBlazor;
using eAdmin.JSHelpers;

namespace eAdmin.Pages.PageCustomers.CustomerDetails
{
    public class ComCustomerSaleProductHistoryBase : PageCore
    { 
        [Parameter] public Guid customer_id { get; set; }
        public List<SaleProductModel> models = new List<SaleProductModel>();
        public int TotalRecord = 0;
        string controller_api = "saleproduct";

        public string StateKey
        {
            get
            {

                return "STOMERPRODUCTsaledmRGrRwdzVOID20154coN" + gv.current_login_user.id + customer_id.ToString(); //Storage and Session Key  
            }
        }
        public string ControllerApi
        {
            get
            {
                if (state.pager.order_by == "id")
                {
                    state.pager.order_by = "created_date";
                    state.pager.order_by_type = "desc";
                }
                string url = $"{controller_api}?$select=id,product_id,sale_id,quantity,price,sale_product_discount_amount,total_tax_amount,total_amount,created_date,created_by,portion_name,&";
                url += $"$expand=sale($select=id,customer_id,business_branch_id,outlet_id,working_date,working_day_number,document_number,cashier_shift_number;$expand=sale_status,outlet($select=id,outlet_name_en,outlet_name_kh),business_branch($select=business_branch_name_en,business_branch_name_kh)),product($select=id,product_name_en,product_name_kh,product_code,photo)";
                url += $"&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";

                return url + GetFilter(state.filters);  
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);

            var default_view = gv.GetDefaultModuleView("page_sale_product");
            if (default_view != null)
            {
                state.page_title = lang[default_view.title];
            }

            if (state.filters.Where(r => r.key == "sale/customer_id").Count() == 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "sale/customer_id",
                    value1 = customer_id.ToString()
                });
            }
            await LoadData();
            //await LoadSaleProductModifier();
        }   

        public async Task LoadData(string api_url="")
        {
            is_loading = true;
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

            //

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

        async Task LoadSaleProductModifier()
        {
            var res = await http.ApiGetOData("SaleProductModifier");
            if (res.IsSuccess)
            {

            }
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
                        filter_title = lang["Sale Date"],
                        filter_info_text = state.date_range.start_date.ToString(gv.date_format) + " - " +state.date_range.end_date.ToString(gv.date_format),
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
            if (state.product_group.id > 0)
            {

                state.filters.Add(new FilterModel()
                {
                    key = "product/product_category/product_group/product_group_en",
                    value1 = state.product_group.id.ToString(),
                    is_clear_all = true,
                    filter_title = lang["Product Group"],
                    state_property_name = "product_group",
                    filter_info_text = state.product_group.product_group_en,
                    filter_operator = "eq",
                    will_remove = true
                });
            }


            if (state.product_category.id > 0)
            {
                //product category
                state.filters.Add(new FilterModel()
                {
                    key = "product/product_category_id",
                    value1 = state.product_category.id.ToString(),
                    is_clear_all = true,
                    filter_title = lang["Product Category"],
                    state_property_name = "product_category",
                    filter_info_text = state.product_category.product_category_en,
                    filter_operator = "eq",
                    will_remove = true
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

    }
}
