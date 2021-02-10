
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_station")]
    public  class StationModel  : CoreNoIdentityModel
    {
        public int outlet_id { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }

        private string _station_name_en;
        
        public string station_name_en
        {
            get { return _station_name_en; }
            set { _station_name_en = value;
            if(string.IsNullOrEmpty(station_name_kh))
                {
                    station_name_kh = value;
                }
            }
        }
        public string station_name_kh { get; set; }
        public bool is_already_config { get; set; } = false;         

    }
}
