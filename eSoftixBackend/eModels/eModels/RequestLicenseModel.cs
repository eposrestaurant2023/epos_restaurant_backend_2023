﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{

    [Table("tbl_request_license")]
    public class RequestLicenseModel:CoreModel
    {
    

        public Guid business_branch_id { get; set; }
         
        public int outlet_id { get; set; }
        public int station_id   { get; set; }

        [Column(TypeName = "date")]
        public DateTime expired_date { get; set; }

        public bool is_full_license { get; set; }

 


    }
}
