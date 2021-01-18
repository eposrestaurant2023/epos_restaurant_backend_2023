
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_payment_type")]
    public class PaymentTypeModel : CoreModel
    {
        [Required(ErrorMessage = "Please select a business branch.")]
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        [Required(ErrorMessage = "Please select a currency.")]
        public Guid currency_id { get; set; }
        [ForeignKey("currency_id")]
        public CurrencyModel currency { get; set; }

        private string _payment_type_name_en;
        [MaxLength(50)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string payment_type_name_en
        {
            get { return _payment_type_name_en; }
            set { _payment_type_name_en = value;   
                if (string.IsNullOrEmpty(payment_type_name_kh))
                {
                    payment_type_name_kh = value;
                }
            }
        }
        [MaxLength(50)]
        public string payment_type_name_kh { get; set; }

        public string image_name { get; set; }

    }
}
