using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eModels
{
    [Table("tbl_customer")]
    public class CustomerModel : CoreGUIDModel
    {
        public CustomerModel()
        {
            customer_business_branchs = new List<CustomerBusinessBranchModel>();
        }

        public string customer_code { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string customer_code_name { get { return customer_code + "-" + customer_name_en; } }


        [Required(ErrorMessage = "Please select a customer group.")]
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }


        [Required(ErrorMessage = "Please select a customer group.")]
        public int customer_group_id { get; set; }
        [ForeignKey("customer_group_id")]
        public CustomerGroupModel customer_group { get; set; }

        private string _customer_name_en;
        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(50)]
        public string customer_name_en
        {
            get { return _customer_name_en; }
            set { _customer_name_en = value;
                if (string.IsNullOrEmpty(customer_name_kh))
                {
                    customer_name_kh = value;
                }
            }
        }
        [MaxLength(50)]
        public string customer_name_kh { get; set; }

        [MaxLength(10)]
        public string gender { get; set; }

        [MaxLength(50)]
        public string email { get; set; }

        [MaxLength(100)]
        public string address { get; set; }

        [MaxLength(50)]
        public string phone_1 { get; set; }

        [MaxLength(50)]
        public string phone_2 { get; set; }
        public string photo { get; set; }
      

        [MaxLength(50)]
        public string   nationality { get; set; }


        [Column(TypeName = "date")]
        public DateTime date_of_birth { get; set; }   = DateTime.Now.AddYears(-18);

        public string note { get; set; }


        public List<CustomerBusinessBranchModel> customer_business_branchs { get; set; }

    }
}
