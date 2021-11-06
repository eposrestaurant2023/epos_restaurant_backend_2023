
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
    public class StockStransferProductDetailHistoryBase : PageCore
    { 
        [Parameter] public int product_id { get; set; }
        public List<StockTransferProductModel> models = new List<StockTransferProductModel>();
        public int TotalRecord = 0;
        string controller_api = "StockTransferProduct";

        public string StateKey
        {
            get
            {

                return "STOSTOCKSFTANProdyctsaledmRGrRwd5021D20154coN" + gv.current_login_user.id; //Storage and Session Key  
            }
        }
        public string ControllerApi
        {
            get
            {
                if (string.IsNullOrEmpty(state.pager.order_by))
                {
                    state.pager.order_by = "id";
                    state.pager.order_by_type = "desc";
                }
                string url = $"{controller_api}?";
                url += $"$expand=stock_transfer($select=id,from_business_branch_id,from_stock_location_id,to_business_branch_id,to_stock_location_id,document_number,stock_transfer_date,reference_number,is_fulfilled;$expand=from_business_branch($select=id,business_branch_name_en,business_branch_name_kh),to_business_branch($select=id,business_branch_name_en,business_branch_name_kh),from_stock_location,to_stock_location)";
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
            if (state.filters.Where(r => r.key == "stock_transfer/from_business_branch_id").Count() == 0)
            {
                //From  Business Branch Filter
                state.filters.Add(new FilterModel()
                {
                    key = "stock_transfer/from_business_branch_id",
                    value1 = gv.business_branch_ids_filter_1,
                    filter_title = "From Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.business_branch_ids_filter_1,
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }
            if (state.filters.Where(r => r.key == "stock_transfer/from_stock_location_id").Count() == 0)
            {
                //From  Stock Location  Filter
                state.filters.Add(new FilterModel()
                {
                    key = "stock_transfer/from_stock_location_id",
                    value1 = gv.stock_location_ids_filter(gv.business_branch_ids_filter_1),
                    filter_title = "From Stock Location",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.stock_location_ids_filter(gv.business_branch_ids_filter_1),
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            if (state.filters.Where(r => r.key == "stock_transfer/to_business_branch_id").Count() == 0)
            {
                //To  Business Branch Filter
                state.filters.Add(new FilterModel()
                {
                    key = "stock_transfer/to_business_branch_id",
                    value1 = gv.business_branch_ids_filter_2,
                    filter_title = "To Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.business_branch_ids_filter_2,
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }
            if (state.filters.Where(r => r.key == "stock_transfer/to_stock_location_id").Count() == 0)
            {
                //to  Stock Location  Filter
                state.filters.Add(new FilterModel()
                {
                    key = "stock_transfer/to_stock_location_id",
                    value1 = gv.stock_location_ids_filter(gv.business_branch_ids_filter_2),
                    filter_title = "to Stock Location",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.stock_location_ids_filter(gv.business_branch_ids_filter_2),
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
                models = JsonSerializer.Deserialize<List<StockTransferProductModel>>(resp.Content.ToString());
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
                        key = "stock_transfer/stock_transfer_date",
                        value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.start_date),
                        filter_title = "Stock Transfer Date",
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
                    key = "stock_transfer/stock_transfer_date",
                    value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.end_date),
                    is_clear_all = true,
                    filter_operator = "Le",
                    will_remove = true,
                    state_property_name = "date_range"
                });
            }


            // filter from  business
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
                    key = "stock_transfer/from_business_branch_id",
                    value1 = business_branch_ids,
                    filter_title = "From Business Branch",
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
                    key = "stock_transfer/from_business_branch_id",
                    value1 = gv.business_branch_ids_filter_1,
                    filter_title = "From Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.business_branch_ids_filter_1,
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            // filter from stock location
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
                    key = "stock_transfer/from_stock_location_id",
                    value1 = value,
                    filter_title = "From Stock Location",
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
                    key = "stock_transfer/from_stock_location_id",
                    value1 = gv.stock_location_ids_filter(business_branch_ids),
                    filter_title = "From Stock Location",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.stock_location_ids_filter(business_branch_ids),
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            // filter to  business
            string to_business_branch_ids = "";
            if (state.multi_select_id_3 != null && state.multi_select_value_1.Any())
            {

                foreach (var x in state.multi_select_value_3)
                {
                    to_business_branch_ids += x + ",";
                }
                if (!string.IsNullOrEmpty(to_business_branch_ids))
                {
                    to_business_branch_ids = to_business_branch_ids.Substring(0, to_business_branch_ids.Length - 1);
                }

                state.filters.Add(new FilterModel()
                {
                    key = "stock_transfer/to_business_branch_id",
                    value1 = to_business_branch_ids,
                    filter_title = "To Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = to_business_branch_ids,
                    is_clear_all = true,
                    will_remove = true
                });
            }
            else
            {
                state.filters.Add(new FilterModel()
                {
                    key = "stock_transfer/to_business_branch_id",
                    value1 = gv.business_branch_ids_filter_2,
                    filter_title = "To Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = gv.business_branch_ids_filter_2,
                    is_clear_all = true,
                    will_remove = true,
                    is_show_on_infor = false
                });
            }

            // filter to stock location
            if (state.multi_select_value_4 != null)
            {
                string value = "";
                foreach (var x in state.multi_select_value_4)
                {
                    value += x + ",";
                }
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                state.filters.Add(new FilterModel()
                {
                    key = "stock_transfer/to_stock_location_id",
                    value1 = value,
                    filter_title = "To Stock Location",
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
                    key = "stock_transfer/to_stock_location_id",
                    value1 = gv.stock_location_ids_filter(business_branch_ids),
                    filter_title = "To Stock Location",
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
                // clear filter from business
                if (k == "stock_transfer/from_business_branch_id" && state.multi_select_id_1 != null)
                {
                    state.multi_select_id_1.Clear();
                    state.multi_select_value_1.Clear();
                }

                // clear filter from stock location
                if (k == "stock_transfer/from_stock_location_id" && state.multi_select_id_2 != null)
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }

                // clear filter to business
                if (k == "stock_transfer/to_business_branch_id" && state.multi_select_id_3 != null)
                {
                    state.multi_select_id_3.Clear();
                    state.multi_select_value_3.Clear();
                }

                // clear filter to stock location
                if (k == "stock_transfer/to_stock_location_id" && state.multi_select_id_4 != null)
                {
                    state.multi_select_id_4.Clear();
                    state.multi_select_value_4.Clear();
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
                // clear filter from business
                if (f.key == "stock_transfer/from_business_branch_id")
                {
                    state.multi_select_id_1.Clear();
                    state.multi_select_value_1.Clear();
                }

                // clear filter from stock location
                if (f.key == "stock_transfer/from_stock_location_id")
                {
                    state.multi_select_id_2.Clear();
                    state.multi_select_value_2.Clear();
                }

                // clear filter to business
                if (f.key == "stock_transfer/to_business_branch_id")
                {
                    state.multi_select_id_3.Clear();
                    state.multi_select_value_3.Clear();
                }

                // clear filter to stock location
                if (f.key == "stock_transfer/to_stock_location_id")
                {
                    state.multi_select_id_4.Clear();
                    state.multi_select_value_4.Clear();
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
