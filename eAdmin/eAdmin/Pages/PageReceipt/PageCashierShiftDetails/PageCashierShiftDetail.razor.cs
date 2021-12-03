using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;
using eAdmin.JSHelpers;
using MatBlazor;
using System;
using System.Security.Cryptography;
using MudBlazor;
using System.Linq;
using System.Collections.Generic;

namespace eAdmin.Pages.PageReceipt.PageCashierShiftDetails
{
    public class PageCahsierShiftDetails : PageCore
    {
        [Parameter] public string id { get; set; }
        string controller_api = "CashierShift";
        public bool is_open_print, receipt_tab_click, sale_product_history_tab_click, payment_history_tab_click;
        public CashierShiftModel model = new CashierShiftModel();
        public List<ListSummaryModel> list_summaries = new List<ListSummaryModel>();

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
            await GetCloseCashierShiftSummary();
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
        public async Task GetCloseCashierShiftSummary()
        {
            var resp = await http.ApiPost("GetData", new FilterModel { procedure_name = "sp_get_close_cashier_shift_summary", procedure_parameter = $"'{id}','json'"});
            if (resp.IsSuccess)
            {
                list_summaries = JsonSerializer.Deserialize<List<ListSummaryModel>>(resp.Content.ToString());
            }
        }
        public void OnPrint()
        {
            var parameters = new DialogParameters { ["parent_id"] = 243, ["report_parameters"] = $"id={ id}", ["gv"] = gv };
            Dialog.Show<eAdmin.Shared.Components.ComPreviewReport>(lang["Cashier Shift"], parameters, new DialogOptions() { FullScreen = true, CloseButton = true });
        }

        
    }
}
