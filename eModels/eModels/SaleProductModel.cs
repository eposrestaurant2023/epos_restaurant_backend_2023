using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eShareModel;

namespace eModels
{
    [Table("tbl_sale_product")]
    public class SaleProductModel : SaleProductShareModel
    {
        public SaleProductModel()
        {

        }

        public Guid sale_id { get; set; }
        [ForeignKey("sale_id")]
        public virtual SaleModel sale { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
 

        [Required(ErrorMessage = "Please select unit.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select unit.")]
        public int unit_id { get; set; } = 1;
        [ForeignKey("unit_id")]
        public UnitModel unit { get; set; }
       

        private decimal _multipler = 1;

        public decimal multiplier
        {
            get { return _multipler; }
            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                _multipler = value;

            }
        }

        private bool _is_add_note;
        [NotMapped, JsonIgnore]
        public bool is_add_note
        {
            get
            {
                return _is_add_note;
            }
            set { _is_add_note = value; }
        }


        [NotMapped, JsonIgnore]
        public bool is_can_delete { get; set; } = true;
    }
}
