using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    [Table("tbl_sale")]
    public class SaleModel : SaleShareModel
    {
        public SaleModel()
        {
            payments = new List<PaymentModel>();
            histories = new List<HistoryModel>();
            sale_products = new List<SaleProductModel>();
        }
        public List<HistoryModel> histories { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }

        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }
        public List<PaymentModel> payments { get; set; }
        public List<SaleProductModel> sale_products { get; set; }

        [NotMapped, JsonIgnore]

        public List<SaleProductModel> active_sale_products
        {
            get { return sale_products.Where(r => r.is_deleted == false).ToList(); } 
        }


    }

}
