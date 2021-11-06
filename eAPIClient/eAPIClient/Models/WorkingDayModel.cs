using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;       

namespace eAPIClient.Models
{
    [Table("tbl_working_day")]
    public class WorkingDayModel   : WorkingDayShareModel
    {
        WorkingDayModel()
        {
            cashier_shifts = new List<CashierShiftModel>();
            cash_drawer_amounts = new List<CashDrawerAmountModel>();
        }                                                
        
        
        public List<CashierShiftModel> cashier_shifts { get; set; }
        public List<CashDrawerAmountModel> cash_drawer_amounts { get; set; }



    }
    [Table("tbl_cashier_shift")]
    public class CashierShiftModel: CashierShiftShareModel
    { 
        [ForeignKey("working_day_id")]
        public  WorkingDayModel working_day { get; set; }



      
    }
}
