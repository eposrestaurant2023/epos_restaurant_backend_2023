using System;              
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
    public class SalePaymentShareModel : CoreGUIDModel
    {
        public Guid? sale_id { get; set; }
        public int payment_type_id { get; set; }

        public string reference_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime payment_date { get; set; } = DateTime.Now;

        public Guid outlet_id { get; set; }

        public decimal payment_amount { get; set; }

        public string payment_note { get; set; }

        public bool is_create_payment_in_sale_order { get; set; }
    }
}
