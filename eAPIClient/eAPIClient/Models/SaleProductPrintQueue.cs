
using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eAPIClient.Models
{
    [Table("tbl_sale_product_print_queue")]
    public class SaleProductPrintQueue 
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }

        public Guid sale_product_id { get; set; }
        [ForeignKey("sale_product_id")]
        public SaleProductModel sale_product { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string outlet_name { get; set; }
        public string station_name { get; set; }
        public string sale_number { get; set; }
        public int group_item_type_id { get; set; } = 1;
        public string printer_name { get; set; }
        public string printer_ip_address { get; set; }
        public int printer_port { get; set; }

        public string product_code { get; set; }
        public string product_name_en { get; set; }
        public string product_name_kh { get; set; }
        public string kitchen_group_name { get; set; }
        public int? kitchen_group_sort_order { get; set; }
        public string note { get; set; }
        public string unit { get; set; }
        public string portion_name { get; set; }

        public decimal quantity { get; set; }
        public decimal price { get; set; } = 0;

        public bool is_printed { get; set; } = false;  
        public string modifier_items { get; set; }

        public string sale_product_status_note { get; set; }

        public Guid session_id { get; set; }
    }
}
