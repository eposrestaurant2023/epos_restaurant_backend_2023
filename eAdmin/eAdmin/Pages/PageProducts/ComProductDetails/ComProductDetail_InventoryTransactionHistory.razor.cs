
using eModels;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MatBlazor;
using eAdmin.JSHelpers;
namespace eAdmin.Pages.PageInventory.PageVendor.ComVendorDetail
{
    public class InventoryTransactionProductDetailHistoryBase : PageCore
    { 
        [Parameter] public int product_id { get; set; }
        public List<InventoryTransactionModel> models = new List<InventoryTransactionModel>();
        public int TotalRecord = 0;
        string controller_api = "InventoryTransaction";

        public string StateKey
        {
            get
            {

                return "InventoryTranProdyctsaledmRGrRwd5021D20154coN" + gv.current_login_user.id; //Storage and Session Key  
            }
        }

        public string ControllerApi
        {
            get
            {
                state.pager.order_by = "id";
                state.pager.order_by_type = "desc";
                string url = $"{controller_api}?";
                url += $"$expand=inventory_transaction_type,stock_location($select=id,stock_location_name;$expand=business_branch($select=id,business_branch_name_en))";
                url += $"&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";
         
                return url + GetFilter(state.filters);  
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);

            var default_view = gv.GetDefaultModuleView("page_purchase_order");
            if (default_view != null)
            {
                state.page_title = default_view.title;
            }

            if (state.filters.Where(r => r.key == "product_id").Count() == 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "product_id",  
                    value1 = product_id.ToString()
                });
            }

            await LoadData();
        }   

        public async Task LoadData(string api_url="")
        {
            is_loading = true;
            if (state.filters.Where(r => r.key == "stock_location/business_branch_id").Count() == 0)
            {
                //Business Branch Filter
                state.filters.Add(new FilterModel()
                {
                    key = "stock_location/business_branch_id",
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
            if (state.filters.Where(r => r.key == "stock_location_id").Count() == 0)
            {
                //Outlet Filter
                state.filters.Add(new FilterModel()
                {
                    key = "stock_location_id",
                    value1 = gv.stock_location_ids_filter(gv.business_branch_ids_filter_1),
                    filter_title = lang["Stock Location"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.stock_location_ids_filter(gv.business_branch_ids_filter_1),
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
                models = JsonSerializer.Deserialize<List<InventoryTransactionModel>>(resp.Content.ToString());
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
                        key = "transaction_date",
                        value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.start_date),
                        filter_title = lang["Purchase Date"],
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
                    key = "transaction_date",
                    value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.end_date),
                    is_clear_all = true,
                    filter_operator = "Le",
                    will_remove = true,
                    state_property_name = "date_range"
                });
            }



            // customer group
            if (state.inventory_stransation_type != null && state.inventory_stransation_type.id > 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "inventory_transaction_type_id",
                    value1 = state.inventory_stransation_type.id.ToString(),
                    filter_title =lang[ "Inventory Stransation Type"],
                    state_property_name = "inventory_transaction_type",
                    filter_info_text = state.inventory_stransation_type.inventory_transaction_type_name,
                    is_clear_all = true,
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
                    key = "stock_location/business_branch_id",
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
                    key = "stock_location/business_branch_id",
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

            // filter station
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
                    key = "stock_location_id",
                    value1 = value,
                    filter_title = lang["Stock Location"],
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
                    key = "stock_location_id",
                    value1 = gv.stock_location_ids_filter(business_branch_ids),
                    filter_title = lang["Stock Location"],
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.stock_location_ids_filter(business_branch_ids),
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
                if (k == "stock_location/business_branch_id" && state.multi_select_id_1 != null)
                {
                    state.multi_select_id_1.Clear();
                    state.multi_select_value_1.Clear();
                }

                // clear filter outlet
                if (k == "stock_location_id" && state.multi_select_id_2 != null)
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
                if (f.key == "stock_location/business_branch_id")
                {
                    if(state.multi_select_id_1 != null) state.multi_select_id_1.Clear();
                    if (state.multi_select_value_1 != null) state.multi_select_value_1.Clear();
                }

                // clear filter stock location
                if (f.key == "stock_location_id")
                {
                    if (state.multi_select_id_2 != null)
                    {
                        state.multi_select_id_2.Clear();
                    }

                    if (state.multi_select_value_2 != null)
                    {
                        state.multi_select_value_2.Clear();
                    }
                    
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

    }
}
