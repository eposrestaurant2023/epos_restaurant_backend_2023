using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
    [Table("tbl_shift")]
    public class ShiftModel : ShiftShareModel
    {
        public List<CashierShiftModel> cashier_shift { get; set; }
    }
}
