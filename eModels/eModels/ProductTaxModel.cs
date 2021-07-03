using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eModels
{
    [Table("tbl_product_tax")]
    public class ProductTaxModel : CoreModel
    {
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        public decimal tax_1_rate { get; set; }
        public decimal tax_2_rate { get; set; }
        public decimal tax_3_rate { get; set; }

    }
}
