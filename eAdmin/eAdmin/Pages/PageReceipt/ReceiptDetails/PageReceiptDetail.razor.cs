﻿using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;
using eAdmin.JSHelpers;
using MatBlazor;
using System;
using System.Security.Cryptography;
using MudBlazor;
using System.Linq;

namespace eAdmin.Pages.PageReceipt.ReceiptDetails
{
    public class ComSaleDetailBase : PageCore
    {
 
        [Parameter] public Guid id { get; set; }
        public bool is_open_print, is_show_comment, is_show_payment_history;
        public HistoryModel history { get; set; } = new HistoryModel();

        public string api_url { get {
                string url = $"sale({id})?";
                url = url + "$expand=customer,sale_status,";
                url = url + "sale_products($expand=product,sale_product_modifiers($select=modifier_name,price)),";
                url = url + "sale_payments($expand=payment_type),";
                url = url + "business_branch";
                return url;
            }
        }

        public SaleModel sale { get; set; } = new SaleModel();


        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            await LoadData(); 
            is_loading = false;
        }
        public async Task LoadData()
        {
            is_loading_data = true;
          
            var resp =await http.ApiGet(api_url);
            if (resp.IsSuccess)
            {
                sale = JsonSerializer.Deserialize<SaleModel>(resp.Content.ToString());
                history.sale_id = id;
                if (sale.customer_id != Guid.Empty)
                { 
                    history.customer_id = sale.customer_id; 
                }
            }
            else
            { 
                toast.Add(lang["Load data fail. Please try again."], MudBlazor.Severity.Warning);

                is_loading_data = false;
            }
         
            is_loading_data = false;
        }

        public void ShowPaymentHistory()
        {
            if (!is_show_payment_history)
            {
                is_show_payment_history = true;
            }
        }
        public void ShowComment()
        {
            if (!is_show_comment)
            {
                is_show_comment = true;
            }
        }
 
 

        public async Task SavePayment_Click(bool is_success)
        {
            if (is_success)
            { 
                await LoadData(); 
            } 
            sale.is_loading = false;
            is_loading_data = false;
        }

 
        public async Task OnRefresh()
        {
            is_loading_data = true;
            await LoadData();
            is_loading_data = false;
        }

        public void PrintReceipt()
        {

            var parameters = new DialogParameters { ["parent_id"] = 190, ["report_parameters"] = $"id={ id}", ["gv"] = gv};
             Dialog.Show<eAdmin.Shared.Components.ComPreviewReport>(lang["Sale Receipt"], parameters, new DialogOptions() { FullScreen = true, CloseButton = true });
        }
    }

}