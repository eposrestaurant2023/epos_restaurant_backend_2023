using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_unit_category")]
    public class UnitCategoryModel
    {
        public UnitCategoryModel()
        {
            units = new List<UnitModel>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string category_name { get; set; }
        public int sort_order { get; set; }
        public decimal length { get; set; }
        public decimal weight { get; set; }
        public decimal volumes { get; set; }
        public string unit { get; set; }
        public bool is_built_in { get; set; }
        public List<UnitModel> units { get; set; }
    }
}
