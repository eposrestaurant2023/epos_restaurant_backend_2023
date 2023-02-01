namespace ReportModel.Interface
{ 
    public interface ICloseWorkingDaySummaryReportModel
    {
        public string group{ get; set; }
        public string title{ get; set; }
        public string value{ get; set; }
        public int sort_order { get; set; }
    } 

}
