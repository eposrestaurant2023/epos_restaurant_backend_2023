using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{

    [Table("tbl_history")]
    public class HistoryModel : CoreGUIDModel
    {
        public HistoryModel()
        {

        }

        public HistoryModel(string _title)
        {
            title = _title;

        }

        public string title { get; set; }


        public string description { get; set; }

        public string note { get; set; } = "";

        [Column(TypeName = "date")]
        public DateTime? transaction_date { get; set; }

        public Guid? outlet_id { get; set; }

        public Guid? customer_id { get; set; }
        [ForeignKey("customer_id")]
        public virtual CustomerModel customer { get; set; }

        public int? purchase_order_id { get; set; }
        [ForeignKey("purchase_order_id")]
        public virtual PurchaseOrderModel purchase_order { get; set; }

        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual ProductModel product { get; set; }

        public int? production_id { get; set; }
        [ForeignKey("production_id")]
        public virtual ProductionModel production { get; set; }



        public Guid? sale_payment_id { get; set; }
        [ForeignKey("sale_payment_id")]
        public virtual SalePaymentModel sale_payment { get; set; }

        public int? vendor_id { get; set; }
        [ForeignKey("vendor_id")]
        public virtual VendorModel vendor { get; set; }

        public Guid? sale_id { get; set; }
        [ForeignKey("sale_id")]
        public virtual SaleModel sale { get; set; }
        public int? stock_take_id { get; set; }
        [ForeignKey("stock_take_id")]
        public virtual StockTakeModel stock_take { get; set; }
        public int? stock_transfer_id { get; set; }
        [ForeignKey("stock_transfer_id")]
        public virtual StockTransferModel stock_transfer { get; set; }


        public Guid? inventory_check_id { get; set; }
        [ForeignKey("inventory_check_id")]
        public virtual InventoryCheckModel inventory_check { get; set; }


        public string document_number { get; set; } = "";
        public string module { get; set; } = "";
        public string url { get; set; } = "";

        public decimal amount { get; set; } = 0;
        public decimal old_amount { get; set; } = 0;

        public int? user_id { get; set; }
        [ForeignKey("user_id")]
        public UserModel user { get; set; }



        public Guid? modifier_id { get; set; }
        [ForeignKey("modifier_id")]
        public ModifierModel modifier { get; set; }

        public Guid? modifier_group_id { get; set; }
        [ForeignKey("modifier_group_id")]
        public ModifierGroupModel modifier_group { get; set; }

        public string station_name { get; set; }
        public string outlet_name { get; set; }
        public string business_branch_name { get; set; }

        public string table_name { get; set; }

    }
}
