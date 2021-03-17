using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    [Table("tbl_stock_transfer")]
    public class StockTransferModel : CoreModel
    {
        public StockTransferModel()
        {
            stock_transfer_products = new List<StockTransferProductModel>();
            histories = new List<HistoryModel>();
        }

        public List<HistoryModel> histories { get; set; }


        /// <summary>
        /// FROM LOCATION
        /// </summary>
        public Guid from_business_branch_id { get; set; }
        [ForeignKey("from_business_branch_id")]
        public BusinessBranchModel from_business_branch { get; set; }

        public Guid from_stock_location_id { get; set; }
        [ForeignKey("from_stock_location_id")]
        public StockLocationModel from_stock_location { get; set; }

        /// <summary>
        /// TO LOCATION
        /// </summary>
        public Guid to_business_branch_id { get; set; }
        [ForeignKey("to_business_branch_id")]
        public BusinessBranchModel to_business_branch { get; set; }

        public Guid to_stock_location_id { get; set; }
        [ForeignKey("to_stock_location_id")]
        public StockLocationModel to_stock_location { get; set; }

        public string document_number { get; set; } = "New";

        [Column(TypeName = "date")]
        public DateTime stock_transfer_date { get; set; } = DateTime.Now;
        public string reference_number { get; set; }
        public string term_conditions { get; set; }
        public string note { get; set; }
        public bool is_fulfilled { get; set; }

        private decimal _total_quantity;
        public decimal total_quantity
        {
            get
            {
                if (stock_transfer_products.Count > 0)
                {
                    _total_quantity = active_stock_transfer_products.Sum(r => r.quantity);
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
                if (stock_transfer_products.Count > 0)
                {
                    _sub_total = active_stock_transfer_products.Sum(r => r.sub_total);
                }
                return _sub_total;
            }
            set { _sub_total = value; }
        }

        private decimal _total_amount;
        public decimal total_amount
        {
            get
            {
                if (active_stock_transfer_products.Count() > 0)
                {
                    _total_amount = sub_total;
                }
                return _total_amount;
            }
            set { _total_amount = value; }
        }

        public List<StockTransferProductModel> stock_transfer_products { get; set; }

        [NotMapped, JsonIgnore]
        public List<StockTransferProductModel> active_stock_transfer_products
        {
            get
            {
                return stock_transfer_products.Where(r => r.is_deleted == false).ToList();
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
