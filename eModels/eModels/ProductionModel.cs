using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    [Table("tbl_production")]
    public class ProductionModel : CoreModel
    {
        public ProductionModel()
        {
            production_products = new List<ProductionProductModel>();
            histories = new List<HistoryModel>();
        }

        public List<HistoryModel> histories { get; set; }

        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        public Guid stock_location_id { get; set; }
        [ForeignKey("stock_location_id")]
        public StockLocationModel stock_location { get; set; }

        public string production_code { get; set; }

        [Column(TypeName = "date")]
        public DateTime production_date { get; set; } = DateTime.Now;
        public string reference_number { get; set; }
        public string term_conditions { get; set; }
        public string note { get; set; }
        public bool is_fulfilled { get; set; }

        private decimal _total_quantity;
        public decimal total_quantity
        {
            get
            {
                if (production_products.Count > 0)
                {
                    _total_quantity = active_production_products.Sum(r => r.quantity);
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
                if (production_products.Count > 0)
                {
                    _total_amount = active_production_products.Sum(r => r.sub_total);
                }
                return _total_amount;
            }
            set { _total_amount = value; }
        }

        public List<ProductionProductModel> production_products { get; set; }

        [NotMapped, JsonIgnore]
        public List<ProductionProductModel> active_production_products
        {
            get
            {
                return production_products.Where(r => r.is_deleted == false).ToList();
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
