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
        [Parameter] public int id { get; set; }
        [Parameter] public int clone_id { get; set; }
        public DiscountPromotionModel model { get; set; } = new DiscountPromotionModel();


        public string PageTitle
        {
            get
            {
                if (id > 0)
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
                if (id > 0)
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
            //if (clone_id > 0)
            //{
            //    await CloneProduct();
            //}
            //else
            //{
            //    if (id == 0)
            //    {
            //        model.unit = gv.units.Where(r => r.id == model.unit_id).FirstOrDefault();
            //        model.unit_category_id = gv.units.Where(r => r.id == model.unit_id).FirstOrDefault().unit_category_id;
            //        await LoadData();
            //    }
            //    else if (id > 0)
            //    {
            //        await LoadData();
            //    }
            //}

            is_loading = false;
        }

        public async Task OnSaveClick()
        {
                     //
        }


        public void OnCancelClick()
        {      
                nav.NavigateTo("happyhour");     
        }      
    }
}
