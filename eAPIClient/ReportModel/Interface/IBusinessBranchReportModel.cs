
namespace ReportModel.Interface
{
    public interface IBusinessBranchReportModel
    {
        public string business_branch_name_en { get; set; }
        public string business_branch_name_kh { get; set; }
        public string address_en { get; set; }
        public string address_kh { get; set; }

        public string phone_1 { get; set; }
        public string phone_2 { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string contact_name { get; set; }
    } 
}
