using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    public class HourlyTimeChargeRateModel
    {
        public int  hour { get; set; }
        public int price_rule_id { get; set; }
        public int portion_id { get; set; }
        public bool is_break_point { get; set; }
    }
}
