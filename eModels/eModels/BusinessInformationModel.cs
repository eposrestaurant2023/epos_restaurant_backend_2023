using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eModels
{

    [Table("tbl_business_information")]
    public class BusinessInformationModel : KeyGUIDModel
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
        public string color { get; set; }

    }

    [Table("tbl_business_branch")]
    public class BusinessBranchModel   : eShareModel.BusinessBranchShareModel
    {
        public BusinessBranchModel()
        {
            outlets = new List<OutletModel>();                                                                                               
            customer_business_branchs = new List<CustomerBusinessBranchModel>();
            printers = new List<PrinterModel>();
            business_branch_payment_types = new List<BusinessBranchPaymentTypeModel>();
            business_branch_prices = new List<BusinessBranchPriceRule>();  
            business_branch_settings = new List<BusinessBranchSettingModel>();
            cashier_notes = new List<NoteModel>();
            business_branch_roles = new List<BusinessBranchRoleModel>();
            stock_locations = new List<StockLocationModel>();
            discount_codes = new List<DiscountCodeModel>();

        }

        public List<DiscountCodeModel> discount_codes { get; set; }
        public List<StockLocationModel> stock_locations { get; set; }
         
        
   
        public List<OutletModel>  outlets { get; set; }

                                                                                       
        public List<CustomerBusinessBranchModel> customer_business_branchs { get; set; }

        public List<PrinterModel> printers { get; set; }

        public List<BusinessBranchPaymentTypeModel> business_branch_payment_types { get; set; }
        public List<BusinessBranchPriceRule> business_branch_prices { get; set; }   
        public List<BusinessBranchSettingModel> business_branch_settings { get; set; }
        public List<BusinessBranchRoleModel> business_branch_roles { get; set; }
        public List<NoteModel> cashier_notes { get; set; }
  
         
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
        public bool status { get; set; }

        [NotMapped,JsonIgnore]
        public bool is_change_status { get; set; }
        [NotMapped, JsonIgnore]
        public bool is_loading { get; set; }
    }
}
