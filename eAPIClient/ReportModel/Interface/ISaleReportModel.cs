
namespace ReportModel.Interface
{
    public interface ISaleReportModel
    {
        public string id { get; set; }
        public string sale_number { get; set; }
        public string waiting_number { get; set; }

        public string outlet_id { get; set; }
        public string station_id { get; set; }

        public string station_name_en { get; set; }
        public string station_name_kh { get; set; }

        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }

        public string business_branch_id { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_code { get; set; }
        public string phone_number { get; set; }

        public int guest_cover { get; set; }        
        public int r_table_id { get; }
        public string table_name { get; set; } 
        public string working_day_id { get; set; }
        public string cashier_shift_id { get; set; }

        public string working_day_number { get; set; }
        public string cashier_shift_number { get; set; }

        public string document_number { get; set; }

        public string working_date { get; set; } 

        public bool is_partially_paid { get; set; }
        public bool is_print_invoice { get; set; } 

        public double total_quantity { get; set; }
        public double sub_total { get; set; }

        public string shift_name { get; set; }
        public bool is_sale_use_tax_1 { get; set; }
        public bool is_sale_use_tax_2 { get; set; }
        public bool is_sale_use_tax_3 { get; set; }


        //Discount
        public double discountable_amount { get; set; }
        public double sale_product_discount_amount { get; set; }
        public double sale_discount_value { get; set; } 
        public string sale_discount_type { get; set; }
        public double sale_discount_amount { get; set; }
        public double total_discount_amount { get; set; } 


        public string discount_code { get; set; }
        public string discount_note { get; set; }

        public double total_net_sale { get; set; }

        public double total_amount { get; set; }
        public double total_credit { get; set; }
        public double balance { get; set; }
        public double paid_amount { get; set; }
        public bool is_paid { get; set; }
        public double total_cost { get; set; }
        public double total_profit { get; set; } //net_sale - total_cost

        public double rounding_amount { get; set; } 

        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }

        //Tax 
        public double tax_1_rate { get; set; }
        public double tax_1_amount { get; set; }

        public double tax_2_rate { get; set; }
        public double tax_2_amount { get; set; }

        public double tax_3_rate { get; set; }
        public double tax_3_amount { get; set; }
        public double total_tax_amount { get; set; }


        //Other 

        public string currency_exchange_rate_data { get; set; }
        public string sale_note { get; set; }
        public int status_id { get; set; }
        //
        public bool is_closed { get; set; }
        public string closed_by { get; set; }
        public string closed_date { get; set; }

        public string sale_type { get; set; } 

        public bool is_synced { get; set; } 

        public string created_by { get; set; }
        public string created_date { get; set; }

        public bool is_deleted { get; set; }

        public string deleted_by { get; set; }
        public string deleted_date { get; set; }
        public string deleted_note { get; set; }

        public bool status { get; set; }

        public bool is_reprint_receipt { get; set; } 


        public bool is_park { get; set; }
        public bool is_redeem_park { get; set; }
        public string park_sale_id { get; set; }
        public string redeem_park_sale_id { get; set; }


        public string check_in_by { get; set; }
        public string check_in_date { get; set; }
        public string check_out_by { get; set; }
        public string check_out_date { get; set; }
        public bool is_checked_out { get; set; }
        public string kitchen_message_text { get; set; }
        public string sale_seat { get; set; }
        public string coupon_number { get; set; }
        public double coupon_voucher_amount { get; set; }


    } 

}
