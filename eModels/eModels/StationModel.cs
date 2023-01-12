
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

        public Guid? cash_drawer_id { get; set; }
        public string cash_drawer_name { get; set; }


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

     
        private decimal _tax_1_taxable_rate = 1;
        public decimal tax_1_taxable_rate
        {
            get { 
                return _tax_1_taxable_rate * 100; 
            }
            set {

                if (value > 100) value = 100;
                _tax_1_taxable_rate = value / 100; 
            }
        }

        private decimal _tax_2_taxable_rate = 1;
        public decimal tax_2_taxable_rate
        {
            get
            { 
                return _tax_2_taxable_rate * 100;
            }
            set
            {
                if (value > 100) value = 100;
                _tax_2_taxable_rate = value / 100;
            }
        }

        private decimal _tax_3_taxable_rate = 1;
        public decimal tax_3_taxable_rate
        {
            get
            { 
                return _tax_3_taxable_rate * 100;
            }
            set
            {
                if (value > 100) value = 100;
                _tax_3_taxable_rate = value / 100;
            }
        }
          
        private decimal _tax_1_rate = 1;
        public decimal tax_1_rate
        {
            get
            { 
                return _tax_1_rate * 100;
            }
            set
            {
                if (value > 100) value = 100;
                _tax_1_rate = value / 100;
            }
        }

        private decimal _tax_2_rate = 1;
        public decimal tax_2_rate
        {
            get
            { 
                return _tax_2_rate * 100;
            }
            set
            {
                if (value > 100) value = 100;
                _tax_2_rate = value / 100;
            }
        }

        private decimal _tax_3_rate = 1;
        public decimal tax_3_rate
        {
            get
            { 
                return _tax_3_rate * 100;
            }
            set
            {
                if (value > 100) value = 100;
                _tax_3_rate = value / 100;
            }
        }


        public bool is_order_station { get; set; }
        public bool order_station_allow_payment { get; set; } = false;
        public bool is_terminal_pos { get; set; } = false;
    }
}
