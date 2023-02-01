
using System;
namespace Reporting.Models
{
    public class PrintRequestModel
    {
        public string action { get; set; }
        public Guid sale_id { get; set; }
        public Guid id { get; set; }
        public string receipt_name { get; set; }
        public string copies { get; set; } = "1";
        public string printed_by { get; set; }
        public string language { get; set; } = "en";

        public bool is_reprint { get; set; } = false;
        public string data { get; set; }
        public string default_printer_name { get; set; } = "NONE";

    }
}
