using eShareModel;                                    
using System.ComponentModel.DataAnnotations.Schema;    

namespace eModels
{
    [Table("tbl_currency")]
    public class CurrencyModel    : CurrencyShareModel
    {
        public string symbol { get; set; }
        public bool is_prefix_symbol { get; set; }
        public bool is_show_in_bill { get; set; }
    }
}
