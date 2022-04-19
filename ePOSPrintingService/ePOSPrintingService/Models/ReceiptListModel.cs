using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingService.Models
{
   public class ReceiptListModel
    {
        public string ReceiptName { get; set; }
        public string InvoiceFileName { get; set; }
        public string ReceiptFileName { get; set; }
        public short number_invoice_copies { get; set; } = 1;
        public short number_receipt_copies { get; set; } = 1;
        public decimal PageWidth { get; set; }
        public decimal PageHeight { get; set; }
        public decimal MarginTop { get; set; }
        public decimal MarginRight { get; set; }
        public decimal MarginBottom { get; set; }
        public decimal MarginLeft { get; set; }
    }
    public class TelegramSettingModel
    {
        public string token { get; set; }
        public string chat_id { get; set; }
        public string image_path { get; set; }  

       
}

}
