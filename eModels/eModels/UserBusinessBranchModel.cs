using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_user_business_branch")]
    public class UserBusinessBranchModel  
    {
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public UserModel user { get; set; }


        public int business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
    }
}
