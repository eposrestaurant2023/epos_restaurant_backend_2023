using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;
using eAdmin.JSHelpers;
using MatBlazor;
using System;

namespace eAdmin.Pages.PageReceipt.ReceiptDetails
{
    public class ComSaleDetailBase : PageCore
    {
 
        [Parameter] public Guid id { get; set; }
        public int payment_id { get; set; }
        public bool is_open_print, is_show_comment;
        public HistoryModel history { get; set; } = new HistoryModel();

        public string api_url { get {
                string url = $"sale({id})?";
                url = url + "$expand=customer,";
                url = url + "sale_products($expand=product,product_type,product_variant),";
                url = url + "payments($expand=payment_type),";
                url = url + "outlet,";
                url = url + "stock_location";
                return url;
            }
        }

        public SaleModel sale { get; set; } = new SaleModel();


        protected override async Task OnInitializedAsync()
        {
            is_loading = true;

            await IsSaleOutlet();

            if (!is_error) {

                await LoadData();

            }
            
            if ((sale == null || sale.id == gv.empty_guid) || is_error )
            {
                is_error = true;

                error_text = "This sale order does not exist";
            }
            else
            {
                //history.sale_id = id;

                //if(sale.customer_id>0)
                //{

                //    history.customer_id = sale.customer_id;

                //}
            }
            

            is_loading = false;
        }

        async Task IsSaleOutlet() {

            var resp = await http.ApiGetOData($"sale?$select=id&$filter=id eq {id} and outlet_id eq {gv.current_outlet_id}&$count=true");
 
            if (resp.Count > 0)
            {
                is_error = false;
            }
            else {
                is_error = true;
            }
            
        }

        public async Task LoadData()
        {
            
            var resp =await http.ApiGet(api_url);
            if (resp.IsSuccess)
            {
                sale = JsonSerializer.Deserialize<SaleModel>(resp.Content.ToString());
            }else
            {
                is_loading_data = false;
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
                is_loading_data = true;
                await LoadData();
                is_loading_data = false;
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
 

    }

}