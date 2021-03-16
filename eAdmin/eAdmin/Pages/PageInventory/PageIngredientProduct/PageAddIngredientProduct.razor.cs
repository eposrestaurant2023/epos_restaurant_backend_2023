using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
namespace eAdmin.Pages.PageInventory.PageIngredientProduct
{
    public class PageAddIngredientBase : PageCore
    {

        [Parameter] public int id { get; set; }
        [Parameter] public int clone_id { get; set; }
        public ProductModel model { get; set; } = new ProductModel();
        public string PageTitle
        {
            get
            {

                if (id > 0)
                {
                    return lang["Edit Ingredient"];
                }
                else
                {
                    return lang["New Ingredient"];
                }
            }
        }

        public string role_name
        {
            get
            {
                if (id > 0)
                {
                    return "ingredient_edit";
                }
                else
                {
                    return "ingredient_add";
                }

            }
        }

        public string api_url { get {

                string url = $"Product({id})?$expand=stock_location_products,vendor,unit";
                return url;
            } }

        public bool is_save_and_new { get; set; }



       public void OnCancel()
        {
            if (id == 0)
            {
                nav.NavigateTo("ingredient");
            }else
            {
                nav.NavigateTo("ingredient/" + id);
            }
        }



        protected override async Task OnInitializedAsync()
        {

            is_loading = true;
            if (id > 0)
            {
                await LoadData();
            }else if (clone_id > 0)
            {
                await CloneProduct();
            }
            is_loading = false;
        }


        public async Task LoadData()
        {
            is_loading = true;

            if (id > 0)
            {
                var resp = await http.ApiGet(api_url);
                if (resp.IsSuccess)
                {
                    model = JsonSerializer.Deserialize<ProductModel>(resp.Content.ToString());
                }
            }

            is_loading = false;

        }
        public async Task CloneProduct()
        {
            is_loading = true;


            var resp = await http.ApiPost($"Product/Clone/{clone_id}");
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<ProductModel>(resp.Content.ToString());
            }


            is_loading = false;

        }

        public async Task Save_Click()
        {
            is_saving = true;

            ProductModel save_model = new ProductModel();

            save_model = JsonSerializer.Deserialize<ProductModel>(JsonSerializer.Serialize(model));
 
            //remove menu
            save_model.product_menus.ForEach(r => r.menu = null);
            save_model.stock_location_products.ForEach(r => r.stock_location = null);
            save_model.product_portions = null;
            save_model.is_ingredient_product = true;
            save_model.is_menu_product = false;
            save_model.vendor = null;
            Console.WriteLine(JsonSerializer.Serialize(save_model));
            var resp = await http.ApiPost("Product/Save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save ingredient successfully"], MatToastType.Success);
                if (is_save_and_new)
                {
                    model = new ProductModel();
                    model.product_category_id = save_model.product_category_id;
                    is_save_and_new = false;
                    nav.NavigateTo("ingredient/new");

                }else
                {
                    save_model = JsonSerializer.Deserialize<ProductModel>(resp.Content.ToString());
                    nav.NavigateTo($"ingredient/{save_model.id}"); 
                }
                
            }
            else
            {
                toast.Add(lang[$"{resp.Content.ToString()}"], MatToastType.Warning);
            }
            is_saving = false;


    }


    }
}
