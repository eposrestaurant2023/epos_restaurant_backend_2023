using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingServiceReportModel
{
   public class SaleModel
    {
        public Guid id { get; set; }
        public string sale_number { get; set; }
        public string waiting_number { get; set; }

        public Guid outlet_id { get; set; }
        public Guid station_id { get; set; }

        public string station_name_en { get; set; }
        public string station_name_kh { get; set; }

        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }

        public Guid business_branch_id { get; set; }
        public Guid? customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_code { get; set; }
        public string phone_number { get; set; }

        public int guest_cover { get; set; } = 0;
        public int? table_id { get; set; }
        public string table_name { get; set; } = "";
        public Guid working_day_id { get; set; }
        public Guid cashier_shift_id { get; set; }

        public string working_day_number { get; set; }
        public string cashier_shift_number { get; set; }

        public string document_number { get; set; } = "New";
 
        public DateTime working_date { get; set; } = DateTime.Now;


        public bool is_partially_paid { get; set; }
        public bool is_print_invoice { get; set; } = false;

        public decimal total_quantity { get; set; }
        public decimal sub_total { get; set; }

        public string shift_name { get; set; }
        public bool is_sale_use_tax_1 { get; set; }
        public bool is_sale_use_tax_2 { get; set; }
        public bool is_sale_use_tax_3 { get; set; }


        //Discount
        public decimal discountable_amount { get; set; }
        public decimal sale_product_discount_amount { get; set; }
        public decimal sale_discount_value { get; set; } = 0;
        public string sale_discount_type { get; set; } = "Percent"; //Percent and Amount;
        public decimal sale_discount_amount { get; set; }
        public decimal total_discount_amount { get; set; } //sale_product_discount_amount + sale_discount_amount// report must use this column


        public string discount_code { get; set; }
        public string discount_note { get; set; }

        public decimal total_net_sale { get; set; }

        public decimal total_amount { get; set; }
        public decimal total_credit { get; set; }
        public decimal balance { get; set; }
        public decimal paid_amount { get; set; }
        public bool is_paid { get; set; }
        public decimal total_cost { get; set; }
        public decimal total_profit { get; set; } //net_sale - total_cost

        public double rounding_amount { get; set; } = 0;

        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }

        //Tax 
        public decimal tax_1_rate { get; set; }
        public decimal tax_1_amount { get; set; }

        public decimal tax_2_rate { get; set; }
        public decimal tax_2_amount { get; set; }

        public decimal tax_3_rate { get; set; }
        public decimal tax_3_amount { get; set; }
        public decimal total_tax_amount { get; set; }


        //Other 

        public string currency_exchange_rate_data { get; set; }
        public string sale_note { get; set; }
        public int status_id { get; set; }
        //
        public bool? is_closed { get; set; }
        public string closed_by { get; set; }
        public DateTime? closed_date { get; set; }

        public string sale_type { get; set; } = "Dine in";

        public bool is_synced { get; set; } = false;

 
        public string created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;

        public bool is_deleted { get; set; } = false;
 
        public string deleted_by { get; set; }
        public DateTime? deleted_date { get; set; }
        public string deleted_note { get; set; }

        public bool status { get; set; } = true;
        
        public bool is_reprint_receipt { get; set; } = false;


        public bool is_park { get; set; }
        public bool is_redeem_park { get; set; }
        public Guid? park_sale_id { get; set; }
        public Guid? redeem_park_sale_id { get; set; }


        public string check_in_by { get; set; }
        public DateTime? check_in_date { get; set; }
        public string check_out_by { get; set; }
        public DateTime? check_out_date { get; set; }
        public bool? is_checked_out { get; set; }  
        public string kitchen_message_text { get; set; }
        public string sale_seat { get; set; }  

    }
    public class KitchenMessageModel
    {
        public string sale_number { get; set; }
        public string table_name { get; set; }
        public string message_text { get; set; }
        public string printer_names { get; set; }
    }
}
