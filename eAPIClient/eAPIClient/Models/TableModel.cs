
using System.Collections.Generic;

namespace eAPIClient.Models
{                                    
    public class TableModel 
    {
        public int id { get; set; }
        public int table_group_id { get; set; }
        public string table_group_name { get; set; }
        public string table_name { get; set; }
        public int? price_rule_id { get; set; }
        public int sort_order { get; set; }
        public string shape { get; set; }
        public string sale_type { get; set; }
        public double position_x_percent { get; set; }
        public double position_y_percent { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public bool require_check_in { get; set; } = false;
    }

    public class TableGroupModel
    {
        public int id { get; set; }

        public string outlet_id { get; set; }
        public string table_group_name_en { get; set; }
        public string table_group_name_kh { get; set; }
        public string photo { get; set; }

        public List<TableModel> tables { get; set; }

    }
}
