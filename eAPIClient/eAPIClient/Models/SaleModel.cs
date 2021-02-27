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
            sale_products = new List<SaleProductModel>();
        }

        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }

        public List<PaymentModel> payments { get; set; }
        public List<SaleProductModel> sale_products { get; set; }

    }
}
