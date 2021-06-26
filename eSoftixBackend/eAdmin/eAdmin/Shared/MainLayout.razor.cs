using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
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