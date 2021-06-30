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
        public string api_url
        {
            get
            {
                return $"customer({id})?$expand=contacts($filter=is_deleted eq false)";
            }
        }
        

        protected override async Task OnInitializedAsync()
        {
            await LoadCustomer();
        }
        async Task LoadCustomer()
        {
            if (!is_loading)
            {
                is_loading = true;

                if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(clone_id))
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        var res = await http.ApiGet(api_url);
                        if (res.IsSuccess)
                        {
                            model = JsonSerializer.Deserialize<CustomerModel>(res.Content.ToString());
                            title = $"Edit : {model.customer_name_en}";
                        }
                        else
                        {
                            toast.Add(res.Content.ToString(),Severity.Warning);
                        }
                    }
                    else
                    {
                        
                        var res = await http.ApiPost($"customer/clone/{clone_id}");
                        if (res.IsSuccess)
                        {
                            model = JsonSerializer.Deserialize<CustomerModel>(res.Content.ToString());
                            Console.WriteLine(res.Content.ToString());
                            title = $"Clone : {model.customer_name_en}";
                        }
                        else
                        {
                            toast.Add(res.Content.ToString(), Severity.Warning);
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
        }
        public async Task Save_Click(bool allow_duplicate_name = false)
        {
            model.is_saving = true;
            var res = await http.ApiPost($"customer/save?allow_duplicate_name={allow_duplicate_name}", model);
            if (res.IsSuccess)
            {
                toast.Add("Save Successfull.", Severity.Success);
                var c = JsonSerializer.Deserialize<CustomerModel>(res.Content.ToString());
                nav.NavigateTo($"customer/{c.id}");
            }
            else
            {
                if (res.status_code >= 400 && res.status_code <= 499)
                {
                    ApiResponseModel r = JsonSerializer.Deserialize<ApiResponseModel>(res.Content.ToString());
                    toast.Add(r.message, Severity.Warning);
                }
                //else
                //{
                //    toast.Add(res.Content.ToString(), Severity.Warning);
                //}

            }
            model.is_saving = false;
        }

        
    }
}
