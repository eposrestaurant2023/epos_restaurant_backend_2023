using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eShareModel;

namespace eModels
{
    [Table("tbl_purchase_order_payment")]
    public class PurchaseOrderPaymentModel : CoreGUIDModel
    {
        public PurchaseOrderPaymentModel()
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

        public int purchase_order_id { get; set; }
        [ForeignKey("purchase_order_id")]
        public PurchaseOrderModel purchase_order { get; set; }

        public decimal payment_amount { get; set; }

        public string payment_note { get; set; }

        public bool is_create_payment_in_puchase_order { get; set; }

        [NotMapped, JsonIgnore]
        public bool is_add_note { get; set; }
    }
}
