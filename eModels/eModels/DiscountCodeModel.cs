using eModels.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace eModels
{
    [Table("tbl_discount_code")]
    public class DiscountCodeModel    : CoreModel
    {
        [Required(ErrorMessage = "Please select a business branch.")]

        [GuidNotEmpty(ErrorMessage = "Please select a business branch.")]
        public Guid business_branch_id{ get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch{ get; set; }

        [MaxLength(50)]
        public string discount_label { get; set; }

        public decimal discount_value { get; set; }      
    }
}
