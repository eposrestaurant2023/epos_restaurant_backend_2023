using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eModels
{
    [Table("tbl_role")]
    public class RoleModel : CoreModel
    {
        public RoleModel()
        {                                      
            permission_option_roles = new List<PermissionOptionRoleModel>();
            business_branch_roles = new List<BusinessBranchRoleModel>();
        }


        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(50)]
        public string role_name { get; set; }
        public string description { get; set; }

        public bool is_buildin { get; set; }

        public List<PermissionOptionRoleModel> permission_option_roles { get; set; }  
        public List<BusinessBranchRoleModel> business_branch_roles { get; set; }  
    }
}
