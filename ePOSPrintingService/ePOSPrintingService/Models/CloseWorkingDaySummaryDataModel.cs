using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingServiceReportModel
{
    public class CloseWorkingDaySummaryDataModel
    {
        public string group{ get; set; }
        public string title{ get; set; }
        public string value{ get; set; }
        public int sort_order { get; set; }
    }

    public class CloseCashierShiftSummaryDataModel
    {
        public string group { get; set; }
        public string title { get; set; }
        public string value { get; set; }
        public int sort_order { get; set; }
    }

}
