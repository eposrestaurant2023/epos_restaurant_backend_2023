using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAPIClient.Models
{

    [Table("tbl_history")]
    public class HistoryModel : KeyGUIDModel
    {
        public HistoryModel()
        {

        }

        public HistoryModel(string _title)
        {
            title = _title;

        }

        public Guid? working_day_id { get; set; }
        public Guid? cashier_shift_id { get; set; }

        public string title { get; set; }


        public string description { get; set; }

        public string note { get; set; } = "";

        [Column(TypeName = "date")]
        public DateTime? transaction_date { get; set; }

        public Guid? outlet_id { get; set; }

        public Guid? customer_id { get; set; }
        [ForeignKey("customer_id")]
       
     
        public int? product_id { get; set; }
   
          

        public Guid? sale_id { get; set; }
      
        

        public string document_number { get; set; } = "";
        public string module { get; set; } = "";
        public string url { get; set; } = "";

        public decimal amount { get; set; } = 0;
        public decimal old_amount { get; set; } = 0;

 
        public string created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;

        public bool is_synced { get; set; }

        public string station_name { get; set; }
        public string outlet_name { get; set; }
        public string business_branch_name { get; set; }

        public string table_name { get; set; }






    }
}
