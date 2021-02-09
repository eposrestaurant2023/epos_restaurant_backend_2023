using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{

    [Table("tbl_history")]
    public class HistoryModel : CoreModel
    {
        public HistoryModel()
        {

        }

        public HistoryModel(string _title)
        {
            title = _title;

        }

        public string title { get; set; }


        public string description { get; set; }

        public string note { get; set; } = "";

        [Column(TypeName = "date")]
        public DateTime? transaction_date { get; set; }

        public int? outlet_id { get; set; }
        public int? product_id { get; set; }
        public int? purchase_order_id { get; set; }

        public Guid? customer_id { get; set; }
        [ForeignKey("customer_id")]
        public virtual CustomerModel customer { get; set; }       
        
        public int? vendor_id { get; set; }
        [ForeignKey("vendor_id")]
        public virtual VendorModel vendor { get; set; } 
        
        public Guid? sale_id { get; set; }
        [ForeignKey("sale_id")]
        public virtual SaleModel sale { get; set; }

   

        public string document_number { get; set; } = "";
        public string module { get; set; } = "";
        public string url { get; set; } = "";

        public decimal amount { get; set; } = 0;
        public decimal old_amount { get; set; } = 0;

        public int? user_id { get; set; }
        [ForeignKey("user_id")]
        public UserModel user { get; set; }

       
    }
}
