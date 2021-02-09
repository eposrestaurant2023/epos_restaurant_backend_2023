﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_price_rule")]
   public class PriceRuleModel  : CoreModel
    {
        public PriceRuleModel()
        {
            business_branch_prices = new List<BusinessBranchPriceRule>();
        }
        [MaxLength(50)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string price_name { get; set; }
   

        public List<BusinessBranchPriceRule> business_branch_prices { get; set; }
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
        public bool is_default { get; set; }
    }




    [Table("tbl_product_price")]

    public class ProductPriceModel : CoreModel
    {
     
        
        public int product_portion_id { get; set; }
        [ForeignKey("product_portion_id")]
        public ProductPortionModel product_portion { get; set; }
        public int price_rule_id { get; set; }
        [ForeignKey("price_rule_id")]
        public PriceRuleModel price_rule { get; set; }
        public decimal price { get; set; }

 
    }
    
    [Table("tbl_business_branch_product_price")]

    public class BusinessBranchProductPriceModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }

        public Guid business_branch_id { get; set; }

        public int product_portion_id { get; set; }
         
      
        public string prices { get; set; }
     
    }





}