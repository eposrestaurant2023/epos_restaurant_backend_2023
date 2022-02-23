using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eModels;
using eShareModel;

namespace eModels
{
    [Table("tbl_inventory_check")]
    public class InventoryCheckModel : CoreGUIDModel
    {
        public InventoryCheckModel()
        {
            histories = new List<HistoryModel>();
            inventory_check_products = new List<InventoryCheckProductModel>();
        }
        public string document_number { get; set; } = "New";
        public string reference_number { get; set; } = "";
        public DateTime? start_date { get; set; }
        public DateTime end_date { get; set; } = DateTime.Now;

        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        public Guid stock_location_id { get; set; }
        [ForeignKey("stock_location_id")]
        public StockLocationModel stock_location { get; set; }
        public string inventory_check_type { get; set; } = "Full"; //Full, Partial
        public decimal total_cost { get; set; }

        public bool is_fulfilled { get; set; }

        public string note { get; set; }

        public string product_categories { get; set; }

        public List<HistoryModel> histories { get; set; }
        public List<InventoryCheckProductModel> inventory_check_products { get; set; }


    }
    [Table("tbl_inventory_check_product")]
    public class InventoryCheckProductModel 
    {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }
        public Guid inventory_check_id { get; set; }
        [ForeignKey("inventory_check_id")]
        public InventoryCheckModel inventory_check { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
        public int unit_id { get; set; }
        public decimal multiplier { get; set; }
        public decimal cost { get; set; }
        public DateTime? last_inventory_date { get; set; }
        public decimal initial_quantity { get; set; }
        public decimal receive_quantity { get; set; }
        public decimal consume_quantity { get; set; }
        public decimal expected_quantity { get; set; }
        public decimal actual_quantity { get; set; }

        private decimal _diference_quantity;

        public decimal diference_quantity
        {
            get {
                _diference_quantity =  actual_quantity - expected_quantity;
                return _diference_quantity; 
            }
            set { _diference_quantity = value; }
        }


        public decimal diference_amount { get; set; }
        public string note { get; set; }

        public string product_group_name { get; set; }
        public string product_category_name { get; set; }
        public string product_name { get; set; }
        public string unit_name { get; set; }

        public DateTime inventory_check_date { get; set; }



    }

}
