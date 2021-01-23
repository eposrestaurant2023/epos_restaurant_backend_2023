
using Microsoft.AspNetCore.Components;
 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eModels;
using System.Text.Json;


namespace eAdmin.Pages.PagesUsersAndPermissions
{
    public class PageEditPermissionOptionBase:PageCore
    {
        [Parameter] public int id { get; set; }
        [Parameter] public bool is_role_check { get; set; }
        
        public bool input_error;
         
        public PermissionOptionModel permission = new PermissionOptionModel();
        public RoleModel role = new RoleModel();
        public List<RoleModel> roles { get; set; } = new List<RoleModel>();
        string Apirole = "Role?$select=role_name,id&$expand=permission_option_roles($select=permission_option_id)";
    
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
           
            await LoadData();
            await LoadRoles();
            is_loading = false;
        }

        protected async Task SavePermission_Click()
        {
            var post = await http.ApiPost("PermissionOption/save", permission);
            if (post.IsSuccess)
            {
                toast.Add("Save Successfully",MatBlazor.MatToastType.Success);
                nav.NavigateTo("permissionoption");
            }
            else
            {
                toast.Add(post.Content.ToString(), MatBlazor.MatToastType.Success);
            }
        }

        protected async Task LoadData()
        {
            if (id > 0)
            {
                string query = $"permissionoption({id})?$select=id,parent_id,option_name&$expand=permission_option_roles";
                var res = await http.ApiGet(query);
                if (res.IsSuccess)
                {
                    permission = JsonSerializer.Deserialize<PermissionOptionModel>(res.Content.ToString());
                    if (permission.id == 0)
                    {
                        is_error = true;
                    }
                    
                }
            }

        }
        public async Task LoadRoles()
        {
            
            if (roles.Count() == 0)
            {
                var res = await http.ApiGetOData(Apirole);
                if (res.IsSuccess)
                {
                    roles = JsonSerializer.Deserialize<List<RoleModel>>(res.Content.ToString());
                }
            }

        }
       
        
        public async Task SelectedRole_Click(RoleModel r)
        {
            await Task.Delay(100);
            if (r.permission_option_roles.Where(r=>r.permission_option_id == id).Count() == 0)
            {
                permission.permission_option_roles.Add(new PermissionOptionRoleModel()
                {
                    role_id = r.id,
                });
            }
            else
            {
                permission.permission_option_roles.Where(r => r.permission_option_id == id).FirstOrDefault().is_delete = true;
                
            }
            
        }
        public void SelectAllRoleClick(bool is_select = false)
        {
            is_role_check = is_select;
        }
    }
}
