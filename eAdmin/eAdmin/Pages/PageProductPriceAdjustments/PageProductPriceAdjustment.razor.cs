﻿using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
namespace eAdmin.Pages.PageProducts
{
    public class PageProductPriceAdjustmentBase : PageCore
    {

        public DataModel model { get; set; } = new DataModel();

        public string keyword { get; set; } = "";
        protected override async Task OnInitializedAsync()
        {

            is_loading = true;

            await LoadData();

            is_loading = false;
        }


        public async Task LoadData()
        {
            is_loading = true;
            var resp = await http.ApiPost("GetData", new FilterModel()
            {
                procedure_name= "sp_get_product_for_price_adjustment",
                procedure_parameter = $"'{gv.current_login_user.username}'"
            });
            if (resp.IsSuccess)
            {
                
                model = JsonSerializer.Deserialize<DataModel>(resp.Content.ToString());
            }
            is_loading = false;

        }
        
        public async Task OnPriceChanged(ProductPrice pp , decimal price)
        {
            is_loading_data = true;
            pp.price = price;
            var resp = await http.ApiPost($"Product/UpdateProductPrice/{pp.id}/{price}");
            if (resp.IsSuccess)
            {
                toast.Add(lang["Update price successfully"], MudBlazor.Severity.Success);
            }
            is_loading_data = false;
        }
    }

  


}
