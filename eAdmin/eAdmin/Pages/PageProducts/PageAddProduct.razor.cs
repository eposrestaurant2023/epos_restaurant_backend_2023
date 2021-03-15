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
    public class PageAddProductBase : PageCore
    {

        [Parameter] public int id { get; set; }
        [Parameter] public int clone_id { get; set; }
        public ProductModel model { get; set; } = new ProductModel();
        ApiResponseModel error_saving_info = new ApiResponseModel();
        public int unit_category_id { get; set; } = 0;
        public int old_unit_category_id { get; set; } = 0;
        public string PageTitle
        {
            get
            {

                if (id > 0)
                {
                    return lang["Edit Product"];
                }
                else
                {
                    return lang["New Product"];
                }
            }
        }

        public string role_name
        {
            get
            {
                if (id > 0)
                {
                    return "product_edit";
                }
                else
                {
                    return "product_add";
                }

            }
        }

        public string api_url { get {

                string url = $"Product({id})?";
                url = url + "$expand=product_printers,";
                url = url + "product_portions($expand=product_prices,unit;$filter=is_deleted eq false),";
                url = url + "product_menus($expand=menu;$filter=is_deleted eq false),";
                url = url + "product_modifiers($expand=children($expand=modifier;$filter=is_deleted eq false);$filter=is_deleted eq false),";
                url = url + "stock_location_products,";
                url = url + "unit";
                return url;
            } }

        
        public bool is_save_and_new { get; set; }



       public void OnCancel()
        {
            if (id == 0)
            {
                nav.NavigateTo("product");
            }else
            {
                nav.NavigateTo("product/" + id);
            }
        }



        protected override async Task OnInitializedAsync()
        {

            is_loading = true;
            if (id > 0)
            {

                await LoadData();
            }
            else if (clone_id > 0)
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

                    unit_category_id = model.unit.unit_category_id;
                    old_unit_category_id = model.unit.unit_category_id;

                    if (model.is_deleted)
                    {
                        error_text = lang["This product was deleted so cannot edit it."];
                        is_error = true;
                        is_loading = false;
                        
                        return;
                    }
                    else{
                        is_error = true;
                    }

                }
            }
            
            is_loading = false;

        }
        public void onUnitCategoryChange(int val)
        {
            if (old_unit_category_id != val) 
            {
                var _data = gv.units.Where(r => r.unit_category_id == val && !r.is_deleted && r.status).ToList();
               
                model.unit_id = _data.Count > 0? _data.FirstOrDefault().id :0;
                 
            } 
            old_unit_category_id = val; 
            unit_category_id = val; 
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
            Console.WriteLine(JsonSerializer.Serialize(save_model.unit));

            if (save_model.unit_id == 0)
            { 
                toast.Add(lang["Please Select Unit."], MatToastType.Warning);
                is_saving = false;
                return;
            }

            if (save_model.product_portions.Where(r => r.is_deleted == false).SelectMany(r => r.product_prices).Where(r => r.is_deleted == false && r.price > 0).Count() > 0)
            {
                save_model.min_price = save_model.product_portions.Where(r => r.is_deleted == false).SelectMany(r => r.product_prices).Where(r => r.is_deleted == false && r.price > 0).Min(r => r.price);
                save_model.max_price = save_model.product_portions.Where(r => r.is_deleted == false).SelectMany(r => r.product_prices).Where(r => r.is_deleted == false && r.price > 0).Max(r => r.price);
            }
            //remove menu
            save_model.product_menus.ForEach(r => r.menu = null);
            save_model.is_menu_product = true;
            save_model.vendor = null;
            save_model.vendor_id = save_model.vendor_id == 0 ? null : save_model.vendor_id;

            var resp = await http.ApiPost($"Product/Save", save_model);

            if (resp.IsSuccess)
            {
                toast.Add(lang["Save product successfully"], MatToastType.Success);
                if (is_save_and_new)
                {
                    model = new ProductModel();
                    model.product_category_id = save_model.product_category_id;
                    is_save_and_new = false;
                    save_model = JsonSerializer.Deserialize<ProductModel>(resp.Content.ToString());
                    nav.NavigateTo("product/new");

                }else
                {
                    save_model = JsonSerializer.Deserialize<ProductModel>(resp.Content.ToString());
                    nav.NavigateTo($"product/{save_model.id}"); 
                }
            }
            else
            {
                toast.Add(lang[resp.Content.ToString()], MatToastType.Warning);
            }
            is_saving = false;
        }
         
    }
}
