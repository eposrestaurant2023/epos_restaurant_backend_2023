
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
    public class PageReceiptListBase : PageCore
    {

        [Parameter] public bool is_receipt_list { get; set; }
        [Parameter] public int visit_id { get; set; }
        public List<SaleModel> models = new List<SaleModel>();
        public SaleModel model = new SaleModel();
        public string StateKey = "";   
        public int TotalRecord = 0; 

        string controller_api = "sale";
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
                url += $"$expand=customer($select=id,customer_name_en,customer_name_kh,customer_code,photo),stock_location,outlet($select=id,outlet_name_en,outlet_name_kh;$expand=business_branch($select=business_branch_name_en,business_branch_name_kh))";
                url += $"&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";

                return url + GetFilter(state.filters);  
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            if (is_receipt_list)
                StateKey = "Elist9hUndmRGRECEIPTnBau9T3AEj";
            else
                StateKey = "list9hUndmRGrRwdzVOID2012u9T3AEj";

            state = await GetState(StateKey); 
            if (state.page_title == "")
            {
                if (is_receipt_list)
                    state.page_title = "Receipt List";
                else
                    state.page_title = "Void Receipt";

                var default_view = gv.GetDefaultModuleView("page_sale");
                if (default_view != null)
                {
                    state.page_title = default_view.title;
                    state.filters = default_view.filters;
                }    
            } 
            if (state.filters.Count == 0)
            {
                if (is_receipt_list)
                {
                    state.filters.Add(new FilterModel()
                    {
                        key = "is_deleted",
                        value1 = "false"
                    });
                }
                else
                {
                    state.filters.Add(new FilterModel()
                    {
                        key = "is_deleted",
                        value1 = "true"
                    });
                }
                                
            }

            Console.WriteLine(JsonSerializer.Serialize(state.filters));


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
                models = JsonSerializer.Deserialize<List<SaleModel>>(resp.Content.ToString());
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
                        key = "sale_date",
                        value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.start_date),
                        filter_title = "Sale Date",
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
                    key = "sale_date",
                    value1 = string.Format("{0:yyyy-MM-dd}", state.date_range.end_date),
                    is_clear_all = true,
                    filter_operator = "Le",
                    will_remove = true,
                    state_property_name = "date_range"
                });  
            }
            // customer
            if (state.customer != null)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "customer_id",
                    value1 = state.customer.id.ToString(),
                    filter_title = "Customer",
                    state_property_name = "customer",
                    filter_info_text = state.customer.customer_code_name,
                    is_clear_all = true,
                    will_remove = true
                });
            }

            // filter business
            if (state.multi_select_value_1 != null)
            {

                string value = "";
                foreach(var x in state.multi_select_value_1)
                {
                    value += x + ",";
                }
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Substring(0, value.Length - 1);
                } 

                state.filters.Add(new FilterModel()
                {
                    key = "outlet/business_branch_id",
                    value1 = value,
                    filter_title = "Business Branch",
                    filter_operator = "multiple",
                    state_property_name = "list_selected_values",
                    filter_info_text = value,
                    is_clear_all = true,
                    will_remove = true
                });
            }
            // filter outlet
            if (state.multi_select_value_2 != null)
            {

                string value = "";
                foreach(var x in state.multi_select_value_2)
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
                    filter_title = "Outlet",
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

        public async Task OnDelete(SaleModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("Delete Sale", "Are you sure you want to delete this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add("Delete sale successfully", MatToastType.Success);
                    if (models.Count() == 1 && state.pager.current_page > 0)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task RemoveFilter(FilterModel f)
        {
            is_loading = true;
            string[] remove_key = f.remove_key.Split(',');
            foreach (var k in remove_key)
            {
                // clear filter business
                if (k == "outlet/business_branch_id")
                {
                    state.multi_select_guid_1.Clear();
                    state.multi_select_value_1.Clear();
                }
                    

                // clear filter outlet
                 if (k == "outlet_id")
                {
                    state.multi_select_guid_2.Clear();
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
                if (f.key == "outlet/business_branch_id")
                {
                    state.multi_select_guid_1.Clear();
                    state.multi_select_value_1.Clear();
                }


                // clear filter outlet
                if (f.key == "outlet_id")
                {
                    state.multi_select_guid_2.Clear();
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
