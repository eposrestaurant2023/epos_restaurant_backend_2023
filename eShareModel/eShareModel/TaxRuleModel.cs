using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
    public class TaxRuleModel
    {

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
