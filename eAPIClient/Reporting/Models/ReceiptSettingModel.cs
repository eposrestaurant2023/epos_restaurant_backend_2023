
namespace Reporting.Models
{
    public class ReceiptSettingModel
    {
        public string receipt_name { get; set; } = string.Empty;
        public string invoice_file_name { get; set; }
        public string receipt_file_name { get; set; }
        public short invoice_copies { get; set; } = 1;
        public short receipt_copies { get; set; } = 1;
        public short  feed_papper { get; set; } =0; 
    }  
}
