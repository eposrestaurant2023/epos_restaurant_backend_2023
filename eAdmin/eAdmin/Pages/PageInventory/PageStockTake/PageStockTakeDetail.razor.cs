using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;
using eAdmin.JSHelpers;
using MatBlazor;
using System;

namespace eAdmin.Pages.PageInventory.PageStockTake
{
    public class PageStockTakeDetailBase : PageCore
    {

        [Parameter] public int id { get; set; }

        public bool is_open_print, is_show_comment;
        public HistoryModel history { get; set; } = new HistoryModel();

        public string api_url
        {
            get
            {
                string url = $"StockTake({id})?";
                url = url + "$expand=stock_take_products($expand=product),";  
                url = url + "business_branch,";
                url = url + "stock_location";
                return url;
            }
        }

        public StockTakeModel model { get; set; } = new StockTakeModel();


        protected override async Task OnInitializedAsync()
        { 
            if (!is_error)
            { 
                await LoadData(); 
            }

            if ((model == null || model.id == 0) || is_error)
            {
                is_error = true;

                error_text = "This stock take does not exist";
            }
            else
            {
                history.stock_take_id = id;
            } 
        }

        public async Task LoadData()
        {
            is_loading = true;
            var resp = await http.ApiGet(api_url);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<StockTakeModel>(resp.Content.ToString());
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
                var resp = await http.ApiPost("StockTake/MarkAsFulfilled/" + model.id);
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