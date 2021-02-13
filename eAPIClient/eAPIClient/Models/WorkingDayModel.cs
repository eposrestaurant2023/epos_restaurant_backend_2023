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
        }

        public int outlet_id { get; set; }
        public Guid business_branch_id { get; set; }

        public List<CashierShiftModel> cashier_shifts { get; set; }

    }
    [Table("tbl_cashier_shift")]
    public class CashierShiftModel: CashierShiftShareModel
    {
       
        public Guid working_day_id { get; set; }
        [ForeignKey("working_day_id")]
        public  WorkingDayModel working_day { get; set; }
        public string shift { get; set; }

    }
}
