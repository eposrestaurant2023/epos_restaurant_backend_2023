

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_predefined_payment_amount")]
    public class PredefinedPaymentAmountModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string prefix_price_value { get; set; }
        public int payment_type_id { get; set; }
        public int currency_id { get; set; }
        public int sort_order { get; set; }
        public bool status { get; set; }
    }
}
