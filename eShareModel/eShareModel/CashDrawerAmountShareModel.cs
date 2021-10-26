using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
   
    public class CashDrawerAmountShareModel:CoreGUIDModel
    {

        public Guid business_branch_id { get; set; }   
      
         
        public Guid? cashier_shift_id { get; set; }   
        public Guid outlet_id { get; set; }   
        public Guid cash_drawer_id { get; set; }
        public int currency_id { get; set; }
        public string currency_name { get; set; }
        public string format{ get; set; }

        public double exchange_rate { get; set; }

        public string   transaction_type_name { get; set; }

        public decimal amount { get; set; }
        public decimal base_currency_amount { get; set; }
        public string base_currency_format { get; set; }

        public int multiplier_value { get; set; } = 1;

        public string cash_deposit_to { get; set; }
        public string note { get; set; }


    }
}
