
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
    public class CashierShiftShareModel  : CoreGUIDModel
    {
        public Guid outlet_id { get; set; }
        public Guid closed_station_id { get; set; }
        public Guid opened_station_id { get; set; }
        public string cashier_shift_number { get; set; } = "";
        public Guid working_day_id { get; set; }
        public bool is_closed { get; set; }          
        public string closed_by { get; set; }
        public DateTime? closed_date { get; set; }

        public decimal? open_amount { get; set; }
        public decimal? close_amount { get; set; }

        public string open_note { get; set; }
        public string close_note { get; set; }

        [Column(TypeName ="decimal(19,8)")]
        public decimal exchange_rate { get; set; }
        public string shift { get; set; }


    }
}
