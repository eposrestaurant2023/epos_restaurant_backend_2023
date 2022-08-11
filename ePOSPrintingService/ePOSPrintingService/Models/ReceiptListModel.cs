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


    public class TelegramAlertModel : TelegramModel
    {
        public List<TelegramActionModel> actions { get; set; }

    }
    public class TelegramActionModel
    {
        public string name { get; set; }
        public string title { get; set; }
        public string msg { get; set; }
        public bool allow_send { get; set; }
        public int sort { get; set; }
    }
    public class TelegramModel
    {
        public string chat_id { get; set; }
        public string token { get; set; }
    }

    public class TelegramMessageParamModel
    {
        public string document_number { get; set; }       
        public string printed_by { get; set; }
        public DateTime printed_date { get; set; } = DateTime.Now;
        public string outlet_name { get; set; }
        public string station_name { get; set; }
        public string shift_name { get; set; }
    }

}
