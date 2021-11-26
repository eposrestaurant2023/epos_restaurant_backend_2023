using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingService.ReportDataModel
{
    public class SettingModel
    {
        public string tax_1_name { get; set; }
        public string tax_2_name { get; set; }
        public string tax_3_name { get; set; }
        public string company_name_en { get; set; }
        public string company_name_kh { get; set; }
        public string address_en { get; set; }
        public string address_kh { get; set; }
        public string phone_1 { get; set; }
        public string phone_2 { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string wifi_password { get; set; }
        public string currency_format { get; set; }
        public string printed_by { get; set; }
    }
}
