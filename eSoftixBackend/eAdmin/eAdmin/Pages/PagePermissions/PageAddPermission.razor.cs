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
        public bool is_saving,is_checked;
        public bool input_error;
        public List<PermissionOptionModel> PermissionOptions = new List<PermissionOptionModel>();
        public RoleModel role = new RoleModel();
        public List<RoleModel> roles { get; set; } = new List<RoleModel>();

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
            role.permission_option_roles = PermissionOptions.SelectMany(r => r.permission_option_roles).ToList();
            var post = await http.ApiPost("Role/SavePermission", role);
            if (post.IsSuccess)
            {
                AddToast("Saved successfully");
                nav.NavigateTo("role");
            }
            else
            {
                AddToast(post.Content.ToString(),Severity.Warning);
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

            var res = await http.ApiGetOData($"PermissionOption?$select=id,option_name,note,parent_id&$expand=permission_options,permission_option_roles($select=role_id,permission_option_id;$filter=role_id eq {role_id})");
            if (res.IsSuccess)
            {

                PermissionOptions = JsonSerializer.Deserialize<List<PermissionOptionModel>>(res.Content.ToString());
            }
        }

        public async Task SelectedPermission_Click(PermissionOptionModel p)
        {
            await Task.Delay(100);
            if (p.permission_option_roles.Count > 0)
            {
                p.permission_option_roles.Clear();
            }
            else
            {
                p.permission_option_roles.Add(new PermissionOptionRoleModel() { role_id = role_id, permission_option_id = p.id });
            }
        }
        public async Task SelectAllRoleClick(bool is_select = false)
        {
            await Task.Delay(10);
            if (is_select)
            {
                PermissionOptions.Where(r => r.permission_option_roles.Count == 0).Select(r => { r.permission_option_roles.Add(new PermissionOptionRoleModel(role_id, r.id)); return r; }).ToList();
                Console.WriteLine(JsonSerializer.Serialize(PermissionOptions));
            }
            else
            {
                PermissionOptions.Where(r => r.permission_option_roles.Count > 0).Select(r => { r.permission_option_roles.Clear(); return r; }).ToList();
            }


        }
    }
}
