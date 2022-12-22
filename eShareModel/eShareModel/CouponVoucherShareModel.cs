using System;                      
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{

    public class CouponVoucherShareModel : CoreGUIDModel
    {
        public Guid registered_business_branch_id { get; set; }
        public string registered_business_branch_en { get; set; }
        public string registered_business_branch_kh { get; set; }

        public string coupon_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime registered_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime expiry_date { get; set; }

        public bool unlimited { get; set; } = false;

        public decimal total_balance { get; set; }
        public decimal total_refund_amount { get; set; }
        public bool is_synced { get; set; } = false;

    }

    public class CouponVoucherTransactionShareModel     : CoreGUIDModel
    {
        public string coupon_number { get; set; }
        public Guid coupon_voucher_id { get; set; }

        public string document_number { get; set; } = "New";

        public Guid business_branch_id { get; set; }
        public string business_branch_en { get; set; }
        public string business_branch_kh { get; set; }

        public Guid outlet_id { get; set; }
        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }

        public Guid station_id { get; set; }
        public string station_name_en { get; set; }
        public string station_name_kh { get; set; }

        public Guid cash_drawer_id { get; set; }
        public string cash_drawer_name { get; set; }

        public Guid working_day_id { get; set; }
        public string working_day_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime working_date { get; set; } = DateTime.Now;

        public Guid cashier_shift_id { get; set; }
        public string cashier_shift_number { get; set; }
        public string shift_name { get; set; }

       
        public decimal top_up_amount { get; set; }
        public decimal base_top_up_amount { get; set; }
        public decimal refund_amount { get; set; }
        public decimal current_balance { get; set; }  
        public decimal base_current_balance { get; set; }
        public bool is_used { get; set; } = false;



        //  payment
        ////currency 
        public int currency_id { get; set; }
        public string currency_name_en { get; set; }
        public string currency_name_kh { get; set; }
        public double exchange_rate { get; set; }
        public double change_exchange_rate { get; set; }
        public string symbol { get; set; }
        public bool is_prefix_symbol { get; set; }


        ////payment field
        public string payment_type_group { get; set; }
        public int payment_type_id { get; set; }
        public string payment_type_name_en { get; set; }
        public string payment_type_name_kh { get; set; }

        public bool is_synced { get; set; } = false;

    }   
}
