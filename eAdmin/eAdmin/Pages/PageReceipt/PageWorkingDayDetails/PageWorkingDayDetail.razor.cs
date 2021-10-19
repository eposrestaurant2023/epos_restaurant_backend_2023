using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace eAdmin.Pages.PageReceipt.PageWorkingDayDetails
{
    public class PageWorkingDayDetails : PageCore
    {
        [Parameter] public string id { get; set; }
        string controller_api = "WorkingDay";
        public bool receipt_tab_click, is_open_print, sale_product_history_tab_click, payment_history_tab_click;
        public WorkingDayModel model = new WorkingDayModel();
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
            await GetTistSummary();
        }

        public async Task LoadData()
        {
            is_loading = true;
            var resp = await http.ApiGet(api_query);
            if (resp.IsSuccess)
            {
                model = JsonSerializer.Deserialize<WorkingDayModel>(resp.Content.ToString());
            }
            else
            {
                is_error = true;
            }

            is_loading = false;
        }
        public async Task GetTistSummary()
        {
            var resp = await http.ApiPost("GetData", new FilterModel { procedure_name = "sp_get_end_of_day_summary", procedure_parameter = $"'{id}'"});
            if (resp.IsSuccess)
            {
                list_summaries = JsonSerializer.Deserialize<List<ListSummaryModel>>(resp.Content.ToString());
            }
        }
        public void OnPrint()
        {
            is_open_print = true;
        }

        
    }
}
