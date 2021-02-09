using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
    [Table("tbl_working_day")]
    public class WorkingDayModel
    {
        WorkingDayModel()
        {
            cashier_shifts = new List<CashierShiftModel>();
        }
        public bool is_closed { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string closed_by { get; set; }
        public DateTime close_date { get; set; }
        public List<CashierShiftModel> cashier_shifts { get; set; }

    }
    [Table("tbl_cashier_shift")]
    public class CashierShiftModel
    {
        public bool is_closed { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string closed_by { get; set; }
        public DateTime close_date { get; set; }
        public decimal open_amount { get; set; }
        public decimal close_amount { get; set; }
        public int working_day_id { get; set; }
        [ForeignKey("working_day_id")]
        public virtual WorkingDayModel working_day { get; set; }
    }
}
