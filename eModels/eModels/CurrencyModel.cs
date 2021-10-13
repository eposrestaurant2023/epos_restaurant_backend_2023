using eShareModel;                                    
using System.ComponentModel.DataAnnotations.Schema;    

namespace eModels
{
    [Table("tbl_currency")]
    public class CurrencyModel    : CurrencyShareModel
    {
        public string symbol { get; set; }
    }
}
