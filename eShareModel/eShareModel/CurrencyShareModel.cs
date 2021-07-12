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
        public bool is_base_exchange_currency { get; set; }
        public bool is_main { get; set; }
        public bool status { get; set; } = true;
        public string report_format { get; set; }
    }
}
