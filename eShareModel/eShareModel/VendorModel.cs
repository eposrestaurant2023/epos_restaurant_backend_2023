using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{

    [Table("tbl_vendor")]
    public class VendorModel : CoreModel
    {
        [Required]
        public string vendor_code { get; set; }
        [Required]
        public string vendor_name { get; set; }
        public string photo { get; set; }
        public string company_name { get; set; }
        public string office_phone { get; set; }
        public string mobile_phone { get; set; }
        public string email_address { get; set; }
        public string address { get; set; }
        public string note { get; set; }
        public decimal total_payable { get; set; }
       
    }
}
