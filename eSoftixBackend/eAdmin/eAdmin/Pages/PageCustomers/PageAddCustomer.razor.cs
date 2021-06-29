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
        [Parameter] public string clone_id { get; set; }
        public CustomerModel model { get; set; }
        public string page_title { get; set; }

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
            
                await LoadCustomer();

            
            
                
            
            
        }
        async Task LoadCustomer()
        {
            is_loading = true;
            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(clone_id))
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var res = await http.ApiGet($"customer({id})");
                    if (res.IsSuccess)
                    {
                        model = JsonSerializer.Deserialize<CustomerModel>(res.Content.ToString());
                        if (model != null)
                        {
                            page_title = $"Edit : {model.customer_code} - {model.customer_name_en}";
                        }
                        else
                        {
                            model = new CustomerModel();
                        }

                    }
                }
                else
                {
                    var res = await http.ApiGet($"customer({clone_id})");
                    if (res.IsSuccess)
                    {
                        model = JsonSerializer.Deserialize<CustomerModel>(res.Content.ToString());
                        if (model != null)
                        {
                            page_title = $"Clone : {model.customer_code} - {model.customer_name_en}";
                        }
                        else
                        {
                            model = new CustomerModel();
                        }

                    }
                }

            }
            else
            {
                model = new CustomerModel();
                page_title = "New Customer";
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
