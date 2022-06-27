using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingServiceReportModel
{
    public class DynamicDataModel
    {
        public string transaction_type { get; set; }
        public string id { get; set; }

        public string sale_data { get; set; }
        public string sale_product_data { get; set; }
        public string grand_total_data { get; set; }
        public string setting_data { get; set; }
        public string sale_payment_data { get; set; }
        public string sale_payment_change_data { get; set; }

        public string working_day_info { get; set; }
        public string working_day_data { get; set; }
        public string deleted_sale_data { get; set; }
        public string foc_sale_product_data { get; set; }
        public string cashier_shift_info { get; set; }
        public string cashier_shift_data { get; set; }



        //translate report property
        public string close_cashier_shift_report { get; set; } 
        public string close_cashier_shift_summary_report { get; set; } 
        

        public string working_day_no{ get; set; }
        public string shift_information{ get; set; }
        public string shift_no{ get; set; }
        public string sale_transaction{ get; set; }
        public string receipt_no{ get; set; }
        public string tbl_no{ get; set; }
        public string time{ get; set; }
        public string qty{ get; set; }
        public string amt{ get; set; }
        public string by { get; set; }

        public string close_working_day_summary_report { get; set; }
        public string working_day_information { get; set; }
        public string branch { get; set; }
        public string outlet { get; set; }
        public string cash_drawer_name { get; set; }
        public string status { get; set; }
        public string opened_date { get; set; }
        public string opened_by { get; set; }
        public string closed_date { get; set; }
        public string closed_by { get; set; }
        public string printed_by { get; set; }
        public string printed_on { get; set; }

        //new translate
        public string sale_products { get; set; }
        public string sale_product { get; set; }
        public string amount { get; set; }
        public string total { get; set; }
        public string grand_total { get; set; }
        public string product_name { get; set; }
        public string summary_by_revenue_group{ get; set; }
        public string revenue_group{ get; set; }
        public string foc_sale_product{ get; set; }
        public string free_sale_product{ get; set; }
        public string total_quantity{ get; set; }
        public string sub_total{ get; set; }
        public string item_discount { get; set; }
        public string sale_discount { get; set; }



    }


    public class TranslateCacheModel
    {
        public string language_code { get; set; }
        public List<DynamicDataModel> translate_data { get; set; }
    }
}
