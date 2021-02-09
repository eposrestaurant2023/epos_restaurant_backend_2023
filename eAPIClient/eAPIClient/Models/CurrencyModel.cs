using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eAPIClient.Models
{
    [Table("tbl_currency")]
    public class CurrencyModel : KeyModel
    {
        public Guid business_branch_id { get; set; }

        public string currency_name_en { get; set; }

        public string currency_name_kh { get; set; }


        public string currency_format { get; set; }


        [Column(TypeName = "decimal(19,8)")]
        public decimal exchange_rate { get; set; }


        [Column(TypeName = "decimal(19,8)")]
        public decimal change_exchange_rate { get; set; }

        public bool is_main { get; set; }

        public bool status { get; set; } = true;
    }
}
