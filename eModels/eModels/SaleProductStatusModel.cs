using eShareModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_sale_product_status")]
    public  class SaleProductStatusModel      : SaleProductStatusShareModel
    {
    }

    [Table("tbl_sale_status")]
    public class SaleStatusModel : SaleStatusShareModel
    {
    }
}
