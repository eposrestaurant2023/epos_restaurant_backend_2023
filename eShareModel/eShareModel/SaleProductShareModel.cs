using System;
using System.Collections.Generic;

namespace eShareModel
{
    public class SaleProductShareModel :CoreGUIDModel
    {
       
        public Guid sale_id { get; set; }   
        public int product_id { get; set; }
        public string product_code { get; set; } = "";
        private string _product_name_en="";
        public string product_name_en
        {
            get { return _product_name_en; }
            set { 
                _product_name_en = value;
                if (string.IsNullOrEmpty(product_name_kh))
                {
                    product_name_kh = value;
                }
            }
        }
        public string product_name_kh { get; set; } = "";
        public bool is_inventory_product { get; set; } = false;
        public bool is_allow_discount { get; set; } = true;
        public bool is_allow_free { get; set; } = true;
        public int portion_id { get; set; }
        public string portion_name { get; set; }

        //
        public string kitchen_group_name { get; set; }
        public int kitchen_group_sort_order { get; set; } = 0;
        //
        public string unit { get; set; }
        public double multiplier { get; set; } = 1;
        public int status_id { get; set; } = 1;
        public string status_name { get; set; } = "New";

        public bool is_free { get; set; } = false;

        public decimal cost { get; set; }
        public decimal reqular_price { get; set; }
        public decimal price { get; set; }

        public decimal total_modifier_amount { get; set; }

        public decimal quantity { get; set; } 
        public decimal sub_total { get; set; }

        public decimal discount { get; set; }
        public string   discount_type { get; set; }
        public decimal total_discount { get; set; }
        public decimal total_amount { get; set; }
        public decimal total_revenue { get; set; }


        //Tax
        public decimal taxable_amount { get; set; }
        public decimal tax_1_rate { get; set; }
        public decimal tax_1_amount { get; set; }
        public decimal tax_1_taxable_amount { get; set; }

        public decimal tax_2_rate { get; set; }
        public decimal tax_2_amount { get; set; }
        public decimal tax_2_taxable_amount { get; set; }

        public decimal tax_3_rate { get; set; }
        public decimal tax_3_amount { get; set; }
        public decimal tax_3_taxable_amount { get; set; }

        public decimal total_tax_amount { get; set; }

        //


        //Other
        public string note { get; set; }
        public string deleted_note { get; set; }
        public string free_note { get; set; }
        public string discount_note { get; set; }
        public string discount_lable { get; set; }


       

    }

    public class SaleProductModifierShareModel : CoreGUIDModel
    {
        public Guid sale_product_id { get; set; }
        public string   modifier_name { get; set; }
        public int product_modifier_id { get; set; }
        public decimal  price { get; set; }
    }
}
