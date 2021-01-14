using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_business_branch")]
    public class BusinessBranchModel   : CoreModel
    {
        public BusinessBranchModel()
        {
            outlets = new List<OutletModel>();
            payment_type_business_branchs = new List<PaymentTypeBusinessBranchModel>();
            discount_code_business_branchs = new List<DiscountCodeBusinessBranchModel>();
        }

        private string _business_branch_name_en;

        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(100)]
        public string business_branch_name_en
        {
            get { return _business_branch_name_en; }
            set { 
                _business_branch_name_en = value;
                if(string.IsNullOrEmpty(business_branch_name_kh))
                {
                    business_branch_name_kh = value;
                }
            }
        }


        [MaxLength(100)]
        public string business_branch_name_kh { get; set; }

        public string address_en { get; set; }
        public string address_kh { get; set; }

        [MaxLength(50)]
        public string email { get; set; }

        [MaxLength(50)]
        public string phone_1 { get; set; }

        [MaxLength(50)]
        public string phone_2 { get; set; }

        [MaxLength(50)]
        public string website { get; set; }

        [MaxLength(50)]
        public string vatin_number { get; set; }

        public string logo { get; set; }



        public List<OutletModel>  outlets { get; set; }

        public List<PaymentTypeBusinessBranchModel> payment_type_business_branchs { get; set; }
        public List<DiscountCodeBusinessBranchModel> discount_code_business_branchs { get; set; }
    }
}
