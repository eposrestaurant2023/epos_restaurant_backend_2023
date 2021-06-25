using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using eModels;

namespace eAdmin.Pages
{
    public class Dashboard:PageCore
    {
        

        protected override async Task OnInitializedAsync()
        {
            
            is_loading_data = true;
            await Task.Delay(100);
            is_loading_data = false;
        }
    }
  




}
