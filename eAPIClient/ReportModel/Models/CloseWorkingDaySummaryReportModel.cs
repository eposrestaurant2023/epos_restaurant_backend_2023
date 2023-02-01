using ReportModel.Interface;

namespace ReportModel
{ 
   
    public class CloseWorkingDaySummaryReportModel : ICloseWorkingDaySummaryReportModel
    {
        public string group { get; set; }
        public string title { get; set; }
        public string value { get; set; }
        public int sort_order { get; set; }
    }

}
