using eModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using eAdmin.JSHelpers;

namespace eAdmin.Pages.PageProducts
{
    public class PageDiscountPromotionBase:PageCore
    {
        public List<DiscountPromotionModel> discount_promotions = new List<DiscountPromotionModel>();
        public DiscountPromotionModel model = new DiscountPromotionModel();

        public int TotalRecord = 0;
        public string StateKey
        {
            get
            {
                return "rivvMbRyefrHD4G8vWdlP2t5gzZpAcvbuyapxoRk" + gv.current_login_user.id; //Storage and Session Key  
            }
        }

        string controller_api = "DiscountPromotion";
        public string ControllerApi
        {
            get
            {                                               

                string url = $"{controller_api}?";
                url = url + $"$expand=business_branch&$count=true&$top={state.pager.per_page}&$skip={state.pager.per_page * (state.pager.current_page - 1)}&$orderby={state.pager.order_by} {state.pager.order_by_type}"; 
                return url + GetFilter(state.filters);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);
            if (state.page_title == "")
            {
                state.page_title = lang["Happy Hour"];
                var default_view = gv.GetDefaultModuleView("page_discount_promotion");
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
                discount_promotions = JsonSerializer.Deserialize<List<DiscountPromotionModel>>(resp.Content.ToString());
                TotalRecord = resp.Count;
            }
            is_loading = false;
        }
        public async Task OrderBy(string col_name = "")
        {

            state.pager.order_by = col_name;
            state.pager.order_by_type = (state.pager.order_by_type == "asc" ? "desc" : "asc");

            //await LoadData();
        }
        public async Task ViewClick(ModuleViewModel m)
        {
            state.filters.Clear();
            state.filters = m.filters;
            state.pager.order_by = m.default_order_by;
            state.pager.order_by_type = m.default_order_by_type;
            state.page_title = m.title;
            state.pager.current_page = 1;
            //await LoadData();
        }

        public async Task RemoveFilter(FilterModel f)
        {
            is_loading = true;
            string[] remove_key = f.remove_key.Split(',');
            foreach (var k in remove_key)
            {
                state.filters.RemoveAll(r => r.key == k);
            }
            if (f.key == "business_branch_ids")
            {
                state.multi_select_id_1.Clear();
                state.multi_select_value_1.Clear();
            }

            state.pager.current_page = 1;
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

        public void OnCloneClick(Guid id)
        {
            is_loading_data = true;
            nav.NavigateTo($"happyhour/clone/{id}");
            is_loading_data = false;
        }

        public void OnEditClick(Guid id)
        {
            is_loading_data = true;
            nav.NavigateTo($"happyhour/edit/{id}");
            is_loading_data = false;
        }


        public async Task OnToggleStatusLabel(DiscountPromotionModel p)
        {
            p.is_change_status = true;
            await SaveStatus(p);
            p.is_change_status = false;
        }
        public async Task OnToggleStatus(DiscountPromotionModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }



        public async Task SaveStatus(DiscountPromotionModel p)
        {
            var discount_promotion = new DiscountPromotionModel();
            discount_promotion = p;
            discount_promotion.status = !discount_promotion.status;
            var resp = await http.ApiPost(controller_api + "/UpdateStatus/" + discount_promotion.id);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Change status successfully"], MudBlazor.Severity.Success);
                if (discount_promotions.Count() == 1 && state.pager.current_page > 1)
                {
                    state.pager.current_page = state.pager.current_page - 1;
                }
                await LoadData();
            }
            else
            {
                toast.Add(resp.Content.ToString(), MudBlazor.Severity.Error);
                discount_promotion.status = discount_promotion.status;
            }
        }


        public async Task OnDeleteRestoreClick(DiscountPromotionModel p,bool is_restore=false)
        {
            p.is_loading = true;
            if (await js.Confirm(lang[$"{(is_restore?"Restore":"Delete")} Happy Hour"], lang[$"Are you sure you want to {(is_restore?"restore":"delete")} this record?"]))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add(lang["Delete Happy Hour successfully"], MudBlazor.Severity.Success);
                    if (discount_promotions.Count() == 1 && state.pager.current_page > 0)
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
