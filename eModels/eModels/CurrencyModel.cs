using eShareModel;                                    
using System.ComponentModel.DataAnnotations.Schema;    

namespace eModels
{
    [Table("tbl_currency")]
    public class CurrencyModel    : CurrencyShareModel
    {
        public double default_exchange_rate { get; set; }
        public double default_change_exchange_rate { get; set; }
    }
}
