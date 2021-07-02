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
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            tax_rule = JsonSerializer.Deserialize<eShareModel.TaxRuleModel>(gv.tax_rule);
            is_loading = false;
        }

    }
}
