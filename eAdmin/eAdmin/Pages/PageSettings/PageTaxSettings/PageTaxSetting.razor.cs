using eAdmin.JSHelpers;
using eModels;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace eAdmin.Pages.PageSettings.PageTaxSettings
{
    public class PageTaxSettingCore : PageCore
    {
        public eShareModel.TaxRuleModel tax_rule = new eShareModel.TaxRuleModel();
        public List<StationModel> stations = new List<StationModel>();
        public bool show_tax_rule = true;
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            tax_rule = JsonSerializer.Deserialize<eShareModel.TaxRuleModel>(gv.tax_rule); 
            await GetSations();
            is_loading = false;
        }


        public async Task GetSations()
        {
            var resp = await http.ApiGetOData("station");
            if (resp.IsSuccess)
            {
                stations = JsonSerializer.Deserialize<List<StationModel>>(resp.Content.ToString());
            }
        }


        public async Task SaveTaxRule()
        {
            is_saving = true;
            if (string.IsNullOrEmpty(tax_rule.tax_1_name) || string.IsNullOrEmpty(tax_rule.tax_2_name) || string.IsNullOrEmpty(tax_rule.tax_1_name))
            {

                toast.Add(lang["Please enter tax rule name"], MudBlazor.Severity.Warning);
                is_saving = false;
                return;
            }
            var resp = await http.ApiPost("setting/taxrule/save", tax_rule);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully"], MudBlazor.Severity.Success);
            }
            else
            {

                toast.Add(lang["Save data fail"], MudBlazor.Severity.Warning);
            }
            is_saving = false;
        }


        public async Task SaveStationTax()
        {
            is_saving = true;
            var resp = await http.ApiPost("station/save/multiple", stations);
            if (resp.IsSuccess)
            {
                toast.Add(lang["Save successfully"], MudBlazor.Severity.Success);
            }
            else
            {

                toast.Add(lang["Save data fail"], MudBlazor.Severity.Warning);
            }
            is_saving = false;
        }

    }
}
