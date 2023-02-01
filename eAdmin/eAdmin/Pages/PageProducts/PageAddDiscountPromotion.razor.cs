using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eModels;

namespace eAdmin.Pages.PageProducts
{
    public class PageAddEditDiscountPromotionBase:PageCore
    {
        [Parameter] public Guid id { get; set; }
        public DiscountPromotionModel model { get; set; } = new DiscountPromotionModel();
        public TimeSpan? start_time = new TimeSpan(0, 0, 0);
        public TimeSpan? end_time = new TimeSpan(0, 0, 0);


        public string PageTitle
        {
            get
            {
                if (id !=Guid.Empty)
                {
                    return lang["Edit Happy Hour"];
                }
                else
                {
                    return lang["New Happy Hour"];
                }
            }
        }

        public string role_name
        {
            get
            {
                if (id != Guid.Empty)
                {
                    return "happy_hour_edit";
                }
                else
                {
                    return "happy_hour_add";
                }
            }
        }
        protected override async Task OnInitializedAsync()
        {

            is_loading = true;

            await onLoadData();


            is_loading = false;
        }

        async Task onLoadData()
        {
            if (id != Guid.Empty)
            {
                var resp = await http.ApiGet($"DiscountPromotion({id})?$expand=discount_promotion_items");
                if (resp.IsSuccess)
                {
                    model = System.Text.Json.JsonSerializer.Deserialize<DiscountPromotionModel>(resp.Content.ToString());
                    start_time = TimeSpan.FromTicks((model.start_time??DateTime.Now).Ticks);
                    end_time = TimeSpan.FromTicks((model.end_time??DateTime.Now).Ticks);
                }
            }
        }

        public async Task OnSaveClick()
        {
            if(model.business_branch_id == Guid.Empty)  {
                toast.Add(lang["Please select a business branch"],MudBlazor.Severity.Warning);
                return;
            }
            if (string.IsNullOrEmpty(model.title))
            {
                toast.Add(lang["Title cannot be empty"], MudBlazor.Severity.Warning);
                return;
            }    
            model.start_time = new DateTime(model.start_date.Value.Year, model.start_date.Value.Month, model.start_date.Value.Day, start_time.Value.Hours, start_time.Value.Minutes, 0);
            model.end_time =new  DateTime(model.end_date.Value.Year, model.end_date.Value.Month, model.end_date.Value.Day, end_time.Value.Hours, end_time.Value.Minutes, 0);

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));

            if ((model.is_base_on_hour && model.end_time < model.start_time) || model.end_date < model.start_date )
            {
                toast.Add(lang["Invalid date range"], MudBlazor.Severity.Warning);
                return;
            }
            if(model.discount_promotion_items != null)
            {
                if(model.discount_promotion_items.Where(r=>(r.product_category_id<=0 || r.discount_value<0) && !r.is_deleted ).Any())
                {
                    toast.Add(lang["Please verify discount of product category"], MudBlazor.Severity.Warning);
                    return;
                }
            }
            model.business_branch = null;     
            var resp = await http.ApiPost("DiscountPromotion/Save", model);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully"], MudBlazor.Severity.Success);           
                nav.NavigateTo($"happyhour");
            }
            else
            {
                toast.Add(resp.Content.ToString(), MudBlazor.Severity.Error);
            }


                     //
        }

        public void onAddRecord()
        {
            if (model.discount_promotion_items == null)
            {
                model.discount_promotion_items = new List<DiscountPromotionItemModel>();
            }
            model.discount_promotion_items.Add(new DiscountPromotionItemModel()
            {
                discount_value = 0
            }) ;
        }

        public void onDeleteRecordClick(DiscountPromotionItemModel m)
        {
            if (m.id != Guid.Empty)
            {
                m.is_deleted = true;
            }
            else
            {
                model.discount_promotion_items.Remove(m);
            }
        }


        public void OnCancelClick()
        {      
                nav.NavigateTo("happyhour");     
        }      
    }
}
