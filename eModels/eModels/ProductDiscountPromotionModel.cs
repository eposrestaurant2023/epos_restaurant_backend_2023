﻿
using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_discount_promotion")]
    public class DiscountPromotionModel   : CoreGUIDModel
    {
        public Guid business_branch_id { get; set; }

        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        public string title { get; set; }
        public string description { get; set; }

        [Column(TypeName = "Date")]
        public DateTime start_date { get; set; }


        [DataType(DataType.Time)]
        public DateTime start_time { get; set; }


        [Column(TypeName = "Date")]
        public DateTime end_date { get; set; }

        
        [ DataType(DataType.Time)]
        public DateTime end_time { get; set; }

        public List<ProductDiscountPromotionModel> product_promotions { get; set; }
                                                                                                     
    }

    [Table("tbl_product_disocunt_promotion")]
    public class ProductDiscountPromotionModel : CoreGUIDModel
    {
        public Guid dicount_promotion_id { get; set; }

        [ForeignKey("dicount_promotion_id")]
        public DiscountPromotionModel dicount_promotion { get; set; }

        public int product_category_id { get; set; }

        [ForeignKey("product_category_id")]
        public ProductCategoryModel product_category { get; set; }

        public decimal discount_percentage { get; set; }

    }
}
