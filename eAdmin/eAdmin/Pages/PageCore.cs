using eAdmin.Services;
using eModels;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using NETCore.Encrypt;
using System.Text.Json;
namespace eAdmin.Pages
{
    
    public class PageCore : ComponentBase
    {
        [CascadingParameter] public GlobalVariableModel gv { get; set; }

        [Inject] protected IMatToaster toast { get; set; }
        [Inject] protected IConfiguration config { get; set; }
        [Inject] protected IJSRuntime js { get; set; }
        [Inject] protected IHttpService http { get; set; }
        [Inject] protected ILocalStorageService localStorage { get; set; }
        [Inject] protected ISessionStorageService sessionStorage { get; set; }
        [Inject] protected NavigationManager nav { get; set; }

        public bool is_loading { get; set; } = false;
        public bool is_loading_data { get; set; } = false;
        public bool is_error { get; set; } = false;
        public bool is_saving { get; set; }
        public string error_text { get; set; }
        public string title { get; set; } = "";

        public StateModel state = new StateModel();
      
        public Dictionary<string, object> formAttributes { get; set; } = new Dictionary<string, object>()
        {
                { "class", "uk-form-horizontal" }
        };
        public Dictionary<string, object> searchFormAttributes { get; set; } = new Dictionary<string, object>()
        {
                { "class", "uk-search uk-search-default" }
        };

        public string GetFilterValue1(List<FilterModel> filters, string _key, string default_value = "")
        {
            var d = filters.Where(r => r.key == _key);
            if (d.Count() > 0)
            {
                return d.FirstOrDefault().value1;
            }
            return default_value;
        }
        public string GetFilterValue2(List<FilterModel> filters, string _key, string default_value = "")
        {
            var d = filters.Where(r => r.key == _key);
            if (d.Count() > 0)
            {
                return d.FirstOrDefault().value2;
            }
            return default_value;
        }
        public DateTime GetFilterDate1(List<FilterModel> filters, string _key)
        {
            var d = filters.Where(r => r.key == _key);
            if (d.Count() > 0)
            {
                return d.FirstOrDefault().date1;
            }
            return DateTime.Now;
        }
        public DateTime GetFilterDate2(List<FilterModel> filters, string _key)
        {
            var d = filters.Where(r => r.key == _key);
            if (d.Count() > 0)
            {
                return d.FirstOrDefault().date2;
            }
            return DateTime.Now;
        }


        public void SetFilterValue1(List<FilterModel> filters, string _key, string _value, bool is_clear_all = false)
        {
            var f = filters.Where(r => r.key == _key);
            if (f.Count() > 0)
            {
                f.FirstOrDefault().value1 = _value;
            }
            else
            {
                filters.Add(new FilterModel() { key = _key, value1 = _value, is_clear_all = is_clear_all });
            }

        }
        public void SetFilterValue2(List<FilterModel> filters, string _key, string _value, bool is_clear_all = false)
        {
            var f = filters.Where(r => r.key == _key);
            if (f.Count() > 0)
            {
                f.FirstOrDefault().value2 = _value;
            }
            else
            {
                filters.Add(new FilterModel() { key = _key, value2 = _value, is_clear_all = is_clear_all });
            }
        }

        public void SetFilterDate1(List<FilterModel> filters, string _key, DateTime _value)
        {
            filters.Where(r => r.key == _key).FirstOrDefault().date1 = _value;
        }
        public void SetFilterDate2(List<FilterModel> filters, string _key, DateTime _value)
        {
            filters.Where(r => r.key == _key).FirstOrDefault().date2 = _value;
        }

        public string GetFilter(List<FilterModel> filters)
        {

            string filter = "";
            var chk = filters.Where(r => r.value1 != "");
            if (chk.Count() > 0)
            {
                filter = "&$filter=";

                foreach (var a in chk)
                {
                    if (a.filter_operator != FilterOperator.notfilter.ToString())
                    {
                        if (a.filter_operator == "contains")
                        {
                            filter = $"{filter}  contains({a.key} , '{a.value1}') {a.filter_join_operator} ";
                        }
                        
                        else
                        {
                            string filter_values = "";
                            if (a.filter_operator == "multiple")
                            {
                                string[] values = a.value1.Split(',');
                                
                                foreach (var i in values)
                                {
                                    filter_values = filter_values + a.key + " eq " + i + " or ";
                                }
                                filter_values = filter_values.Substring(0, filter_values.Length - 4);
                                filter_values = $"({filter_values})    ";

                            }

                            if (!string.IsNullOrEmpty(filter_values))
                            {
                                filter = $"{filter} {filter_values} {a.filter_join_operator}";
                            }
                            else
                            {
                                filter = $"{filter} {a.key} {a.filter_operator} {a.value1} {a.filter_join_operator}";
                            }
                            
                        }
                    } 
                }
                filter = filter.Substring(0, filter.Length - 4);
            }

            return filter;
        }

        public async Task<StateModel> GetState(string _key)
        {
            StateModel state = new StateModel();
            try
            {
                string value = await sessionStorage.GetItemAsync<string>(_key);
                if (!string.IsNullOrEmpty(value))
                {
                    value = EncryptProvider.Base64Decrypt(value);
                    state = JsonSerializer.Deserialize<StateModel>(value);
                }
                state.pager = await localStorage.GetItemAsync<PagerModel>(_key);

                if (state.filters == null)
                    state.filters = new List<FilterModel>();

                if (state.api_url == null)
                    state.api_url = "";
                if (state.pager == null)
                    state.pager = new PagerModel();

                state.pager.current_page = state.pager.current_page <= 0 ? 1 : state.pager.current_page;

                return state;
            }
            catch
            {
                state.api_url = "";
                state.pager = new PagerModel();
                return state;
            }
        }

        public async Task SetState(string _key, StateModel state)
        {
            try
            {

                if (!string.IsNullOrEmpty(state.api_url))
                {
                    string value = EncryptProvider.Base64Encrypt(JsonSerializer.Serialize(state));
                    await sessionStorage.SetItemAsync(_key, value);
                }
                if (state.pager != null)
                {
                    await localStorage.SetItemAsync(_key, state.pager);
                }
            }
            catch
            {

            }
        }

        public void RemoveFilter(StateModel state, string key)
        {
            switch (key)
            {
                case "product_category":
                    state.product_category = new ProductCategoryModel();
                    break;

                case "product_group":
                    state.product_group = new ProductGroupModel();
                    break;

                case "customer_group":
                    state.customer_group = new CustomerGroupModel();
                    break;

                case "customer":
                    state.customer = new CustomerModel();
                    break;

                case "outlet":
                    state.outlet = new OutletModel();
                    break;

                case "vendor":
                    state.vendor = new VendorModel();
                    break;
                   
               
                default:
                    break;
            }
        }
        public async Task ToogleMenu()
        {
            await localStorage.SetItemAsync("showhidemenu", "0");
        }

    }

}
