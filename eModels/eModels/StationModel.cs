
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
        public bool is_already_config { get; set; } = false;

        public bool is_full_license { get; set; }
        [Column(TypeName = "date")]
        public DateTime expired_date { get; set; }

        public string tax_1_name { get; set; } = "Service Charge";
        public string tax_2_name { get; set; } = "P/L Tax";
        public string tax_3_name { get; set; } = "VAT";
        public decimal tax_1_taxable_rate { get; set; } = 1;
        public decimal tax_2_taxable_rate { get; set; } = 1;
        public decimal tax_3_taxable_rate { get; set; } = 1;

        public decimal tax_1_rate { get; set; } = 1; 
        public decimal tax_2_rate { get; set; } = 1;
        public decimal tax_3_rate { get; set; } = 1;


    }
}
