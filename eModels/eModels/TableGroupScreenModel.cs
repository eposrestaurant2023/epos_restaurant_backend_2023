using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_table_group_screen")]
    public  class TableGroupScreenModel 
    {
        public Guid outlet_id { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }

        public Guid station_id { get; set; }
        [ForeignKey("station_id")]
        public StationModel station { get; set; }


        public Guid table_group_id { get; set; }
        [ForeignKey("table_group_id")]
        public TableGroupModel table_group { get; set; }


        public int screen_width { get; set; } 
        public int screen_height { get; set; }
    }
}
