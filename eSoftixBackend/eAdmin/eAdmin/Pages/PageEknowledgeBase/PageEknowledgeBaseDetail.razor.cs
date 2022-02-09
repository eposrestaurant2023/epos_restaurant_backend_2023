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
    public class PageEknowledgeBaseDetailBase : PageCore
    {
        [Parameter] public string id { get; set; }
        public List<eKnowledgeBaseModel> eknowledgebase = new List<eKnowledgeBaseModel>();
        public eKnowledgeBaseModel model = new eKnowledgeBaseModel();
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            is_loading_data = true;
            await LoadData();
            is_loading_data = false;
            is_loading = false;
        }

        public async Task LoadData()
        {
            var res = await http.ApiGet($"eKnowledgeBase({id})?$expand=children");
            if (res.IsSuccess)
            {
                model = JsonSerializer.Deserialize<eKnowledgeBaseModel>(res.Content.ToString());
            }
        }

        public async Task OnRefresh()
        {
            is_loading = true;
            await LoadData();
            is_loading = false;
        }

       


    }
}
