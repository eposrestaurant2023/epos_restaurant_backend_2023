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
        [Parameter] public bool is_production_product { get; set; }
        public ProductModel model { get; set; } = new ProductModel();
        public string PageTitle
        {
            get
            {
                if (!is_production_product)
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
                else
                {
                    if (id > 0)
                    {
                        return lang["Edit Production Product"];
                    }
                    else
                    {
                        return lang["New Production Product"];
                    }
                }
                
            }
        }

        public string role_name
        {
            get
            {
                if (!is_production_product)
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
                else
                {
                    if (id > 0)
                    {
                        return "production_edit";
                    }
                    else
                    {
                        return "production_add";
                    }

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
            if (!is_production_product)
            {
                if (id == 0)
                {
                    nav.NavigateTo("ingredient");
                }
                else
                {
                    nav.NavigateTo("ingredient/" + id);
                }
            }
            else
            {
                if (id == 0)
                {
                    nav.NavigateTo("productionproduct");
                }
                else
                {
                    nav.NavigateTo("productionproduct/" + id);
                }
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
            else
            {
                model.is_ingredient_product = true;
                model.is_production_product = is_production_product;
                
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
                    model.unit_category_id = model.unit.unit_category_id;
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
            // assign multipier from unit
            save_model.product_portions.ForEach(r => r.multiplier = r.unit.multiplier);
            //remove menu
            save_model.product_menus.ForEach(r => r.menu = null);
            save_model.stock_location_products.ForEach(r => r.stock_location = null);
            save_model.product_portions = null;
            save_model.is_ingredient_product = true;
            save_model.is_menu_product = false;
            save_model.vendor = null;
            save_model.default_stock_location_products.ForEach(r => { r.station = null;
                r.stock_location = null;
               
            });

            var resp = await http.ApiPost("Product/Save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully"], MudBlazor.Severity.Success);
                if (is_save_and_new)
                {
                    model = new ProductModel();
                    model.product_category_id = save_model.product_category_id;
                    is_save_and_new = false;
                    if (!is_production_product)
                    {
                        nav.NavigateTo("ingredient/new");
                    }
                    else
                    {
                        nav.NavigateTo("productionproduct/new");
                    }
                    

                }else
                {
                    save_model = JsonSerializer.Deserialize<ProductModel>(resp.Content.ToString());
                    if (!is_production_product)
                    {
                        nav.NavigateTo($"ingredient/{save_model.id}");
                    }
                    else
                    {
                        nav.NavigateTo($"productionproduct/{save_model.id}");
                    }
                        
                }
                
            }
            else
            {
                toast.Add(lang[$"{resp.Content.ToString()}"], MudBlazor.Severity.Warning);
            }
            is_saving = false;


    }

        public void onUnitCategoryChange(int val)
        {
            if (model.unit_category_id != val)
            {
                var _data = gv.units.Where(r => r.unit_category_id == val && !r.is_deleted && r.status && r.type_name == "Reference").ToList();

                model.unit_id = _data.Count > 0 ? _data.FirstOrDefault().id : 0;
                model.unit = _data.Count > 0 ? _data.FirstOrDefault() : new UnitModel();
                model.unit_category_id = val;
            }

        }
    }
}
