using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using eShareModel;

namespace eAPIClient.Models
{

    [Table("tbl_sale_payment")]
    public class PaymentModel : PaymentShareModel
    {
       
       
        [ForeignKey("sale_id")]
        public SaleModel sale { get; set; }

        [NotMapped, JsonIgnore]
        public bool is_add_note { get; set; }
    }
}
