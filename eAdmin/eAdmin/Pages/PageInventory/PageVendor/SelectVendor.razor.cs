
using eModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageInventory.PageVendor
{
    public class SelectvendorBase : PageCore
    {
        [Parameter] public bool is_open { get; set; }              
        [Parameter] public EventCallback<bool> is_openChanged { get; set; }      
        [Parameter] public VendorModel vendor { get; set; }
        [Parameter] public EventCallback<VendorModel> vendorChanged { get; set; }        

        public List<VendorModel> models = new List<VendorModel>();

        public PagerModel pager = new PagerModel() { per_page=10};
        public int total_record = 0;
        public string keyword = "";           

        string controller_api
        {
            get
            {
                return $"vendor?&keyword={keyword}&$count=true&$top={pager.per_page}&$skip={pager.per_page * (pager.current_page - 1)}&$filter=status eq true and is_deleted eq false";
                
            }
        }

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            await LoadData();
        }

        public async Task LoadData()
        {
            is_loading_data = true;
            var resp = await http.ApiGetOData(controller_api);
            if (resp.IsSuccess)
            {
                models = JsonSerializer.Deserialize<List<VendorModel>>(resp.Content.ToString());
                total_record = resp.Count;
            }

            is_loading_data = 
                is_loading = false;
        }

        public async Task onCloseClick()
        {
            await is_openChanged.InvokeAsync(false);
        }

        public async Task SelectChange(int perpage)
        {
            pager.per_page = perpage;
            pager.current_page = 1;
            await LoadData();
        }
        public async Task ChangePager(int _page)
        {
            pager.current_page = _page;
            await LoadData();
        }
        public async Task OnSearch(string _keyword)
        {
            pager = new PagerModel();
            keyword = _keyword;
            await LoadData();
        }
        public async Task SelectVendorClick(VendorModel c)
        {
            await vendorChanged.InvokeAsync(c);
            await is_openChanged.InvokeAsync(false);
        }
    }
}
