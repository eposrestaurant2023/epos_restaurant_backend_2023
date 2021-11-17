using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;
using eAdmin.JSHelpers;
using MatBlazor;
using System;

namespace eAdmin.Pages.PageInventory.PagePurchaseOrder
{
    public class PagePurchaseOrderDetailBase : PageCore
    {

        [Parameter] public int id { get; set; }
        public int payment_id { get; set; }
        public bool is_open_print, is_add_payment, is_show_comment, is_show_payment_history;
        public HistoryModel history { get; set; } = new HistoryModel();

        public string api_url
        {
            get
            {
                string url = $"PurchaseOrder({id})?";
                url = url + "$expand=vendor,";
                url = url + "purchase_order_products($expand=product),";
                url = url + "purchase_order_payments($expand=payment_type),";
                url = url + "business_branch,";
                url = url + "stock_location";
                return url;
            }
        }

        public PurchaseOrderModel model { get; set; } = new PurchaseOrderModel();


        protected override async Task OnInitializedAsync()
        { 
            if (!is_error)
            {
                is_loading = true;
                await LoadData();
                is_loading = false;

            }

            if ((model == null || model.id == 0) || is_error)
            {
                is_error = true;

                error_text = "This PO does not exist";
            }
            else
            {
                history.purchase_order_id = id;

                if (model.vendor_id > 0)
                {

                    history.vendor_id = model.vendor_id;

                }
            } 
        }

        public async Task LoadData()
        {
            is_loading_data = true;
            var resp = await http.ApiGet(api_url);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<PurchaseOrderModel>(resp.Content.ToString());
            }
            else
            {
                is_loading_data = false;
            }
            is_loading_data = false;
        }
        public void ShowComment()
        {
            if (!is_show_comment)
            {
                is_show_comment = true;
            }
        }
        public void ShowPaymentHistory()
        {
            if (!is_show_payment_history)
            {
                is_show_payment_history = true;
            }
        }
 
        public async Task SavePayment_Click(bool is_success)
        {
            if (is_success)
            {
             
                await LoadData();
              
            }
            is_add_payment = false;
            model.is_loading = false;
          
        }

        public void EditPayment_Click(PurchaseOrderPaymentModel p)
        {
            payment_id = p.id;
            is_add_payment = true;
        }


        public async Task DeletePayment_Click(PurchaseOrderPaymentModel p)
        {
            is_loading_data = true;
            if (await js.Confirm(lang["Delete Record"], lang["Are you sure you want to delete this record?"], SweetAlertMessageType.question))
            {
                var resp = await http.ApiPost("payment/sale/delete/" + p.id);
                if (resp.IsSuccess)
                {
                    await LoadData();
                    toast.Add(lang["Delete record successfully"], MatBlazor.MatToastType.Success);
                }
                else
                {
                    toast.Add(lang[resp.Content.ToString()], MatBlazor.MatToastType.Warning);
                }
            }
            is_loading_data = false;
        }

        public async Task OnRefresh()
        {
            is_loading_data = true;
            await LoadData();
            is_loading_data = false;
        }   
        public async Task OnSavePayment(bool is_saving)
        {
            if(is_saving)
            {
                await LoadData();

            }

        }


        public async Task MarkAsFulfilled()
        {
            is_loading_data = true;
            if (await js.Confirm("Make As Fulfilled", lang["Are you sure you want to make as fulfilled?"]))
            {
                var resp = await http.ApiPost("PurchaseOrder/MarkAsFulfilled/" + model.id);
                if (resp.IsSuccess)
                {
                    await LoadData();
                    toast.Add(lang["Mark as fulfilled successfully"], MatToastType.Success);
                }
            }

            is_loading_data = false;
        }

    }

}