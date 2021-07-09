
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;
namespace eModels
{
    [Table("tbl_station")]
    public  class StationModel   : CoreGUIDModel
    {

        public StationModel()
        {

            extend_license_histories = new List<ExtendLicenseHistoryModel>();
        }


        public Guid outlet_id { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }



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
        [Column(TypeName = "date")]
        public DateTime? expired_date { get; set; } = DateTime.Now.AddMonths(1);
        public DateTime? full_license_date { get; set; } = DateTime.Now.AddMonths(1);
        public string full_license_by { get; set; }
        public bool is_full_license { get; set; }
        public string station_type { get; set; }
        public string hardware_detail { get; set; }
        public string note { get; set; }
        public List<ExtendLicenseHistoryModel> extend_license_histories { get; set; }


    }
}
