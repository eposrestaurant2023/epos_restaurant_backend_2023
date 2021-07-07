
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;
namespace eModels
{
    [Table("tbl_extend_liscense_history")]
    public  class ExtendLicenseHistoryModel : CoreNoDeleted
    {


        public Guid station_id { get; set; }
        [ForeignKey("station_id")]
        public StationModel station { get; set; }

        [Required]

        public DateTime? extend_date { get; set; } = DateTime.Now.AddMonths(1);

        public string note { get; set; }


    }
}
