using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_sale")]
    public class SaleModel : SaleShareModel
    {
        public SaleModel()
        {
            payments = new List<PaymentModel>();
        }
        public int outlet_id { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }
        public Guid? business_branch_id { get; set; }
        public Guid customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }
        public List<PaymentModel> payments { get; set; }
    }

}
