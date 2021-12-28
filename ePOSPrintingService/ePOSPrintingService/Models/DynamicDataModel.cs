using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingService.Models
{
    public class DynamicDataModel
    {
        public string transaction_type { get; set; }
        public string id { get; set; }

        public string sale_data { get; set; }
        public string sale_product_data { get; set; }
        public string grand_total_data { get; set; }
        public string setting_data { get; set; }
        public string sale_payment_data { get; set; }
        public string sale_payment_change_data { get; set; }

        public string working_day_info { get; set; }
        public string working_day_data { get; set; }
        public string deleted_sale_data { get; set; }
        public string foc_sale_product_data { get; set; }
        public string cashier_shift_info { get; set; }
        public string cashier_shift_data { get; set; }

    }
}
