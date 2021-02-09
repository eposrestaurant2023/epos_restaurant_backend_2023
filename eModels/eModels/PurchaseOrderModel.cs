using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace eModels
{
    [Table("tbl_purchase_order")]
    public class PurchaseOrderModel : CoreModel
    {
        public PurchaseOrderModel()
        {
            purchase_order_products = new List<PurchaseOrderProductModel>();
            payments = new List<PaymentModel>();
            histories = new List<HistoryModel>();
        }

        public List<HistoryModel> histories { get; set; }

        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        public int stock_location_id { get; set; }
        [ForeignKey("stock_location_id")]
        public StockLocationModel stock_location { get; set; }

        public string document_number { get; set; } = "New";

        [Column(TypeName = "date")]
        public DateTime purchase_date { get; set; } = DateTime.Now;
        public int vendor_id { get; set; }
        [ForeignKey("vendor_id")]
        public VendorModel vendor { get; set; }
        public int? discount_user_id { get; set; }
        [ForeignKey("discount_user_id")]
        public UserModel discount_user { get; set; }
        public string vendor_note { get; set; }
        public string reference_number { get; set; }
        public string term_conditions { get; set; }
        public string purchase_order_note { get; set; }
        public bool is_partially_paid { get; set; }
        public bool is_fulfilled { get; set; }
        public bool is_over_due { get; set; }

        [Column(TypeName = "date")]
        public DateTime? due_date { get; set; }

        private decimal _total_quantity;
        public decimal total_quantity
        {
            get
            {
                if (purchase_order_products.Count > 0)
                {
                    _total_quantity = active_purchase_order_products.Sum(r => r.quantity);
                }
                return _total_quantity;
            }
            set { _total_quantity = value; }
        }
        private decimal _sub_total;
        public decimal sub_total
        {
            get
            {
                if (purchase_order_products.Count > 0)
                {
                    _sub_total = active_purchase_order_products.Sum(r => r.sub_total);
                }
                return _sub_total;
            }
            set { _sub_total = value; }
        }
        private decimal _discountable_amount;
        public decimal discountable_amount
        {
            get
            {
                if (purchase_order_products.Count > 0)
                {
                    _discountable_amount = active_purchase_order_products.Where(r => r.is_allow_discount == true && r.discount == 0).Sum(r => r.sub_total);
                }
                return _discountable_amount;
            }
            set { _discountable_amount = value; }
        }
        private decimal _po_product_discount_amount;
        public decimal po_product_discount_amount
        {
            get
            {
                if (purchase_order_products.Count > 0)
                {
                    _po_product_discount_amount = active_purchase_order_products.Sum(r => r.total_discount);
                }
                return _po_product_discount_amount;
            }
            set { _po_product_discount_amount = value; }
        }

        private string _discount_type = "Percent"; //Percent and Amount;

        public string discount_type
        {
            get { return _discount_type; }
            set
            {
                _discount_type = value;
                if (discount > discountable_amount && _discount_type != "Percent")
                {
                    _discount = discountable_amount;
                }
            }
        }

        private decimal _discount = 0;
        public decimal discount
        {
            get
            {

                return _discount;
            }
            set
            {
                if (value > discountable_amount && discount_type != "Percent")
                {
                    _discount = discountable_amount;
                }
                else
                {
                    _discount = value;
                }

            }
        }
        private decimal _total_discount;
        public decimal total_discount
        {
            get
            {
                if (discount_type == "Percent")
                {
                    _total_discount = discountable_amount * discount / 100;
                }
                else
                    _total_discount = discount;
                return _total_discount;
            }
            set { _total_discount = value; }
        }
        private decimal _total_amount;
        public decimal total_amount
        {
            get
            {
                if (active_purchase_order_products.Count() > 0)
                {
                    _total_amount = sub_total - grand_total_discount;
                }
                return _total_amount;
            }
            set { _total_amount = value; }
        }
        private decimal _balance;
        public decimal balance
        {
            get
            {
                _balance = total_amount - paid_amount;
                return _balance;
            }
            set { _balance = value; }
        }
        public decimal paid_amount { get; set; }
        public bool is_paid { get; set; }
        
        public List<PaymentModel> payments { get; set; }
        public List<PurchaseOrderProductModel> purchase_order_products { get; set; }

        [NotMapped, JsonIgnore]
        public List<PaymentModel> active_payments
        {
            get
            {
                return payments.Where(r => !r.is_deleted).ToList();
            }
        }
        private decimal _grand_total_discount { get; set; }
        public decimal grand_total_discount
        {
            get
            {
                _grand_total_discount = total_discount + po_product_discount_amount;
                return _grand_total_discount;
            }
            set {
                _grand_total_discount = value;
            }
            
        }


        [NotMapped, JsonIgnore]
        public List<PurchaseOrderProductModel> active_purchase_order_products
        {
            get
            {
                return purchase_order_products.Where(r => r.is_deleted == false).ToList();
            }
        }

        [NotMapped, JsonIgnore]
        public bool can_delete
        {
            get
            {
                //return paid_amount == 0 && !is_paid && !is_deleted && total_visited==0; 
                return !is_deleted && !is_fulfilled;
            }
        }
        [NotMapped, JsonIgnore]
        public bool can_restore
        {
            get
            {
                return is_deleted;
            }
        }

        [NotMapped, JsonIgnore]
        public bool can_edit
        {
            get
            {
                return !is_deleted;
            }
        }
      
    }


}
