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


        public async Task Save_Click()
        {

             
            is_saving = true;

            ProductModel save_model = new ProductModel();
            save_model = JsonSerializer.Deserialize<ProductModel>(JsonSerializer.Serialize(model));


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
