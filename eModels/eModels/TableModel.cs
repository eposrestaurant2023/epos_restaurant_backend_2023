using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eModels
{
    [Table("tbl_table")]
    public  class TableModel  : CoreModel
    {
        [Required(ErrorMessage = "Please select a table group.")]
        public int table_group_id { get; set; }
        [ForeignKey("table_group_id")]
        public TableGroupModel table_group { get; set; }
        
        public int? price_rule_id { get; set; }
        [ForeignKey("price_rule_id")]
        public PriceRuleModel price_rule { get; set; }

        [MaxLength(50)]
        public string table_name { get; set; }

        public double position_x_percent { get; set; }
        public double position_y_percent { get; set; }
        public double height { get; set; } = 60;
        public double width { get; set; } = 60;
 
        public int sort_order { get; set; }  
 
        public string shape { get; set; } = "Rectangle";



    }
}
