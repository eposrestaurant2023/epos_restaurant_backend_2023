using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_customer_business_branch")]
    public class CustomerBusinessBranchModel
    {
        public Guid customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }


        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }       
    }
}
