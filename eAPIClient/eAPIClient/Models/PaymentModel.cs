﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using eShareModel;

namespace eAPIClient.Models
{

    [Table("tbl_payment")]
    public class PaymentModel : PaymentShareModel
    {
        public int payment_type_id { get; set; }
        [ForeignKey("payment_type_id")]
        public PaymentTypeModel payment_type { get; set; }

        public Guid? sale_id { get; set; }
        [ForeignKey("sale_id")]
        public SaleModel sale { get; set; }

        [NotMapped, JsonIgnore]
        public bool is_add_note { get; set; }
    }
}
