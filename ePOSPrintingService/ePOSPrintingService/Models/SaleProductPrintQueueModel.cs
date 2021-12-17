
using System;

namespace ePOSPrintingServiceReportModel
{
    public class SaleProductPrintQueueModel
    {
        public string id { get; set; }
        public string sale_product_id { get; set; }
        public string sale_number { get; set; }
        public string product_code { get; set; }
        public string product_name_en { get; set; }
        public string product_name_kh { get; set; }
        public object kitchen_group_name { get; set; }
        public string kitchen_group_sort_order { get; set; }
        public object note { get; set; }
        public string unit { get; set; }
        public string portion_name { get; set; }
        public double quantity { get; set; }
        public double total_quantity { get; set; }
        public double price { get; set; }
        public bool is_printed { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modifier_items { get; set; }
        public string outlet_name { get; set; }
        public string printer_ip_address { get; set; }
        public string printer_name { get; set; }
        public int printer_port { get; set; }
        public string station_name { get; set; }
        public string sale_product_status_note { get; set; }
        public string table_name { get; set; }
        public string old_table_name{ get; set; }
        public int group_item_type_id { get; set; }
        public bool is_free { get; set; }
    }
}
