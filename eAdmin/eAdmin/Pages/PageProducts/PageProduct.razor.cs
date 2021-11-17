using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageProducts
{
    public  class PageProductBase : PageCore
    {
        public List<ProductModel> products = new List<ProductModel>();
        public ProductModel model = new ProductModel();
         


        public int TotalRecord = 0;

        public string StateKey
        {
            get
            {

                return "278484567Gs252sd45KJssHGytkjhTonB3PCz2Ts" + gv.current_login_user.id; //Storage and Session Key  
            }
        }
        string controller_api = "Product";

       
        public string ControllerApi
        {
            get
            {
                
                string url = $"{controller_api}?";
                url = url + $"$expand=product_category($select=product_category_en),unit($select=unit_name,id,unit_category_id)&keyword={GetFilterValue2(state.filters, "keyword", "").ToString()}&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}";
                return url + GetFilter(state.filters) + " and is_menu_product eq true";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);
            if (state.page_title == "")
            {
                state.page_title = "Products";
                var default_view = gv.GetDefaultModuleView("page_product");
                if (default_view != null)
                {
                    state.page_title = lang[default_view.title];
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

        public void OnEdit(int id)
        {
            is_loading_data = true;
            nav.NavigateTo($"product/edit/{id}");
            is_loading_data = false;
        }

        public void Clone_Click(int id)
        {
            is_loading_data = true;
            nav.NavigateTo($"product/clone/{id}");
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
                products = JsonSerializer.Deserialize<List<ProductModel>>(resp.Content.ToString());
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

        public async Task OnToogleStatus(ProductModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }
        public async Task OnToogleStatusLabel(ProductModel p)
        {
            p.is_change_status = true;
            await SaveStatus(p);
            p.is_change_status = false;
        }
        public async Task SaveStatus(ProductModel p)
        {
            var product = new ProductModel();
            product = p;
            product.status = !product.status;
            var resp = await http.ApiPost(controller_api + "/ChangeStatus/"+product.id);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Change status successfully"], MatToastType.Success);
                if (products.Count() == 1 && state.pager.current_page > 1)
                {
                    state.pager.current_page = state.pager.current_page - 1;
                }

                await LoadData();
            }
        }

        public async Task OnDelete(ProductModel p)
        {
            p.is_loading = true;
            if (await js.Confirm(lang["Delete Product"], "Are you sure you want to delete this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add(lang["Delete product successfully"], MatToastType.Success);
                    if (products.Count() == 1 && state.pager.current_page > 0)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task OnRestore(ProductModel p)
        {
            p.is_loading = true;
            if (await js.Confirm(lang["Restore product"], lang["Are you sure you want to restore this record?"]))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);

                if (resp.IsSuccess)
                {
                    if (products.Count() == 1 && state.pager.current_page > 1)
                    {
                        state.pager.current_page = state.pager.current_page - 1;
                    }
                    await LoadData();
                }
                toast.Add(lang["Restore product successfully"], MatBlazor.MatToastType.Success);
                
            }
            p.is_loading = false;
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

            if (state.product_group != null && state.product_group.id > 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "product_category/product_group_id",
                    value1 = state.product_group.id.ToString(),
                    filter_title = lang["Product Group"],
                    state_property_name = "product_group",
                    filter_info_text = state.product_group.product_group_en,
                    is_clear_all = true,
                    will_remove = true
                });
            }

            if (state.product_category != null && state.product_category.id > 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "product_category_id",
                    value1 = state.product_category.id.ToString(),
                    filter_title = lang["Product Category"],
                    state_property_name = "product_category",
                    filter_info_text = state.product_category.product_category_en,
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
