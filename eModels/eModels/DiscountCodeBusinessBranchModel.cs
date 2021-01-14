using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_discount_code_business_branch")]
    public class DiscountCodeBusinessBranchModel
    {
        public Guid discount_code_id { get; set; }
        [ForeignKey("discount_code_id")]
        public DiscountCodeModel discount_code { get; set; }


        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
    }
}
