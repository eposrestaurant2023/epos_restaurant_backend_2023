﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingServiceReportModel
{
    public class SalePaymentModel
    {
        public string payment_type_name_en { get; set; }
        public string payment_type_name_kh { get; set; }
        public string format { get; set; }
        public double payment_amount { get; set; }
    }
    public class SalePaymentChangeModel
    { 
        public string name_en { get; set; } 
        public string name_kh { get; set; } 
        public string format { get; set; }
        public double change_amount { get; set; } = 0;
    }
}
