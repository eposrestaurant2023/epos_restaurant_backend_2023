using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
    

    public class BusinessBranchProductTaxConfigModel
    {
        public string business_branch_id { get; set; }
        public decimal tax_1_rate { get; set; }
        public decimal tax_2_rate { get; set; }
        public decimal tax_3_rate { get; set; }

    }
    public class ProductTaxConfigModel
    {
        public decimal tax_1_rate { get; set; } = -1; // sale tax
        public decimal tax_2_rate { get; set; } = -1;
        public decimal tax_3_rate { get; set; } = -1;
    }
}
