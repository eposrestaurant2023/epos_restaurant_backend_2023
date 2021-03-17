
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{       
    public class WorkingDayShareModel  : CoreGUIDModel
    {
        public Guid outlet_id { get; set; }
        public Guid closed_station_id { get; set; }
        public Guid opened_station_id { get; set; }

        [Column(TypeName ="date")]
        public DateTime working_date { get; set; }     
        public bool is_closed { get; set; }
        public string closed_by { get; set; }
        public DateTime? closed_date { get; set; }
        public string close_note { get; set; }
        public string open_note { get; set; }

        public string working_day_number { get; set; } = "";
    }
}
