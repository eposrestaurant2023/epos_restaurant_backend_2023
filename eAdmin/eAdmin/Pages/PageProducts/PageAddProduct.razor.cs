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
        public string PageTitle
        {
            get
            {

                if (id > 0)
                {
                    return "Edit Product";
                }
                else
                {
                    return "New Product";
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
                url = url + "product_portions($expand=product_prices;$filter=is_deleted eq false),";
                url = url + "product_menus($expand=menu;$filter=is_deleted eq false)";
                return url;
            } }

        public ProductModel model { get; set; } = new ProductModel();
        public bool is_save_and_new { get; set; }



       public void OnCancel()
        {
            if (id == 0)
            {
                nav.NavigateTo("product");
            }else
            {
                nav.NavigateTo("/product/" + id);
            }
        }



        protected override async Task OnInitializedAsync()
        {

            is_loading = true;
            if (id > 0)
            {
                await LoadData();
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

        public async Task Save_Click()
        {

             
            is_saving = true;

            ProductModel save_model = new ProductModel();
            save_model = JsonSerializer.Deserialize<ProductModel>(JsonSerializer.Serialize(model));
            //remove menu
            save_model.product_menus.ForEach(r => r.menu = null);

            var resp = await http.ApiPost("Product/Save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add("Save product successfully", MatToastType.Success);
                if (is_save_and_new)
                {
                    model = new ProductModel();
                    model.product_category_id = save_model.product_category_id;

                }else
                {
                    save_model = JsonSerializer.Deserialize<ProductModel>(resp.Content.ToString());
                    nav.NavigateTo($"product/{save_model.id}"); 
                }
            }
            else
            {

                toast.Add("Save product fail", MatToastType.Warning);
            }
            is_saving = false;


    }


    }
}
