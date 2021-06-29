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
    public class PageCahsierShiftDetails : PageCore
    {
        [Parameter] public string id { get; set; } 
        public CashierShiftModel model { get; set; }
        public bool ShowModal = false;
        public string ModalTitle = "";
        string controller_api = "CashierShift";
        public bool is_show_check_in,show_member_ship_tab;
        public bool receipt_tab_click, sale_product_history_tab_click, payment_history_tab_click;
        List<HistoryModel> histories = new List<HistoryModel>();

        public string api_query
        {
            get
            {
                string query = $"{controller_api}({id})?$expand=opened_station($select=id,station_name_en,station_name_kh),closed_station($select=id,station_name_en,station_name_kh),outlet($select=outlet_name_en,outlet_name_kh)";  
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
                model = JsonSerializer.Deserialize<CashierShiftModel>(resp.Content.ToString());
            }
            else
            {
                is_error = true;
            }

            is_loading = false;
        }

        public void OnPrint()
        {
            //
        }
    }
}
