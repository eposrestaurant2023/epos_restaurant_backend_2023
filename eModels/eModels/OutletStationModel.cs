using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_outlet_station")]
    public class OutletStationModel
    {
        public int outlet_id { get; set; } 

        public int station_id { get; set; } 
    }
}
