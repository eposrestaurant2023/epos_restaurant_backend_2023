﻿using System;
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

        public int province_id { get; set; }
        [ForeignKey("province_id")]
        public ProvinceModel province { get; set; }
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
