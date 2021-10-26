using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_cash_drawer_amount")]
    public class CashDrawerAmountModel: CashDrawerAmountShareModel
    {
        public Guid working_day_id { get; set; }
        [ForeignKey("working_day_id")]
        public WorkingDayModel working_day { get; set; }

    }
}
