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
    [Table("tbl_production_product")]
    public class ProductionProductModel : CoreModel
    {
        public ProductionProductModel()
        {
            
        }

        public bool is_inventory_product { get; set; }
        public bool is_fulfilled { get; set; }
        public int production_id { get; set; }
        [ForeignKey("production_id")]
        public ProductionModel production { get; set; }

        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
           
    
        public string note { get; set; }
         
      
        private decimal _quantity = 1;
        public decimal quantity
        {
            get { return _quantity <= 0 ? 1 : _quantity; }
            set { _quantity = value; }
        }

        public decimal cost { get; set; }
        public decimal regular_cost { get; set; }

        public decimal grand_total { get; set; }

        public int product_portion_id { get; set; }
        [ForeignKey("product_portion_id")]
        public ProductPortionModel product_portion { get; set; }


        private decimal _sub_total;
        public decimal sub_total
        {
            get
            {
                _sub_total = quantity * cost;
                return _sub_total;
            }
            set { _sub_total = value; }
        }

 

        private decimal _total_amount;
        public decimal total_amount
        {
            get
            {
                _total_amount = sub_total;
                return _total_amount;
            }
            set { _total_amount = value; }
        }
        
        private decimal _multipler;
        public decimal multiplier
        {
            get { return _multipler; }
            set
            {

                if (value == 0)
                {
                    value = 1;
                }
 
                    cost = (cost / (_multipler==0?value:_multipler)) * value;
                    regular_cost = (regular_cost / (_multipler == 0 ? value : _multipler)) * value;
                
                    
                _multipler = value;

            }
        }
        public string unit { get; set; }

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
