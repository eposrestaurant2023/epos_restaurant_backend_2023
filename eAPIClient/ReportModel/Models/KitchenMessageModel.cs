
using ReportModel.Interface;

namespace ReportModel
{
   
    public class KitchenMessageModel : IKitchenMessageModel
    {
        public string sale_number { get; set; }
        public string table_name { get; set; }
        public string message_text { get; set; }
        public string printer_names { get; set; }
    }
}
