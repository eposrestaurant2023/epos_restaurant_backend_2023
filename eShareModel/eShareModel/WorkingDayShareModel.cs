
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{       
    public class WorkingDayShareModel  : CoreGUIDModel
    {
        [Column(TypeName ="date")]
        public DateTime working_date { get; set; }     
        public bool is_closed { get; set; }
        public string closed_by { get; set; }
        public DateTime? closed_date { get; set; }
        public string close_note { get; set; }
        public string open_note { get; set; }
    }
}
