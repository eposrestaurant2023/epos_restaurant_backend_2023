
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eShareModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace eAPIClient.Models
{
    [Table("tbl_payment_type")]
    public class PaymentTypeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int currency_id { get; set; }
        [ForeignKey("currency_id")]
        public CurrencyModel currency { get; set; }

        private string _payment_type_name_en;
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
        public string payment_type_name_kh { get; set; }
        public string photo { get; set; }
        public int sort_order { get; set; }
        public string note { get; set; }
    }
}
