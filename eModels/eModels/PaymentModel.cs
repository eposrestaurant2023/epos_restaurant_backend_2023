using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace eModels
{

    [Table("tbl_payment")]
    public class PaymentModel : CoreGUIDModel
    {
        public PaymentModel()
        {
            histories = new List<HistoryModel>();
        }
        public List<HistoryModel> histories { get; set; }
        public string reference_number { get; set; }

        public int payment_type_id { get; set; }
        [ForeignKey("payment_type_id")]
        public PaymentTypeModel payment_type { get; set; }

        [Column(TypeName = "date")]
        public DateTime payment_date { get; set; } = DateTime.Now;

        public Guid? sale_id { get; set; }
        [ForeignKey("sale_id")]
        public SaleModel sale { get; set; }
        
        public int outlet_id { get; set; }

        public decimal payment_amount { get; set; }

        public string payment_note { get; set; }

        public bool is_create_payment_in_sale_order { get; set; }

        [NotMapped, JsonIgnore]
        public bool is_add_note { get; set; }
    }
}
