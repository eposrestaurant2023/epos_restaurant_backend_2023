
using eModels;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MatBlazor;
using eAdmin.JSHelpers;

namespace eAdmin.Pages.PageInventory.PageStockTransfer
{
    public class PageStockTransferBase : PageCore
    { 
        public List<StockTransferModel> models = new List<StockTransferModel>();
        public StockTransferModel model = new StockTransferModel();       
        public string StateKey = "TRANSFERndmRGPUSTOCKe09830212"; //Storage and Session Key      
        public int TotalRecord = 0;

        string controller_api = "StockTransfer";
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
                url += $"$expand=to_business_branch($select=business_branch_name_en),to_stock_location($select=stock_location_name),";
                url += $"from_business_branch($select=business_branch_name_en),from_stock_location($select=stock_location_name)";
                url += $"&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";

                return url + GetFilter(state.filters);  
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
             
            state = await GetState(StateKey);
            state.filters.Clear();
            if (state.page_title == "")
            {
                state.page_title = "Stock Transfer";
                var default_view = gv.GetDefaultModuleView("page_stock_transfer");
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
        }   

        public async Task LoadData(string api_url="")
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
                models = JsonSerializer.Deserialize<List<StockTransferModel>>(resp.Content.ToString());
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

        public async Task AddNew()
        {
            await Task.Delay(100);
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
                        key = "stock_transfer_date",
                        value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.start_date),
                        filter_title = "Stock Transfer Date",
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
                    key = "stock_transfer_date",
                    value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.end_date),
                    is_clear_all = true,
                    filter_operator = "Le",
                    will_remove = true,
                    state_property_name = "date_range"
                });  
            }
            // filter from business
            if (state.multi_select_value_1 != null)
            {

                string value = "";
                foreach (var x in state.multi_select_value_1)
                {
                    value += x + ",";
                }
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                state.filters.Add(new FilterModel()
                {
                    key = "from_business_branch_id",
                    value1 = value,
                    filter_title = "From Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = value,
                    is_clear_all = true,
                    will_remove = true
                });
            }

            // filter from outlet
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
                    key = "from_stock_location_id",
                    value1 = value,
                    filter_title = "From Stock Location",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = value,
                    is_clear_all = true,
                    will_remove = true
                });
            }

            // filter to business
            if (state.multi_select_value_3 != null)
            {

                string value = "";
                foreach (var x in state.multi_select_value_3)
                {
                    value += x + ",";
                }
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                state.filters.Add(new FilterModel()
                {
                    key = "to_business_branch_id",
                    value1 = value,
                    filter_title = "To Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = value,
                    is_clear_all = true,
                    will_remove = true
                });
            }

            // filter to outlet
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
                    key = "to_stock_location_id",
                    value1 = value,
                    filter_title = "To Stock Location",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = value,
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
            foreach (var f in state.filters.Where(r => r.is_clear_all == true))
            {
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

        public async Task OnDelete(StockTransferModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("Delete Stock Transfer", "Are you sure you want to delete this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add("Delete Stock Transfer successfully", MatToastType.Success);
                    if (models.Count() == 1 && state.pager.current_page > 0)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
            }
            p.is_loading = false;
        }
    }
}
