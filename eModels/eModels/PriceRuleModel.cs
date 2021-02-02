
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_price_rule")]
   public class PriceRuleModel  : CoreModel
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string price_name { get; set; }
    }


    [Table("tbl_product_price")]

    public class ProductPriceModel : CoreModel
    {
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        public int price_rule_id { get; set; }
        [ForeignKey("price_rule_id")]
        public PriceRuleModel price_rule { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string portion_name { get; set; }

        public decimal multiplier { get; set; }

        public decimal cost { get; set; }

        public decimal price { get; set; }

        public bool is_default { get; set; } = false;
    }

    [Table("tbl_business_branch_price_rule")]
    public class BusinessBranchPriceRule
    {
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        public int price_rule_id { get; set; }
        [ForeignKey("price_rule_id")]
        public PriceRuleModel price_rule { get; set; }
    }



}
