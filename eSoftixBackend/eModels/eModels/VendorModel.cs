 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using eShareModel;
namespace eModels
{

    [Table("tbl_vendor")]
    public class VendorModel : CoreModel
    {
        [Required(ErrorMessage = "Please select vendor code.")]
        public string vendor_code { get; set; } = "New";
        [Required(ErrorMessage = "Please select vendor name.")]
        public string vendor_name { get; set; }
        public string photo { get; set; }
        public string company_name { get; set; }
        public string office_phone { get; set; }
        public string mobile_phone { get; set; }
        public string email_address { get; set; }
        public string address { get; set; }
        public string note { get; set; }
        public decimal total_payable { get; set; }
        public bool is_auto_generate_vendor_code { get; set; }

        [Required(ErrorMessage = "Please select a province.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a province.")]
        public int province_id { get; set; }
        [ForeignKey("province_id")]
        public ProvinceModel province { get; set; }

        [Required(ErrorMessage = "Please select a vendor group.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a vendor group.")]
        public int vendor_group_id { get; set; }
        [ForeignKey("vendor_group_id")]
        public VendorGroupModel vendor_group { get; set; }


        [NotMapped, JsonIgnore]
        public string vendor_display_name
        {
            get
            {
                return (string.IsNullOrEmpty(vendor_code) ? "" : (vendor_code + " - ")) + "" + vendor_name;
            }
        }
    }

    [Table("tbl_province")]
    public class ProvinceModel : KeyModel
    {
        public string province_name { get; set; }
    }

    [Table("tbl_vendor_group")]
    public class VendorGroupModel : CoreModel
    {

        public VendorGroupModel()
        {
            vendors = new List<VendorModel>();
        }

        private string _vendor_group_name_en;
        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(50)]
        public string vendor_group_name_en
        {
            get { return _vendor_group_name_en; }
            set
            {
                _vendor_group_name_en = value;
                if (string.IsNullOrEmpty(vendor_group_name_kh))
                {
                    vendor_group_name_kh = value;
                }
            }
        }

        [MaxLength(50)]
        public string vendor_group_name_kh { get; set; }


        public string note { get; set; }


        public List<VendorModel> vendors { get; set; }




    }



}
