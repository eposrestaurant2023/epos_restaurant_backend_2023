using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace eModels
{
    [Table("tbl_discount_code")]
    public class DiscountCodeModel    : CoreModel
    {
        public DiscountCodeModel()
        {
            discount_code_business_branchs = new List<DiscountCodeBusinessBranchModel>();
        }

        [MaxLength(50)]
        public string discount_label { get; set; }

        public decimal discount_value { get; set; }


        public List<DiscountCodeBusinessBranchModel> discount_code_business_branchs { get; set; }
    }
}
