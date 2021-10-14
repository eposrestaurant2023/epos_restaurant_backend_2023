using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;
using eAdmin.JSHelpers;
using MatBlazor;
using System;

namespace eAdmin.Pages.PageInventory.PageStockTake
{
    public class PageStockTransferDetailBase : PageCore
    {

        [Parameter] public int id { get; set; }

        public bool is_open_print, is_show_comment;
        public HistoryModel history { get; set; } = new HistoryModel();

        public string api_url
        {
            get
            {
                string url = $"StockTransfer({id})?";
                url = url + "$expand=stock_transfer_products($expand=product),";  
                url = url + "to_business_branch,";
                url = url + "to_stock_location,";
                url = url + "from_business_branch,";
                url = url + "from_stock_location";
                return url;
            }
        }

        public StockTransferModel model { get; set; } = new StockTransferModel();

        protected override async Task OnInitializedAsync()
        { 
            if (!is_error)
            { 
                await LoadData(); 
            }

            if ((model == null || model.id == 0) || is_error)
            {
                is_error = true;

                error_text = "This stock transfer does not exist";
            }
            else
            {
                history.stock_transfer_id = id;
            } 
        }

        public async Task LoadData()
        {
            is_loading = true;
            var resp = await http.ApiGet(api_url);
            Console.WriteLine(api_url);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<StockTransferModel>(resp.Content.ToString());
            }
            else
            {
                is_loading_data = false;
            }
            is_loading = false;
        }
        public void ShowComment()
        {
            if (!is_show_comment)
            {
                is_show_comment = true;
            }
        } 
 

        public async Task OnRefresh()
        {
            is_loading_data = true;
            await LoadData();
            is_loading_data = false;
        }


        public async Task MarkAsFulfilled()
        {
            is_loading_data = true;
            if (await js.Confirm("Make As Fulfilled", "Are you sure you want to make as fulfilled?"))
            {
                var resp = await http.ApiPost("StockTransfer/MarkAsFulfilled/" + model.id);
                if (resp.IsSuccess)
                {
                    await LoadData();
                    toast.Add("Mark as fulfilled successfully", MatToastType.Success);
                }
            }

            is_loading_data = false;
        }

    }

}