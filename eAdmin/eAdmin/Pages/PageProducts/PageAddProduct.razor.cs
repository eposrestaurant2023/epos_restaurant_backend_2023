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
    public class PageAddProductBase : PageCore
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
                url = url + "product_menus($expand=menu;$filter=is_deleted eq false),";
                url = url + "product_modifiers($expand=modifier;$filter=is_deleted eq false)";
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
                nav.NavigateTo("/product/" + id);
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
            if (save_model.product_portions.Where(r => r.is_deleted == false).SelectMany(r => r.product_prices).Where(r => r.is_deleted == false && r.price > 0).Count() > 0)
            {
                save_model.min_price = save_model.product_portions.Where(r => r.is_deleted == false).SelectMany(r => r.product_prices).Where(r => r.is_deleted == false && r.price > 0).Min(r => r.price);
                save_model.max_price = save_model.product_portions.Where(r => r.is_deleted == false).SelectMany(r => r.product_prices).Where(r => r.is_deleted == false && r.price > 0).Max(r => r.price);
            }
            //remove menu
            save_model.product_menus.ForEach(r => r.menu = null);
            save_model.is_menu_product = true;

            var resp = await http.ApiPost("Product/Save", save_model);
            if (resp.IsSuccess)
            {
                toast.Add("Save product successfully", MatToastType.Success);
                if (is_save_and_new)
                {
                    model = new ProductModel();
                    model.product_category_id = save_model.product_category_id;

                    nav.NavigateTo("product/new");

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
