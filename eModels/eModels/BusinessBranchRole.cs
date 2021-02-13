using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_business_branch_role")]
    public class BusinessBranchRoleModel
    {
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public virtual RoleModel role { get; set; }

        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public virtual BusinessBranchModel business_branch { get; set; }

        public bool is_delete { get; set; } = false;
    }
}
