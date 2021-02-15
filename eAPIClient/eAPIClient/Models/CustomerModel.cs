﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_customer")]
    public class CustomerModel : CustomerShareModel
    {
        [NotMapped]
        [JsonIgnore]
        public string customer_code_name { get { return customer_code + "-" + customer_name_en; } }

        public int customer_group_id { get; set; }
        [ForeignKey("customer_group_id")]
        public CustomerGroupModel customer_group { get; set; }
        public Guid business_branch_id { get; set; }

    }
}
