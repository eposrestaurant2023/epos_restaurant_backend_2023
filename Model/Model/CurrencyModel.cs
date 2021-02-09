using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{
    [Table("tbl_currency")]
    public class CurrencyModel    : KeyModel
    {

        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }


        [MaxLength(50)]
        public string currency_name_en { get; set; }

        [MaxLength(50)]
        public string currency_name_kh { get; set; }


        [MaxLength(50)]
        public string currency_format { get; set; }


        [Column(TypeName = "decimal(16,10)")]
        public decimal exchange_rate { get; set; }


        [Column(TypeName = "decimal(16,10)")]
        public decimal change_exchange_rate { get; set; }

        public bool is_main { get; set; }

        public bool status { get; set; } = true;
    }
}
