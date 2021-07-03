using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
   

    public class StockLocationModel
    { 
        public string stock_location_id { get; set; }
         
        public string stock_location_nme { get; set; } 
        public bool is_default { get; set; }
    }

    public class BusinessBranchStockLocationModel
    {
        public string business_branch_id { get; set; }
        public List<StockLocationModel> stock_locations
        {
            get; set;
        }
    }
}
