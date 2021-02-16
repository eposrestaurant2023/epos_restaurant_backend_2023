using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
   public class CurrencyShareModel    : KeyModel
    {

        [MaxLength(50)]
        public string currency_name_en { get; set; }

        [MaxLength(50)]
        public string currency_name_kh { get; set; }


        [MaxLength(50)]
        public string currency_format { get; set; }


        [Column(TypeName = "decimal(19,10)")]
        public decimal exchange_rate { get; set; }


        [Column(TypeName = "decimal(19,10)")]
        public decimal change_exchange_rate { get; set; }

        public bool is_main { get; set; }

        public bool status { get; set; } = true;
    }
}
