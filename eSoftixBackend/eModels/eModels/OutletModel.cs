﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    public  class OutletModel : CoreModel
    {
       
        [Required(ErrorMessage = "Please select a business branch.")]
        public int business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }   


        private string _outlet_name_en;

        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(100)]
        public string outlet_name_en
        {
            get { return _outlet_name_en; }
            set { 
                _outlet_name_en = value;
                if(string.IsNullOrEmpty(outlet_name_kh))
                {
                    outlet_name_kh = value;
                }
            }
        }

        [MaxLength(100)]
        public string outlet_name_kh { get; set; }


     

    }
}