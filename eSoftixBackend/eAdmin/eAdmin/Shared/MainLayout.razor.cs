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
using Blazored.LocalStorage;
using eModels;
using Microsoft.AspNetCore.Components.Authorization;
using eAdmin.Services;

namespace eAdmin.Shared
{
    public partial class MainLayout
    {
 
        [CascadingParameter]
        public AppState state { get; set; }
        [Inject] protected ILocalStorageService localStorage { get; set; }
        [Inject] public IHttpService http{ get; set; }

        public bool _drawerOpen = true;

        
        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        async Task ChangeTheme()
        {
            state.current_theme = (state.current_theme == state.defaultTheme) ? state.darkTheme : state.defaultTheme;
            await localStorage.SetItemAsync("theme", ((state.current_theme==state.defaultTheme)? "defaultTheme": "darkTheme"));
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(100);
        }

       
    }
}