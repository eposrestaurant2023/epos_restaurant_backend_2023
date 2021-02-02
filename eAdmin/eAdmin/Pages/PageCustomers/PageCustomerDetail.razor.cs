using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace eAdmin.Pages.PageCustomers
{
    public class PageCustomerDetails : PageCore
    {
        [Parameter] public string id { get; set; }
        public CustomerModel model { get; set; }
        public bool ShowModal = false;
        public string ModalTitle = "";
        string controller_api = "customer";
        public bool is_comment_loaded,is_show_check_in, show_member_ship_tab;
        public bool sale_history_tab_click, sale_product_history_tab_click, payment_history_tab_click;
        List<HistoryModel> histories = new List<HistoryModel>();

        public string api_query
        {
            get
            {
                string query = $"{controller_api}({id})";
                query += $"?$expand=customer_group";   
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
                model = JsonSerializer.Deserialize<CustomerModel>(resp.Content.ToString());
            }
            else
            {
                is_error = true;
            }

            is_loading = false;
        }

        public void OnEdit(int id)
        {
            is_loading_data = true;
            nav.NavigateTo($"customer/edit/{id}");
            is_loading_data = false;
        }
        public async Task OnDelete(CustomerModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("Delete Customer", "Are you sure you want to delete this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    toast.Add("Delete Customer successfully", MatBlazor.MatToastType.Success);
                    await LoadData();
                }
            }
            p.is_loading = false;
        }

        public async Task OnRestore(CustomerModel p)
        {
            p.is_loading = true;
            if (await js.Confirm("Restore Customer", "Are you sure you want to restore this record?"))
            {
                var resp = await http.ApiPost(controller_api + "/delete/" + p.id);

                if (resp.IsSuccess)
                {
                    await LoadData();
                }
                toast.Add("Restore Customer successfully", MatBlazor.MatToastType.Success);
            }
            p.is_loading = false;
        }
        public void SaveComment(HistoryModel h)
        {
            histories.Add(h);
        }

        public async Task OnToogleStatus(CustomerModel p)
        {
            p.is_loading = true;
            await SaveStatus(p);
            p.is_loading = false;
        }

        public async Task SaveStatus(CustomerModel p)
        {
            var customer = new CustomerModel();
            customer = p;
            customer.status = !customer.status;
            string d = JsonSerializer.Serialize(customer);
            var resp = await http.ApiPost(controller_api + "/save", customer);
            if (resp.IsSuccess)
            {
                toast.Add("Change status successfully", MatToastType.Success);
                await LoadData();
            }
        }
         
         
      

    }
}
