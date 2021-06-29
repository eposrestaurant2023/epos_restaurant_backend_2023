using eModels;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace eAdmin.Pages.PageCustomers
{
    public class PageAddCustomers:PageCore
    {
        [Parameter] public string id { get; set; }
        public CustomerModel model { get; set; } = new CustomerModel();
        

        private DateTime? _date = DateTime.Now.AddYears(-18);

        public DateTime? BOD
        {
            get { return _date; }
            set { 
                _date = value;
                model.date_of_birth = Convert.ToDateTime(value);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            var res = await http.ApiGet($"customer({id})");
            if (res.IsSuccess)
            {
                model = JsonSerializer.Deserialize<CustomerModel>(res.Content.ToString());
            }
            is_loading = false;
        }

        public async Task Save_Click()
        {
            model.is_saving = true;
            var res = await http.ApiPost("customer/save", model);
            Console.WriteLine(JsonSerializer.Serialize(model));
            if (res.IsSuccess)
            {
                toast.Add("Save Successfull.", Severity.Success);
                nav.NavigateTo("customer");
            }
            model.is_saving = false;
        }

        
    }
}
