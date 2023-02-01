using ReportModel.Interface; 

namespace ReportModel
{
 
    public class CouponVoucherReportModel : ICouponVoucherReportModel
    {
        public string coupon_number { get; set; }
        public string registered_date { get; set; }
        public string expiry_date { get; set; }
        public bool unlimited { get; set; }
        public double total_balance { get; set; }
    }

}
