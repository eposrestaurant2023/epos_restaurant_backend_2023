
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
        public DateTime? start_date { get; set; } =   DateTime.Now;


        public bool is_base_on_hour { get; set; } = false;

        [DataType(DataType.Time)]
        public DateTime? start_time { get; set; }


        [Column(TypeName = "Date")]
        public DateTime? end_date { get; set; } = DateTime.Now;

        
        [ DataType(DataType.Time)]
        public DateTime? end_time { get; set; }

        public List<DiscountPromotionItemModel> discount_promotion_items { get; set; }
                                                                                                     
    }

    [Table("tbl_discount_promotion_item")]
    public class DiscountPromotionItemModel : CoreGUIDModel
    {
        public Guid discount_promotion_id { get; set; }

        [ForeignKey("discount_promotion_id")]
        public DiscountPromotionModel discount_promotion { get; set; }

        public int product_category_id { get; set; }

        [ForeignKey("product_category_id")]
        public ProductCategoryModel product_category { get; set; }   

        public decimal discount_value { get; set; } = 0;
        public string discount_type { get; set; } = "Percent";

    }
}
