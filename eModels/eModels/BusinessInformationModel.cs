using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{

    [Table("tbl_business_information")]
    public class BusinessInformationModel : KeyModel
    {
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string company_name { get; set; }

        [Required(ErrorMessage = "Field cannot be blank.")]
        public string company_name_kh { get; set; }
        public string company_logo { get; set; } = "placeholder.png";
        public string address { get; set; }
        public string mobile_phone { get; set; }
        public string office_phone { get; set; }
        public string email { get; set; }

        public string contact_name { get; set; }
        public string contact_phone_number { get; set; }
        public string contact_office_number { get; set; }
        public string contact_email { get; set; }
    }

    [Table("tbl_business_branch")]
    public class BusinessBranchModel   : CoreGUIDModel
    {
        public BusinessBranchModel()
        {
            outlets = new List<OutletModel>();                                                                                               
            customer_business_branchs = new List<CustomerBusinessBranchModel>();
            printers = new List<PrinterModel>();
            business_branch_payment_types = new List<BusinessBranchPaymentTypeModel>();
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
          
        public string note{ get; set; }

        public string logo { get; set; }
        public List<OutletModel>  outlets { get; set; }

                                                                                       
        public List<CustomerBusinessBranchModel> customer_business_branchs { get; set; }

        public List<PrinterModel> printers { get; set; }

        public List<BusinessBranchPaymentTypeModel> business_branch_payment_types { get; set; }
    }

    [Table("tbl_business_branch_payment_type")]
    public class BusinessBranchPaymentTypeModel
    {
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        public int payment_type_id { get; set; }
        [ForeignKey("payment_type_id")]
        public PaymentTypeModel payment_type { get; set; }
    }
}
