using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace eShareModel
{
   public class BusinessBranchCurrencyShareModel
    {
        public Guid business_branch_id { get; set; }
        public int currency_id { get; set; }
        public decimal exchange_rate { get; set; }
        public decimal exchange_rate_input { get; set; }
        public decimal change_exchange_rate { get; set; }
        public decimal change_exchange_rate_input { get; set; }
    }
}
