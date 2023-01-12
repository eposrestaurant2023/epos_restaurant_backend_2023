using ReportModel.Interface;
using System;

namespace ReportModel
{
    
    public class CouponVoucherTransactionModel : ICouponVoucherTransactionModel
    {
        public string coupon_number { get; set; }
        public double total_balance { get; set; }
        public double total_refund_amount { get; set; }
        public string registered_date { get; set; }
        public string expiry_date { get; set; }
        public string document_number { get; set; }
        public double top_up_amount { get; set; }
        public double base_top_up_amount { get; set; }
        public double refund_amount { get; set; }
        public double current_balance { get; set; }
        public bool unlimited { get; set; }
        public double base_current_balance { get; set; }
        public string payment_type_group { get; set; }
        public string payment_type_name_en { get; set; }
        public string payment_type_name_kh { get; set; }
        public double exchange_rate { get; set; }
        public string currency_format { get; set; }
        public string symbol { get; set; }
        public bool is_prefix_symbol { get; set; }

        public string created_date { get; set; }
        public string created_by { get; set; }
    }
}
