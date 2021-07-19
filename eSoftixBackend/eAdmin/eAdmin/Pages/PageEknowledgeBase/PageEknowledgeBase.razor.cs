 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using eAdmin;
using eAdmin.Shared;
using MudBlazor;
using eModels;
using eAdmin.Shared.Users;
using eAdmin.Shared.Components;
using eAdmin.Shared.ComLayout;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;


namespace eAdmin.Pages.PageEknowledgeBase
{
    public class PageEknowledgeBases :PageCore
    {
        [Parameter] public string id { get; set; }
        public List<eKnowledgeBaseModel> eknowledgebase = new List<eKnowledgeBaseModel>();
        public eKnowledgeBaseModel model = new eKnowledgeBaseModel();
        public string StateKey = "EKNOJ84567Gs25245KJHGytkjhTonB3PCz2Ts"; //Storage and Session Key
        string controller_api = "eKnowledgeBase";
        public int TotalRecord = 0;
        public string TextValue { get; set; }
         public  MudListItem selectedItem;

        public string ControllerApi
        {
            get
            {
                if (string.IsNullOrEmpty(state.pager.order_by))
                {
                    state.pager.order_by = "id";
                    state.pager.order_by_type = "desc";
                }
                string url = $"{controller_api}";
                return url;
            }
        }



        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            state = await GetState(StateKey);
            if (state.page_title == "")
            {
                state.page_title = "eKnowledgeBase";
                var default_view = gv.GetDefaultModuleView("page_eknowledgebase");
                if (default_view != null)
                {
                    state.page_title = default_view.title;
                    state.filters = default_view.filters;
                }
            }
            if (state.filters.Count == 0)
            {
                state.filters.Add(new FilterModel()
                {
                    key = "is_deleted",
                    value1 = "false"
                });
            }
            await LoadData();
            is_loading = false;
        }

        public async Task OnRefresh()
        {
            is_loading = true;
            await LoadData();
            is_loading = false;
        }

        public async Task LoadData(string api_url = "")
        {
            is_loading = true;
            if (string.IsNullOrEmpty(api_url))
            {
                api_url = $"{ControllerApi}";
                state.api_url = api_url;
                await SetState(StateKey, state);
            }

            var resp = await http.ApiGetOData(api_url);
            if (resp.IsSuccess)
            {
                eknowledgebase = JsonSerializer.Deserialize<List<eKnowledgeBaseModel>>(resp.Content.ToString());
                TotalRecord = resp.Count;
            }
            is_loading = false;
        }

       

    }
}
