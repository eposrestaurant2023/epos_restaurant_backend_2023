using System;
using System.Collections.Generic;               
using System.ComponentModel.DataAnnotations.Schema;       
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


        public Guid? business_branch_id { get; set; } 
        [ForeignKey("customer_group_id")]
        public CustomerGroupModel customer_group { get; set; }    
        public List<CustomerBusinessBranchModel> customer_business_branchs { get; set; }

        public List<CustomerCardModel> customer_cards { get; set; }

        [NotMapped, JsonIgnore]
        public string customer_code_name
        {
            get
            {
                return customer_code + "-" + customer_name_en;
            }
        }

    }

    public class YearlySaleRevenueModel
    {
        public string business_branch_id { get; set; }
        public string business_branch_name { get; set; }
        public string color { get; set; }
        public DateTime cal_date { get; set; }
        public decimal total_amount { get; set; }

    }
}
