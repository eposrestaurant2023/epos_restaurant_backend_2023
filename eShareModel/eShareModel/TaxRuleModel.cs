using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
    public class TaxRuleModel
    {

        public string tax_1_name { get; set; } = "Service Charge";
        public string tax_2_name { get; set; } = "P/L Tax";
        public string tax_3_name { get; set; } = "VAT";

        public bool calc_tax_1_after_adding_discount { get; set; } = true;

        public bool calc_tax_2_after_adding_discount { get; set; } = true;
        public bool calc_tax_2_after_adding_tax_1 { get; set; } = true;
        public bool calc_tax_2_taxable_rate_after_adding_tax_1 { get; set; } = true;

        public bool calc_tax_3_taxable_rate_after_adding_tax_1 { get; set; } = false;
        public bool calc_tax_3_taxable_rate_after_adding_tax_2 { get; set; } = false;
        public bool calc_tax_3_after_adding_discount { get; set; } = true;
        public bool calc_tax_3_after_adding_tax_1 { get; set; } = true;
        public bool calc_tax_3_after_adding_tax_2 { get; set; } = true;
        
    }


}
