using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using MatBlazor;
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




namespace eAdmin.Shared
{
    public class LayoutBase : LayoutComponentBase
    {
        [CascadingParameter] Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject] protected NavigationManager nav { get; set; }
        [Inject] protected IJSRuntime js { get; set; }
        [Inject] protected ILocalStorageService localStorage { get; set; }
        [Inject] protected IHttpService http { get; set; }
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] protected ISnackbar toast { get; set; }
        [Inject] protected IConfiguration config { get; set; }

        public GlobalVariableModel gv = new GlobalVariableModel();

        public bool is_dialog_open { get; set; } = false;

        public bool _drawerOpen = true;

       public void DrawerToggle()
        {
            _drawerOpen = true ;
        }



        public MudTheme MyCustomTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = Colors.Blue.Default,
                Secondary = Colors.Green.Accent4,
                AppbarBackground = Colors.Red.Default,


            },

            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "243px",
                DrawerWidthRight = "500px"
            }
        };

        public string image_base_url
        {
            get
            {
                return config["BaseUrl"] + "upload/";
            }
        }

        public bool is_loading_default { get; set; } = true;
        public bool IsLoading { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
        

            var authState = await authenticationStateTask;
            var user = authState.User;
            if (user?.Identity != null)
            {
                if (!user.Identity.IsAuthenticated)
                {
                    List<UserModel> _users = new List<UserModel>();
                    var res_user = await http.ApiGet($"user/get_user?user_id={user.FindFirstValue(ClaimTypes.NameIdentifier)}");
                    if (res_user.IsSuccess)
                    {
                        _users = JsonSerializer.Deserialize<List<UserModel>>(res_user.Content.ToString());
                        if (_users.FirstOrDefault() != null)
                        {
                            gv.current_login_user = _users.FirstOrDefault();

                        }
                        else
                        {
                            nav.NavigateTo("/");
                        }
                    }else
                    {
                        await Logout();
                    }
                }
                 

                if (user.Identity.IsAuthenticated)
                {
                    IsLoading = true;
                    UserModel _users = new UserModel();
                    string user_url = $"user/get_user?user_id={user.FindFirstValue(ClaimTypes.NameIdentifier)}";
                    var res_user = await http.ApiGet(user_url);
                    if (res_user.IsSuccess)
                    {
                        _users = JsonSerializer.Deserialize<UserModel>(res_user.Content.ToString());

                        gv.current_login_user = _users;
                        

                    }
                    else
                    {
                         await Logout();
                    }

                    string api_url = "GlobalVariable?$expand=";
                    api_url = api_url + "permission_options($select=id,parent_id,option_name,roles,url,report_title,report_title_kh,note,icon,show_in_menu,sort_order,is_public_report),";
                    api_url = api_url + "module_views,";
                    api_url = api_url + "payment_types,";
                    api_url = api_url + "product_groups,";
                    api_url = api_url + "product_categories,";
                    api_url = api_url + "customer_groups,";
                    api_url = api_url + "settings,";
                    api_url = api_url + "currencies,";
                    api_url = api_url + "business_info,";
                    api_url = api_url + "bussiness_branches($expand=business_branch_prices;$filter=is_deleted eq false and status eq true),";
                    api_url = api_url + "roles,";
                     
                    api_url = api_url + "countries,";
                    api_url = api_url + "outlets($expand=stations),";
                    api_url = api_url + "category_notes,";
                    api_url = api_url + "stock_locations($expand=business_branch),";
                    api_url = api_url + "printers,";
                    api_url = api_url + "price_rules,";
                    api_url = api_url + "provinces,";
                    api_url = api_url + "vendors,";
                    api_url = api_url + "vendor_groups,";
                    api_url = api_url + "units,";
                    api_url = api_url + "product_categories,";
                    api_url = api_url + "revenue_groups,";
                    api_url = api_url + "inventory_transaction_type,";
                    api_url = api_url + "unit_categories,";
                    api_url = api_url + "kitchen_groups,";
                    api_url = api_url + "business_branch_system_features,";
                    api_url = api_url + "sale_types,";
                    api_url = api_url + "system_features";

                    GetResponse res = await http.ApiGet(api_url);

                    if (res.IsSuccess)
                    {
                        gv = JsonSerializer.Deserialize<GlobalVariableModel>(res.Content.ToString());
                        gv.current_login_user = _users;
                       
                    }
                    
                }
            }
            else
            {
                gv = new GlobalVariableModel();
               
            }

           
            
            var lang = await GetCurrentLanguage();
            gv.current_language = lang;
            gv.image_base_url = config["BaseUrl"] + "upload/";
            IsLoading = false;
        }
        public async Task<LanguageModel> GetCurrentLanguage()
        {
            string current_lang = await localStorage.GetItemAsync<string>("current_language");
            
            if (current_lang != null)
            {
                return await ChangeLanguage_Click(current_lang);
            }
            else
            {
                return await ChangeLanguage_Click("km-KH");
            }

        }
        public async Task<LanguageModel> ChangeLanguage_Click(string current_language_id)
        {
            LanguageModel l = new LanguageModel();
            if (current_language_id == "en-US")
            {
                await js.InvokeVoidAsync("SetLangEn");
                l = new LanguageModel()
                {
                    language_id = "en-US",
                    language_name = "English",
                    image_url = "images/en.jpg"
                };
            }
            else
            {
                await js.InvokeVoidAsync("SetLangKh");
                l = new LanguageModel()
                {
                    language_id = "km-KH",
                    language_name = "Khmer",
                    image_url = "images/kh.jpg"
                };
            }



            await localStorage.SetItemAsync("current_language", current_language_id);
            return l;

        }

        async Task Logout() {
            var localStateProvider = (LocalAuthenticationStateProvider)AuthenticationStateProvider;
            await localStateProvider.LogoutAsync();
            nav.NavigateTo("login");
        }

        public void ToggleUserModal()
        {

            is_dialog_open = !is_dialog_open;
        }

        public async Task ShowHideMenu()
        {
            string showhidemenustate = await localStorage.GetItemAsync<string>("showhidemenu");

            if (String.IsNullOrEmpty(showhidemenustate))
            {
                showhidemenustate = "1";
            }
            await localStorage.SetItemAsync("showhidemenu", showhidemenustate == "1" ? "0" : "1");
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            
            is_loading_default = true;
            
            await js.InvokeVoidAsync("SetActiveMenu");
            
            is_loading_default = false;
        }
        public bool is_spinner = false;
        public async Task LoadData()
        {
            is_spinner = true;
            string api_url = "GlobalVariable?$expand=";
            api_url = api_url + "product_categories,";
            api_url = api_url + "customer_groups,";
            api_url = api_url + "bussiness_branches";
            GetResponse res = await http.ApiGet(api_url);
            if (res.IsSuccess)
            {
                GlobalVariableModel temp_gv = new GlobalVariableModel();
                temp_gv =JsonSerializer.Deserialize<GlobalVariableModel>(res.Content.ToString());
           
               
            }
            is_spinner = false;
        }
    }
}
