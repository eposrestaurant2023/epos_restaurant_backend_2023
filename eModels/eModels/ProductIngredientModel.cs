
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

       
        public int ingredient_id { get; set; }
        [ForeignKey("ingredient_id")]
        public ProductModel ingredient { get; set; }
        
        public int product_portion_id { get; set; }
        [ForeignKey("product_portion_id")]
        public ProductPortionModel product_portion { get; set; }

        public decimal quantity { get; set; } = 1;

        public int unit_id { get; set; }
        [ForeignKey("unit_id")]
        public UnitModel unit { get; set; }
         
        private decimal _total_cost;

        public decimal total_cost
        {
            get {

                return quantity * cost; 
            }
            set { _total_cost = value; }
        }

        private decimal _cost;

        public decimal cost
        {
            get {
                if (ingredient != null && id == 0)
                {
                    return ingredient.cost;
                }
                return _cost;
            }
            set { _cost = value; }
        }

    }
}
