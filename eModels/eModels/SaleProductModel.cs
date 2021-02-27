using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eShareModel;

namespace eModels
{
    [Table("tbl_sale_product")]
    public class SaleProductModel : SaleProductShareModel
    {                 
        [ForeignKey("sale_id")]
        public virtual SaleModel sale { get; set; }    
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }  

    }

    [Table("tbl_sale_product_modifier")]
    public class SaleProductModifierModel : SaleProductModifierShareModel
    {

        [ForeignKey("sale_product_id")]
        public SaleProductModel sale_product { get; set; }
    }
}
