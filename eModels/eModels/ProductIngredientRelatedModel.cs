using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_product_ingredient_related")]
   public class ProductIngredientRelatedModel
    {
        [Key]
        public int id { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
        public int ingredient_id { get; set; }
        [ForeignKey("ingredient_id")]
        public ProductModel ingredient { get; set; }

    }
}
