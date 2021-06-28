
using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_working_day")]
    public class WorkingDayModel : WorkingDayShareModel
    {
        public WorkingDayModel()
        {
            cashier_shifts = new List<CashierShiftModel>();
        }

        public Guid business_branch_id { get; set; }
        public List<CashierShiftModel> cashier_shifts { get; set; }

    }


    [Table("tbl_cashier_shift")]
    public class CashierShiftModel : CashierShiftShareModel
    {
        [ForeignKey("working_day_id")]
        public WorkingDayModel working_day { get; set; }
    }
}
