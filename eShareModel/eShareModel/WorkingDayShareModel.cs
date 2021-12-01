
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{       
    public class WorkingDayShareModel  : CoreGUIDModel
    {
        public Guid business_branch_id { get; set; }
        public Guid outlet_id { get; set; }
        public Guid? closed_station_id { get; set; }
        public Guid opened_station_id { get; set; }
        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }
        public string business_branch_name_kh { get; set; }
        public string business_branch_name_en { get; set; }
        public string closed_station_name_en { get; set; }
        public string closed_station_name_kh { get; set; }

        public string opended_station_name_en { get; set; }
        public string opended_station_name_kh { get; set; }

        [Column(TypeName ="date")]
        public DateTime working_date { get; set; }
        public bool is_closed { get; set; } = false;
        public string closed_by { get; set; }
        public DateTime? closed_date { get; set; } = DateTime.Now;
        public string close_note { get; set; }
        public string open_note { get; set; }
        public string working_day_number { get; set; } = "";

        public Guid cash_drawer_id { get; set; }
        public string cash_drawer_name { get; set; }

        public int total_cashier_shifts { get; set; } = 0;
        public bool is_synced { get; set; } = false;
        

    }
}
