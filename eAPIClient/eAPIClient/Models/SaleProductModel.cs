using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_sale_product")]
    public class SaleProductModel : SaleProductShareModel
    {
        public SaleProductModel()
        {
            sale_product_modifiers = new List<SaleProductModifierModel>();
        }
      
        [ForeignKey("sale_id")]
        public SaleModel sale { get; set; }
       
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        //List
        public List<SaleProductModifierModel> sale_product_modifiers { get; set; }


    }

    [Table("tbl_sale_product_modifier")]
    public class SaleProductModifierModel : SaleProductModifierShareModel
    {

        [ForeignKey("sale_product_id")]
        public SaleProductModel sale_product { get; set; }    
    }
}
