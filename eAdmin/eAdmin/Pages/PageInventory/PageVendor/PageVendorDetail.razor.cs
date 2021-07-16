using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace eAdmin.Pages.PageVendors
{
    public class PageVendorDetail : PageCore
    {
        [Parameter] public int id { get; set; }
        public VendorModel model { get; set; }
        public bool ShowModal = false;
        public string ModalTitle = "";
        public bool is_comment_loaded, is_show_check_in, show_member_ship_tab;
        public bool purchase_history_tab_click, purchase_product_history_tab_click, payment_history_tab_click, purchase_payment_history_tab_click;
        string controller_api = "Vendor";
        List<HistoryModel> histories = new List<HistoryModel>();

        public string api_query
        {
            get
            {
                string query = $"{controller_api}({id})";
                query += $"?$expand=vendor_group";   
                return query;
            }
        }

      
        protected override async Task OnParametersSetAsync()
        {
            is_loading = true;
            await LoadData();
        }

        public async Task LoadData()
        {
            is_loading = true;
            var resp = await http.ApiGet(api_query);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<VendorModel>(resp.Content.ToString());
            }
            else
            {
                is_error = true;
                error_text = "Page not found.";
            }

            is_loading = false;
        }

        public void OnEdit(int id)
        {
            is_loading_data = true;
            nav.NavigateTo($"vendor/edit/{id}");
            is_loading_data = false;
        }
        public async Task OnDelete(VendorModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("vendor Customer", "Are you sure you want to delete this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add("Delete vendor successfully", MatBlazor.MatToastType.Success);
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task OnRestore(VendorModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("Restore Vendor", "Are you sure you want to restore this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);

                if (resp.IsSuccess)
                {
                    await LoadData();
                }
                toast.Add("Restore Vendor successfully", MatBlazor.MatToastType.Success);
            }
            p.is_loading = false;
        }
        public void SaveComment(HistoryModel h)
        {
            histories.Add(h);
        }

        public async Task OnToogleStatus(VendorModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }

        public async Task SaveStatus(VendorModel p)
        {
            var vendor = new VendorModel();
            vendor = p;
            vendor.status = !vendor.status;
            string d = JsonSerializer.Serialize(vendor);
            var resp = await http.ApiPost(controller_api + "/save", vendor);
            if (resp.IsSuccess)
            {
                toast.Add("Change status successfully", MatToastType.Success);
                await LoadData();
            }
        }
         
         
      

    }
}
