﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    [Table("tbl_customer")]
    public class CustomerModel : CustomerShareModel
    {
        public CustomerModel()
        {
            customer_business_branchs = new List<CustomerBusinessBranchModel>();
        }

       
      
      
        [ForeignKey("customer_group_id")]
        public CustomerGroupModel customer_group { get; set; }    
        public List<CustomerBusinessBranchModel> customer_business_branchs { get; set; }


        [NotMapped, JsonIgnore]
        public string customer_code_name
        {
            get
            {
                return customer_code + "-" + customer_name_en;
            }
        }

    }
}
