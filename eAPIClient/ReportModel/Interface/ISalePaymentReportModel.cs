
namespace ReportModel.Interface
{
    /// <summary>
    /// SalePaymentReportModel
    /// </summary>
    public interface ISalePaymentReportModel
    {
        public string payment_type_name_en { get; set; }
        public string payment_type_name_kh { get; set; }
        public string format { get; set; }
        public double payment_amount { get; set; }
        public double payment_fee { get; set; }
        public double payment_fee_amount { get; set; }
        public double base_payment_fee_amount { get; set; }
    } 


    /// <summary>
    /// SalePaymentChangeModel
    /// </summary>
    public interface ISalePaymentChangeModel
    { 
        public string name_en { get; set; } 
        public string name_kh { get; set; } 
        public string format { get; set; }
        public double change_amount { get; set; }
    } 
}
