
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using eShareModel;

namespace eModels
{
    [Table("tbl_product_ingredient")]
    public  class ProductIngredientModel   : CoreModel
    {

        public int product_menu_id { get; set; }
        [ForeignKey("product_menu_id")]
        public ProductModel product_menu { get; set; }
         public int product_ingredient_id { get; set; }
        [ForeignKey("product_ingredient_id")]
        public ProductModel product_ingredient { get; set; }

        public decimal quantity { get; set; } = 1;

        public string unit { get; set; } = "Unit";
        public decimal cost { get; set; } 

        private decimal _total_cost;

        public decimal total_cost
        {
            get {

                return quantity * cost; 
            }
            set { _total_cost = value; }
        }
    }
}
