using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_sale_product")]
    public class SaleProductModel : SaleProductShareModel
    {
        public Guid sale_id { get; set; }
        [ForeignKey("sale_id")]
        public SaleModel sale { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        public int product_type_id { get; set; } = 1;
        [ForeignKey("product_type_id")]
        public ProductTypeModel product_type { get; set; }
    }
}
