using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System;
using eAdmin.Services;
using eModels;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using MudBlazor;

namespace eAdmin
{
    public partial class AppState
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Inject] protected ILocalStorageService localStorage { get; set; }
        
        [CascadingParameter] Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject] protected IHttpService http { get; set; }
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject] protected NavigationManager nav { get; set; }


        private bool _is_loading;

        public bool is_loading
        {
            get { return _is_loading; }
            set { _is_loading = value; StateHasChanged(); }
        }


        private bool _is_logined = true;
        public bool is_logined
        {
            get
            {
                return _is_logined;
            }

            set
            {
                _is_logined = value;
                StateHasChanged();
            }
        }

        private MudTheme _current_theme;
        public MudTheme current_theme
        {
            get
            {
                if (_current_theme == null)
                {
                    _current_theme = defaultTheme;
                }

                return _current_theme;
            }

            set
            {
                _current_theme = value;
                StateHasChanged();
            }
        }

        private UserModel _current_login_user;

        public UserModel current_login_user
        {
            get { return _current_login_user; }
            set { _current_login_user = value; StateHasChanged(); }
        }

        private GlobalVariableModel _gv;

        public GlobalVariableModel gv
        {
            get { return _gv; }
            set { _gv = value;StateHasChanged(); }
        }


        public MudTheme defaultTheme = new MudTheme()
        {Palette = new Palette()
        {Primary = Colors.Blue.Default, Secondary = Colors.Green.Accent4, AppbarBackground = Colors.Red.Default, }, };
        public MudTheme darkTheme = new MudTheme()
        {Palette = new Palette()
        {Black = "#27272f", Background = "#32333d", BackgroundGrey = "#27272f", Surface = "#373740", DrawerBackground = "#27272f", DrawerText = "rgba(255,255,255, 0.50)", DrawerIcon = "rgba(255,255,255, 0.50)", AppbarBackground = "#27272f", AppbarText = "rgba(255,255,255, 0.70)", TextPrimary = "rgba(255,255,255, 0.70)", TextSecondary = "rgba(255,255,255, 0.50)", ActionDefault = "#adadb1", ActionDisabled = "rgba(255,255,255, 0.26)", ActionDisabledBackground = "rgba(255,255,255, 0.12)", Divider = "rgba(255,255,255, 0.12)", DividerLight = "rgba(255,255,255, 0.06)", TableLines = "rgba(255,255,255, 0.12)", LinesDefault = "rgba(255,255,255, 0.12)", LinesInputs = "rgba(255,255,255, 0.3)", TextDisabled = "rgba(255,255,255, 0.2)"}};

        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            
            var theme = await localStorage.GetItemAsync<string>("theme");
            if(theme == null || theme.ToString() == "defaultTheme")
            {
                current_theme = defaultTheme;
            }else
            {
                current_theme = darkTheme;
            }

            //check login
          await CheckUserLogin();

            await GetGlobalVariable();
            is_loading = false;
        }

        public async Task GetGlobalVariable()
        {
            string api_url = "GlobalVariable?$expand=";
            api_url = api_url + "permission_options($select=id,parent_id,option_name,roles,url,report_title,report_title_kh,note,icon,show_in_menu,sort_order,is_section_header,is_match_all,show_in_sub_menu),";
            api_url = api_url + "module_views,";
            api_url = api_url + "payment_types,";
            api_url = api_url + "customer_groups,";
            api_url = api_url + "settings,";
            api_url = api_url + "currencies,";
            api_url = api_url + "roles";

            GetResponse res = await http.ApiGet(api_url);
            if (res.IsSuccess)
            {
                gv = JsonSerializer.Deserialize<GlobalVariableModel>(res.Content.ToString()); 
            }

        }

        async Task CheckUserLogin()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;
            if (user?.Identity != null)
            {
                if (user.Identity.IsAuthenticated)
                {
                    UserModel _users = new UserModel();
                    var user_id = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var res_user = await http.ApiGet($"user/get_user?user_id={user.FindFirst(ClaimTypes.NameIdentifier).Value}&$expand=role($select=role_name)");
                    
                    if (res_user.IsSuccess)
                    {
                        _users = JsonSerializer.Deserialize<UserModel>(res_user.Content.ToString());
                        if (_users!= null)
                        {
                            current_login_user = _users;

                        }
                        else
                        {
                            is_logined = false;
                        }
                        
                    }
                }
            }else
            {
                is_logined = false;
                nav.NavigateTo("auth/login");
            }

        }


        public string GetRole(string option_name)
        {

            if (gv.permission_options.Count() > 0)
            {
                var d = gv.permission_options.Where(r => r.option_name == option_name && (r.roles ?? "") != "");
                if (d.Count() > 0)
                {
                    string role = d.FirstOrDefault().roles;
                    return role;
                }
            }
            return option_name;
        }

          public List<PermissionOptionModel> GetPermissionOption(PermissionOptionModel parent)
        {

            if (gv.permission_options.Count() > 0)
            {
                var d = gv.permission_options.Where(r => r.parent_id == parent.id && r.show_in_menu == true).OrderBy(r=>r.sort_order);
                return d.ToList();
            }
            return new List<PermissionOptionModel>(); ;
        }


        public List<ModuleViewModel> GetModuleView(string view_name)
        {

            return gv.module_views.Where(r => r.module_name == view_name).OrderBy(r => r.sort_order).ToList();

        }




    }
}