
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_station")]
    public  class StationModel   : CoreModel
    {
        public StationModel()
        {
            outlet_stations = new List<OutletStationModel>();
            table_group_screens = new List<TableGroupScreenModel>();
        }

        [Required(ErrorMessage = "Please select an outlet.")]
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }


        private string _station_name_en;
        
        [MaxLength(50)]
        [Required(ErrorMessage = "Field cannot be blank.")]
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
        [MaxLength(50)]
        public string station_name_kh { get; set; }


        public List<OutletStationModel> outlet_stations { get; set; }

        public List<TableGroupScreenModel> table_group_screens { get; set; }

    }
}
