using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
   
  public  class ExpenseShareModel : CoreGUIDModel
    {
        public ExpenseShareModel()
        {
          
        }
        public Guid business_branch_id { get; set; }
        public string business_branch_name { get; set; }
        public Guid? outlet_id { get; set; }
        public string outlet_name{ get; set; }
        
        public Guid? station_id { get; set; }

        public string station_name { get; set; }


        public Guid? working_day_id { get; set; }
        public string working_day_number { get; set; }
        
        public Guid? cashier_shift_id { get; set; }
        public string cashier_shift_number{ get; set; }

        public Guid? cash_drawer_id { get; set; }
        
        public string cash_drawer_name { get; set; }

        public string reference_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime expense_date { get; set; } = DateTime.Now;

        [MaxLength(150)]
        [Required(ErrorMessage = "Field can not be blank.")]
        public string expense_by { get; set; }

        public Guid expense_category_id { get; set; }
        public string   expense_category_name { get; set; }
        
        public Guid expense_item_id { get; set; }
        public string   expense_item_name { get; set; }

        public int paymen_type_id { get; set; }
        public string paymen_type { get; set; }
        public int currency_id { get; set; }
        public string currency_name { get; set; }
        public string currency_symbol{ get; set; }
        public string currency_format { get; set; }
        public decimal amount { get; set; }
        public double exchange_rate { get; set; }


        public decimal base_currency_amount{ get; set; }
        public string note { get; set; }

        public string deleted_note { get; set; }
        public bool is_synced { get; set; } = false;

    }
}
