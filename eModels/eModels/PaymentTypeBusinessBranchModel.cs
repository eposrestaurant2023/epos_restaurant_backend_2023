using System;                          
using System.ComponentModel.DataAnnotations.Schema;    

namespace eModels
{
    [Table("tbl_payment_type_business_branch")]
    public class PaymentTypeBusinessBranchModel
    {
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }


        public Guid payment_type_id { get; set; }
        [ForeignKey("payment_type_id")]
        public PaymentTypeModel payment_type { get; set; }
    }
}
