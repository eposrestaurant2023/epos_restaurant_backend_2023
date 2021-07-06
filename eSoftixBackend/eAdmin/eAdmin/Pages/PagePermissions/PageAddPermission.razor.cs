using eModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace eAdmin.Pages.PagePermissions
{
    public class PageAddPermissions:PageCore
    {
        [Parameter] public int role_id { get; set; }
        [Parameter] public bool is_role_check { get; set; }
        public bool is_checked;
        public bool input_error;
        public HashSet<int> SelectedOption { get; set; } = new HashSet<int>();
        public RoleModel role = new RoleModel();
        public List<RoleModel> roles { get; set; } = new List<RoleModel>();
        public List<PermissionModel> permissions { get; set; } = new List<PermissionModel>();
        public Dictionary<string, object> formAttributes { get; set; } = new Dictionary<string, object>()
        {
            { "class", "uk-form-horizontal" }
        };
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;

            await LoadData();
            await LoadPermission();
            is_loading = false;
        }

        protected async Task SaveRole_Click()
        {
            is_saving = true;
           
                
                var post = await http.ApiPost($"Role/SavePermission?options_id={role.options_id}", role);
                if (post.IsSuccess)
                {
                    AddToast("Saved successfully");
                    nav.NavigateTo("role");
                }
                else
                {
                    AddToast(post.Content.ToString(), Severity.Warning);
                }
           
            is_saving = false;
        }

        protected async Task LoadData()
        {
            if (role_id > 0)
            {
                string query = $"role({role_id})?$expand=permission_option_roles($select=role_id,permission_option_id)";
                var res = await http.ApiGet(query);
                if (res.IsSuccess)
                {
                    role = JsonSerializer.Deserialize<RoleModel>(res.Content.ToString());
                }
            }
        }
        public async Task LoadPermission()
        {
            var resp = await http.ApiPost("GetData",new FilterModel() { 
                procedure_parameter = $"{role_id}",
                procedure_name = "sp_get_permission_option_by_role_id_json"
            });
            if (resp.IsSuccess)
            {
                permissions = JsonSerializer.Deserialize<List<PermissionModel>>(resp.Content.ToString());
            }
        }
    }

    public class PermissionModel
    {
        public int id { get; set; }
        public string note { get; set; }
        public bool is_selected { get; set; }

        public List<PermissionModel> ChildItem { get; set; }
    }
}
