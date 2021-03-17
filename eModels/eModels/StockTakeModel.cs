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

        public Guid stock_location_id { get; set; }
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
        private decimal _total_amount;
        public decimal total_amount
        {
            get
            {
                if (stock_take_products.Count > 0)
                {
                    _total_amount = active_stock_take_products.Sum(r => r.sub_total);
                }
                return _total_amount;
            }
            set { _total_amount = value; }
        }

        public List<StockTakeProductModel> stock_take_products { get; set; }

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
