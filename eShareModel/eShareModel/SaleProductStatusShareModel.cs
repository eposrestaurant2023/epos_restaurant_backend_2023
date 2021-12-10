using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
    public  class SaleProductStatusShareModel 
    {
        [Key,]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string status_name { get; set; }
        public string color { get; set; }
        public bool allow_send_to_printer { get; set; }

        public bool allow_append_quantity { get; set; }
        public bool allow_display_in_pos_order_list { get; set; }       
        public bool allow_void_or_cancel_order { get; set; }    
        public bool active_order { get; set; }      
        public int submited_status_id { get; set; }      
        public bool allow_send_to_printer_when_change_table { get; set; }     
        public bool allow_send_to_printer_when_merge_table { get; set; }
        public bool allow_send_to_printer_when_change_sale_type { get; set; }


        public string note { get; set; }

        public int? multiplier { get; set; }
    }
}
