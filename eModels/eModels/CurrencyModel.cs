using eShareModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;    

namespace eModels
{
    [Table("tbl_currency")]
    public class CurrencyModel    : CurrencyShareModel
    {
        public CurrencyModel()
        {
           business_branch_currencies = new List<BusinessBranchCurrencyModel>();
        }
        public double default_exchange_rate { get; set; }
        public double default_change_exchange_rate { get; set; }

     public List<BusinessBranchCurrencyModel> business_branch_currencies { get; set; }

        public bool is_deleted { get; set; }
      


    }
}
