using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;
using eAdmin.JSHelpers;
using MatBlazor;
using System;
using System.Collections.Generic;

namespace eAdmin.Pages.PageInventory.PageInventoryChecks
{
    public class PageInventoryCheckDetailBase:PageCore
    {
        [Parameter] public Guid id { get; set; }

        public bool is_open_print, is_show_comment;
        public HistoryModel history { get; set; } = new HistoryModel();
        public int counted_items;

        public string api_url
        {
            get
            {
                string url = $"InventoryCheck({id})?";
                url = url + "$expand=";
                url = url + "business_branch,";
                url = url + "stock_location";
                return url;
            }
        }

        public InventoryCheckModel model { get; set; } = new InventoryCheckModel();


        protected override async Task OnInitializedAsync()
        {
            if (!is_error)
            {
                await LoadData();
            }

            if ((model == null || model.id == Guid.Empty) || is_error)
            {
                is_error = true;

                error_text = "This inventory check does not exist";
            }
            else
            {
                history.inventory_check_id = id;
            }
        }

        public async Task LoadData()
        {
            is_loading = true;
            var resp = await http.ApiGet(api_url);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<InventoryCheckModel>(resp.Content.ToString());
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
            if (counted_items==0)
            {
                toast.Add(lang["No Items Counted."], MudBlazor.Severity.Warning);
                return;
            }
            is_loading_data = true;
            if (await js.Confirm(lang["Make As Fulfilled"], lang["Are you sure you want to make as fulfilled?"]))
            {
                var resp = await http.ApiPost("InventoryCheck/MarkAsFulfilled/" + model.id);
                if (resp.IsSuccess)
                {
                    await LoadData();
                    toast.Add(lang["Mark as fulfilled successfully"], MudBlazor.Severity.Success);
                }
            }

            is_loading_data = false;
        }
        public void OnCheckProductCounted(List<InventoryCheckProductModel> list_items)
        {
            counted_items = list_items.Count;
        }

    }

    

}

 