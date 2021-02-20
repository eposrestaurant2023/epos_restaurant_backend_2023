using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    [Table("tbl_stock_take")]
    public class StockTakeModel : CoreModel
    {
        public StockTakeModel()
        {
            stock_take_products = new List<StockTakeProductModel>();
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
        public DateTime stock_take_date { get; set; } = DateTime.Now;
        public string reference_number { get; set; }
        public string term_conditions { get; set; }
        public string note { get; set; }
        public bool is_fulfilled { get; set; }

        private decimal _total_quantity;
        public decimal total_quantity
        {
            get
            {
                if (stock_take_products.Count > 0)
                {
                    _total_quantity = active_stock_take_products.Sum(r => r.quantity);
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
                if (stock_take_products.Count > 0)
                {
                    _sub_total = active_stock_take_products.Sum(r => r.sub_total);
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
                if (stock_take_products.Count > 0)
                {
                    _discountable_amount = active_stock_take_products.Where(r => r.discount == 0).Sum(r => r.sub_total);
                }
                return _discountable_amount;
            }
            set { _discountable_amount = value; }
        }
        private decimal _stock_take_product_discount_amount;
        public decimal stock_take_product_discount_amount
        {
            get
            {
                if (stock_take_products.Count > 0)
                {
                    _stock_take_product_discount_amount = active_stock_take_products.Sum(r => r.total_discount);
                }
                return _stock_take_product_discount_amount;
            }
            set { _stock_take_product_discount_amount = value; }
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
                if (active_stock_take_products.Count() > 0)
                {
                    _total_amount = sub_total - grand_total_discount;
                }
                return _total_amount;
            }
            set { _total_amount = value; }
        }

        public List<StockTakeProductModel> stock_take_products { get; set; }
 
        private decimal _grand_total_discount { get; set; }
        public decimal grand_total_discount
        {
            get
            {
                _grand_total_discount = total_discount + stock_take_product_discount_amount;
                return _grand_total_discount;
            }
            set {
                _grand_total_discount = value;
            }
            
        }

        [NotMapped, JsonIgnore]
        public List<StockTakeProductModel> active_stock_take_products
        {
            get
            {
                return stock_take_products.Where(r => r.is_deleted == false).ToList();
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
      
    }


}
