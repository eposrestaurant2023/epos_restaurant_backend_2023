    
using eShareModel;
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
 
   
}
