using eModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageUsers
{
    public class PageUserProfiles :PageCore
    {
        [CascadingParameter] public AppState app { get; set; }
        public UserModel model { get; set; }
        public string page_title { get; set; }
        public bool is_changepassword = false;
        public string api_url
        {
            get
            {
                return $"user({app.current_login_user?.id})?$expand=role";
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await LoadUser();
        }
        async Task LoadUser()
        {
            if (!is_loading)
            { 
                is_loading = true;
                var res = await http.ApiGet(api_url);
                model = JsonSerializer.Deserialize<UserModel>(res.Content.ToString());
                model.password = EncryptProvider.Base64Decrypt(model.password);
                is_loading = false;
            }
        }
        public async Task Save_Click()
        {
            model.is_saving = true;
            UserModel save_model = JsonSerializer.Deserialize<UserModel>(JsonSerializer.Serialize(model));
            if (is_changepassword)
            {
                if (!string.IsNullOrEmpty(model.current_password) && !string.IsNullOrEmpty(model.new_password) && !string.IsNullOrEmpty(model.retype_new_password))
                {
                    var res = await http.ApiPost($"user/changepassword?", save_model);
                    if (res.IsSuccess)
                    {
                        toast.Add("Save Successfull.", Severity.Success);
                        var c = JsonSerializer.Deserialize<UserModel>(res.Content.ToString());
                        app.current_login_user = JsonSerializer.Deserialize<UserModel>(res.Content.ToString());
                        await localStorage.RemoveItemAsync("_Authorization");
                        var user_convert = EncryptProvider.Base64Encrypt(res.Content.ToString());
                        await localStorage.SetItemAsync("_Authorization", user_convert);
                    }
                    else
                    {
                        toast.Add(res.Content.ToString(), Severity.Warning);
                    }
                }
                else
                {
                    toast.Add("Enter New Password");
                }
            }
            else
            {
                var res = await http.ApiPost($"user/save", save_model);
                if (res.IsSuccess)
                {
                    toast.Add("Save Successfull.", Severity.Success);
                    var c = JsonSerializer.Deserialize<UserModel>(res.Content.ToString());
                }
                else
                {
                    toast.Add(res.Content.ToString(), Severity.Warning);
                }
               
            }

           
            model.is_saving = false;
        }


    }
}
