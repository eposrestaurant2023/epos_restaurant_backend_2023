using eAdmin.JSHelpers;
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
                toast.Add(lang["Update price successfully"], MatToastType.Success);
            }
            is_loading_data = false;
        }
    }

   public class DataModel
    {
        public List<PriceRule> price_rules { get; set; }
        public List<Category> categories { get; set; }

    }
    public class Category
    {
        public int id { get; set; }
        public string product_category_en { get; set; }

        public List<Product> product { get; set; }




    }
    public class PriceRule
    {
        public int id { get; set; }
        public string price_name  { get; set; }
    }
    public class Product
    {
        public int id { get; set; }
        public int c_id { get; set; }
        public string p_name { get; set; }
        public List<ProductPortion> product_portion { get; set; }

        public int total_product_portion{ get { return product_portion.Count(); } }

    }  
    public class ProductPortion
    {
        public ProductPortion()
        {
            product_price = new List<ProductPrice>();
        }
        public int id { get; set; }
        public int p_id { get; set; }
        public string p_name { get; set; }
        public List<ProductPrice> product_price { get; set; }
    }    
    
    public class ProductPrice
    {
        public int id { get; set; }
        public int pp_id { get; set; }
        public int pr_id { get; set; }
        public decimal price { get; set; }
    }


}
