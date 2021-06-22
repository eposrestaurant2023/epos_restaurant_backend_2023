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
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using eAdmin.Services;
using System.Text.Json;
using NETCore.Encrypt;
using Blazored.LocalStorage;

namespace eAdmin.Pages.PageAuth
{
    public partial class PageLogin
    {
        AuthenticateModel model = new AuthenticateModel();


        public string Message { get; set; }
        public bool IsShowMessage { get; set; }
        public bool IsSpinning { get; set; }
        [Inject] protected IHttpService http { get; set; }
        [Inject] protected NavigationManager nav { get; set; }
        [Inject] protected ILocalStorageService storageService { get; set; }
        [Inject] protected AuthenticationStateProvider authenticationStateProvider { get; set; }

        [CascadingParameter] public AppState state { get; set; }


        public bool PasswordVisibility { get; set; }
        public InputType PasswordInput { get; set; } = InputType.Password;

        public string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;

        public void TogglePasswordVisibility()
        {
            if (PasswordVisibility)
            {
                PasswordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        public async Task LoginUser()
        {
            IsSpinning = true;
            if (string.IsNullOrWhiteSpace(model.Username) || model.Password == "")
            {
                IsShowMessage = true;
                Message = "Username or Password is required.";
                IsSpinning = false;
                return;
            }

            var resp = await http.ApiPost("user/auth/login", model);
            if (resp.IsSuccess)
            {
                UserModel user = JsonSerializer.Deserialize<UserModel>(resp.Content.ToString());
                if (user != null)
                {
                    try
                    {
                        var user_convert = EncryptProvider.Base64Encrypt(resp.Content.ToString());
                        await storageService.SetItemAsync("_Authorization", user_convert);
                        await authenticationStateProvider.GetAuthenticationStateAsync();


                        if (state.gv == null)
                        {
                            await state.GetGlobalVariable();
                        }
                        if (state.gv != null)
                        {
                            state.is_logined = true;
                            state.current_login_user = user;
                            nav.NavigateTo("dashboard");
                        }

                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        IsShowMessage = true;
                        Message = "Invalid username and password.";
                    }
                }
                else
                {
                    IsShowMessage = true;
                    Message = "Invalid username and password.";
                }
            }
            else
            {
                IsShowMessage = true;
                Message = "Invalid username and password.";
            }

            IsSpinning = false;
        }
    }
}