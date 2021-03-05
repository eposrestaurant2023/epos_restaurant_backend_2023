using System;
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

        [NotMapped]
        [JsonIgnore]
        public string customer_code_name { get { return customer_code + "-" + customer_name_en; } }

        [Required(ErrorMessage = "Please select a customer group.")]
        [Range(1,int.MaxValue,ErrorMessage = "Please select a customer group.")]
        public int customer_group_id { get; set; }
        [ForeignKey("customer_group_id")]
        public CustomerGroupModel customer_group { get; set; }

        public List<CustomerBusinessBranchModel> customer_business_branchs { get; set; }
        
        [NotMapped, JsonIgnore] 

        public string customer_business_brach_list
        {
            get { 
                if(customer_business_branchs != null && customer_business_branchs.Any())
                {
                    string business = "";
                    foreach (var b in customer_business_branchs) {
                        business += b.business_branch.business_branch_name_en + " / ";
                    }
                    
                    return business.Remove(business.LastIndexOf(" / ")).TrimEnd();
                }
                return "";
            }
        }

    }
}
