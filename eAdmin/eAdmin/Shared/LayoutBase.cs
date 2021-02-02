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
        [Inject] protected IMatToaster toast { get; set; }
        [Inject] protected IConfiguration config { get; set; }

        public GlobalVariableModel gv = new GlobalVariableModel();

        public bool is_dialog_open { get; set; } = false;
        public bool is_menu_open { get; set; } = true;

     
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
                    var res_user = await http.ApiGet($"user/get_user?user_id={user.FindFirstValue(ClaimTypes.NameIdentifier)}&$expand=role($select=role_name),employee,outlet");
                    if (res_user.IsSuccess)
                    {
                        _users = JsonSerializer.Deserialize<List<UserModel>>(res_user.Content.ToString());
                        if (_users.FirstOrDefault() != null)
                        {
                            gv.current_login_user = _users.FirstOrDefault();    
                            
                        }
                        else
                        {
                            nav.NavigateTo("/", true);
                        }
                    }
                }

                if (user.Identity.IsAuthenticated)
                {
                    IsLoading = true;
                    UserModel _users = new UserModel();
                    string user_url = $"user/get_user?user_id={user.FindFirstValue(ClaimTypes.NameIdentifier)}&$expand=role($select=role_name)";
                    var res_user = await http.ApiGet(user_url);
                    if (res_user.IsSuccess)
                    {
                        _users = JsonSerializer.Deserialize<UserModel>(res_user.Content.ToString());

                        gv.current_login_user = _users;
                        

                    }
                    else
                    {
                        nav.NavigateTo("/auth/logout");
                    }

                    string api_url = "GlobalVariable?$expand=";
                    api_url = api_url + "permission_options($select=id,parent_id,option_name,roles,url,report_title,report_title_kh,note,icon,show_in_menu,sort_order),";
                    api_url = api_url + "module_views,";
                    api_url = api_url + "payment_types,";
                    api_url = api_url + "product_groups,";
                    api_url = api_url + "product_categories,";   
                    api_url = api_url + "customer_groups,";
                    api_url = api_url + "settings,";
                    api_url = api_url + "currencies,";
                    api_url = api_url + "business_info,";
                    api_url = api_url + "bussiness_branches,";
                    api_url = api_url + "roles,";
                    api_url = api_url + "product_types,";
                    api_url = api_url + "countries,";
                    api_url = api_url + "outlets,";
                    api_url = api_url + "stock_locations($expand=outlet),";
                    api_url = api_url + "printers,";
                    api_url = api_url + "vendors";
                 
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

            string showhidemenustate = await localStorage.GetItemAsync<string>("showhidemenu");

            if (String.IsNullOrEmpty(showhidemenustate))
            {
                showhidemenustate = "1";
            }
            is_menu_open = showhidemenustate == "1";

            IsLoading = false;
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
            if (firstRender)
            {
                await js.InvokeVoidAsync("SetActiveMenu");
            }
            
            is_loading_default = false;
        }
        public bool is_spinner = false;
        public async Task LoadData()
        {
            is_spinner = true;
            string api_url = "GlobalVariable?$expand=";
            api_url = api_url + "product_categorys,";
            api_url = api_url + "customer_groups,";
            api_url = api_url + "employees($filter=is_deleted eq false),";
            api_url = api_url + "positions,";
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
