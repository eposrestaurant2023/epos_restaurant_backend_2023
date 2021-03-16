using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_outlet_station")]
    public class OutletStationModel
    {
        public Guid outlet_id { get; set; } 

        public Guid station_id { get; set; } 
    }
}
