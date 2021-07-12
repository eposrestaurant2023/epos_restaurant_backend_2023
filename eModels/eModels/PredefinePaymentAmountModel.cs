

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_predefine_payment_amount")]
    public class PredefinePaymentAmountModel
    {
        [Key]
        public int id { get; set; }
        public decimal predefine_value { get; set; }
        public int payment_type_id { get; set; } 
        public int sort_order { get; set; }
        public bool status { get; set; }
    }
}
