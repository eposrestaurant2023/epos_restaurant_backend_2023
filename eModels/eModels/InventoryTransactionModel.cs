using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_inventory_transaction")]
    public class InventoryTransactionModel
    {
        [Key]
        public int id { get; set; }
        [Column(TypeName = "date")]
        public DateTime transaction_date { get; set; }

        public int inventory_transaction_type_id { get; set; }
        [ForeignKey("inventory_transaction_type_id")]
        public InventoryTransactionTypeModel inventory_transaction_type{ get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
        public int stock_location_id { get; set; }
        [ForeignKey("stock_location_id")]
        public StockLocationModel stock_location { get; set; }

        public decimal old_quantity { get; set; }
        public decimal quantity { get; set; }
        public decimal quantity_on_hand { get; set; }

        public int? sale_id { get; set; }
        public int? purchase_order_id { get; set; }
        public int? stock_transfer_id { get; set; }
        public int? stock_take_id { get; set; }

        public string url { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }

    }

    [Table("tbl_inventory_transaction_type")]
    public class InventoryTransactionTypeModel{
        public InventoryTransactionTypeModel()
        {
            inventory_transactions = new List<InventoryTransactionModel>();
        }
        [Key]
        public int id { get; set; }

        public string inventory_transaction_type_name { get; set; }

        public List<InventoryTransactionModel> inventory_transactions { get; set; }

    }
}
