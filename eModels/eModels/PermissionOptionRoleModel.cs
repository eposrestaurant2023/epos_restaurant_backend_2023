using System;
using System.ComponentModel.DataAnnotations.Schema;   

namespace eModels
{
    [Table("tbl_permission_option_role")]
    public class PermissionOptionRoleModel
    {

        public PermissionOptionRoleModel(){}
        public PermissionOptionRoleModel(int _role_id, int _permission_option_id)
        {
            role_id = _role_id;
            permission_option_id = _permission_option_id;
        }


        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public virtual RoleModel role { get; set; }

        public int permission_option_id { get; set; }
        [ForeignKey("permission_option_id")]
        public virtual PermissionOptionModel permission_option { get; set; }


    }
}
