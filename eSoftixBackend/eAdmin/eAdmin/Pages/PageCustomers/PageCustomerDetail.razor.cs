using eModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageCustomers
{
    public class PageCustomerDetails : PageCore
    {
        [Parameter] public string id { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        public CustomerModel model { get; set; }
        public bool ShowModal = false;
        public string ModalTitle = "";
        public bool is_open_customer_mobile = false;
        string controller_api = "customer";
        public bool is_comment_loaded, is_show_check_in, show_member_ship_tab;
        public bool sale_history_tab_click, sale_product_history_tab_click, payment_history_tab_click;
        List<HistoryModel> histories = new List<HistoryModel>();


        public string api_query
        {
            get
            {
                string query = $"{controller_api}({id})";
                query += $"?$expand=customer_group,contacts";
                return query;
            }
        }


        protected override async Task OnParametersSetAsync()
        {
            is_loading = true;
            await LoadData();
            is_loading = false;
        }

     public   async Task OnRefresh()
        {
            is_loading = true;
            await LoadData();
            is_loading = false;
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
                error_text = "Customer not found!";
            }

            is_loading = false;
        }

        public void OnEdit(int id)
        {
            is_loading_data = true;
            nav.NavigateTo($"customer/edit/{id}");
            is_loading_data = false;
        }

        public void SaveComment(HistoryModel h)
        {
            histories.Add(h);
        }

        public void Navigation_Click(string url)
        {
            nav.NavigateTo(url);
        }
        public async Task ChangeStatus_Click(Guid id)
        {
            is_loading = true;
            var res = await http.ApiPost($"customer/status/{id}");
            if (res.IsSuccess)
            {
                if (model.status)
                {
                    toast.Add("Customer is inactived.", Severity.Success);
                }
                else
                {
                    toast.Add("Customer is inactived.", Severity.Success);
                }
                await LoadData();
            }
            is_loading = false;
        }
        public async Task DeleteCustomer_Click()
        {
            
            string state = "Are You sure your want to delete?";
            bool? result = await DialogService.ShowMessageBox(
            "Delete",
            state,
            yesText: "Ok", cancelText: "Cancel");
            StateHasChanged();
            if ((bool)result)
            {
                is_loading = true;
                var res = await http.ApiPost($"customer/delete/{id}");
                if (res.IsSuccess)
                {
                    toast.Add("Delete successfuly.", Severity.Success);
                    await LoadData();
                }
                
            }
            
            is_loading = false;
            
           
        }



       public void ToggleDrawer_modile_custoemr_detail()
        {
            is_open_customer_mobile = !is_open_customer_mobile;

        }


    }

}

