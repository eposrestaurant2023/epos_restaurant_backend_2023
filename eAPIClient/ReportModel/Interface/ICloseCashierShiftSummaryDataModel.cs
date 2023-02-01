namespace ReportModel.Interface
{
    public interface ICloseCashierShiftSummaryDataModel
    {
        public string group { get; set; }
        public string title { get; set; }
        public string value { get; set; }
        public int sort_order { get; set; }
    }
     

}
