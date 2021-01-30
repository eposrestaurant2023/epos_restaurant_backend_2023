
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_modifier")]
   public class ModifierModel     : CoreModel
    {
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string modifier_name { get; set; }


    }

    [Table("tbl_product_modifier")]

    public class ProductModifierModel: CoreModel
    {
        public int modifier_id { get; set; }
        [ForeignKey("modifier_id")]
        public ModifierModel modifier { get; set; }

        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        public decimal price { get; set; }      
    }
}
