using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_unit")]
    public class UnitModel : CoreModel
    {
        public string unit_name { get; set; }
        public string type_name { get; set; }
        public decimal multiplier { get; set; }
        public int sort_order { get; set; }
        public bool is_defualt { get; set; }
        public bool is_built_in { get; set; }

        public int unit_category_id { get; set; }
        [ForeignKey("unit_category_id")]
        public UnitCategoryModel unit_category { get; set; }

    }
}
