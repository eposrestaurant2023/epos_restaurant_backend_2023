using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_modifier_ingredient")]
    public class ModifierIngredientModel
    {
        public int modifier_id { get; set; }
        [ForeignKey("modifier_id")]
        public ModifierModel modifier { get; set; }

        public int ingredient_id { get; set; }
        [ForeignKey("ingredient_id")]
        public ProductModel ingredient { get; set; }
        public bool is_deleted { get; set; }
        public decimal quantity { get; set; } = 1;
        public decimal cost { get; set; }        
        public int unit_id { get; set; }
        [ForeignKey("unit_id")]
        public UnitModel unit { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int unit_categery_id {get;set;}

        private decimal _total_cost;
        public decimal total_cost
        {
            get
            {

                return quantity * cost;
            }
            set { _total_cost = value; }
        }
    }
}
