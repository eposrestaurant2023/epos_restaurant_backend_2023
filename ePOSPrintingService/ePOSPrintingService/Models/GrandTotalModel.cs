using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingServiceReportModel
{
    public class GrandTotalModel
    {
        public int id { get; set; }
        public double exchange_rate { get; set; }
        public double change_exchange_rate { get; set; }
        public string name_en { get; set; }
        public string name_kh { get; set; }
        public string format { get; set; }
        public string symbol { get; set; }
        public bool show_in_receipt { get; set; }
        public int sort_order { get; set; }
        public double total_amount { get; set; }
    }
}
