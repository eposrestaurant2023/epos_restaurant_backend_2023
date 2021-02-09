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
        }


        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(50)]
        public string role_name { get; set; }
        public string description { get; set; }

        public bool is_buildin { get; set; }

        public List<PermissionOptionRoleModel> permission_option_roles { get; set; }  
    }
}
