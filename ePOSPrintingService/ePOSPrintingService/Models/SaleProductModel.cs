﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingService.ReportDataModel
{
   public  class SaleProductModel
    {
        public int product_id { get; set; }
        public string product_name_en { get; set; }
        public string product_name_kh { get; set; }
        public string product_group_en { get; set; }
        public string product_group_kh { get; set; }
        public string sale_product_modifier_name { get; set; }
        public string portion_name { get; set; }
        public bool is_free { get; set; }
        public string sale_product_discount_type { get; set; }
        public decimal sale_product_discount_value { get; set; }
        public decimal sale_product_discount_amount { get; set; }
        public string note { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }
        public decimal sub_total { get; set; }
        public decimal net_sale { get; set; }
        public decimal total_amount { get; set; }
        public string product_category_kh { get; set; }
        public string product_category_en { get; set; }
    }
}
