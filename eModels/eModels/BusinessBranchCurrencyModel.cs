using eShareModel;                                    
using System.ComponentModel.DataAnnotations.Schema;    

namespace eModels
{
    [Table("tbl_business_branch_currency")]
    public class BusinessBranchCurrencyModel : BusinessBranchCurrencyShareModel
    {
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        [ForeignKey("currency_id")]
        public CurrencyModel currency { get; set; }
    }
}
