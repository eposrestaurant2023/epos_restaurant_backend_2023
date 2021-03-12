
using eModels.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using eShareModel;
using System.Text.Json.Serialization;

namespace eModels
{
    [Table("tbl_stock_location")]
    public class StockLocationModel
    {
        public StockLocationModel()
        {
            stock_location_products = new List<StockLocationProductModel>();
        }
        [Key]
        public int id { get; set; }
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        public string stock_location_name { get; set; }
        public bool is_default { get; set; }

        [NotMapped, JsonIgnore]
        public string bustiness_branch_name { get; set; }

        public List<StockLocationProductModel> stock_location_products { get; set; }
    }

    
    [Table("tbl_stock_location_product")]
    public class StockLocationProductModel
    {
        [Key]
        public int id { get; set; }
        public int stock_location_id { get; set; }
        [ForeignKey("stock_location_id")]
        public StockLocationModel stock_location { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

 
        public decimal quantity { get; set; }
        public string unit { get; set; } = "Unit";
        private decimal _multipler = 1;

        public decimal multiplier
        {
            get { return _multipler; }
            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                _multipler = value;

            }
        }
        public decimal min_quantity { get; set; }
        public decimal max_quantity { get; set; }

        public decimal initial_quantity { get; set; } = 0;
        public decimal initial_adjustment_quantity { get; set; } = 0;
        
        [NotMapped, JsonIgnore]
        public decimal adjust_quantity { get; set; } 

        [NotMapped, JsonIgnore]
        public bool is_out_of_stock
        {
            get
            {
                return quantity <= 0;
            }
        }

        [NotMapped, JsonIgnore]
        public bool is_low_stock
        {
            get
            {
                return ((quantity < min_quantity && quantity > 0) && min_quantity > 0);
            }
        }

        [NotMapped, JsonIgnore]
        public bool is_over_stock
        {
            get
            {
                return (quantity > max_quantity && max_quantity > 0);
            }
        }

    }


}
