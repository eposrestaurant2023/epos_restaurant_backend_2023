using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace eAdmin.Pages.PageInventory.PageIngredientProduct.ComIngredientProductDetail
{
    public  class AssociationModifierBase : PageCore
    {
        [Parameter] public int ingredient_id { get; set; }
        public List<ModifierIngredientModel> models = new List<ModifierIngredientModel>();
        
        public string StateKey = "INGRMODIfierxxd45KJssASSOciate21"; //Storage and Session Key

        public int TotalRecord = 0;
       

        string controller_api = "ModifierIngredient";

       
        public string ControllerApi
        {
            get
            {
                if (state.pager.order_by == "id")
                {
                    state.pager.order_by = "modifier_id";
                }
                string url = $"{controller_api}?$expand=modifier&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";
                
                return url + GetFilter(state.filters);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);

            state.filters.Clear(); 
            state.filters.Add(new FilterModel()
            {
                key = "ingredient_id",
                value1 = ingredient_id.ToString()
            });
            await LoadData(state.api_url);
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
                models = JsonSerializer.Deserialize<List<ModifierIngredientModel>>(resp.Content.ToString());
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
  
        public async Task SelectChange(int perpage)
        {
            state.pager.per_page = perpage;
            state.pager.current_page = 1;
            await LoadData();
        }

        public async Task FilterClick()
        {
            state.filters.RemoveAll(r => r.filter_info_text != "");

            //if (state.product_group != null && state.product_group.id > 0)
            //{
            //    state.filters.Add(new FilterModel()
            //    {
            //        key = "modifier/modifier_group_id",
            //        value1 = state.product_group.id.ToString(),
            //        filter_title = lang["Modifier Group"],
            //        state_property_name = "modifier_group",
            //        filter_info_text = state.product_group.product_group_en,
            //        is_clear_all = true,
            //        will_remove = true
            //    });
            //}

            //if (state.product_category != null && state.product_category.id > 0)
            //{
            //    state.filters.Add(new FilterModel()
            //    {
            //        key = "product/product_category_id",
            //        value1 = state.product_category.id.ToString(),
            //        filter_title = lang["Product Category"],
            //        state_property_name = "product_category",
            //        filter_info_text = state.product_category.product_category_en,
            //        is_clear_all = true,
            //        will_remove = true
            //    });
            //}

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
