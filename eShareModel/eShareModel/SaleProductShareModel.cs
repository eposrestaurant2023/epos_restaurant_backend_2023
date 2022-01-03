using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eShareModel
{
    public class SaleProductShareModel :CoreGUIDModel
    {
       
        public Guid sale_id { get; set; }

        public Guid station_id { get; set; }
        public string station_name_en { get; set; }
        public string station_name_kh { get; set; }

        public Guid sale_order_id { get; set; }

        public int product_group_id { get; set; }
        public int product_category_id { get; set; }
        public string product_group_en { get; set; }
        public string product_group_kh { get; set; }
        public string product_category_en { get; set; }
        public string product_category_kh { get; set; }
        public bool is_sale_use_tax_1 { get; set; }
        public bool is_sale_use_tax_2 { get; set; }
        public bool is_sale_use_tax_3 { get; set; }

        public string seat_number { get; set; }
        


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

        [NotMapped, JsonIgnore]
        public string product_display_name
        {
            get
            {
                return (string.IsNullOrEmpty(product_code) ? "" : (product_code + " - ")) + "" + product_name_en;
            }
        }

        public bool is_free { get; set; } = false;

        public bool is_inventory_product { get; set; } = false;
        public bool is_allow_discount { get; set; } = true;
        public bool is_allow_change_price { get; set; } = true;
        public bool is_allow_free { get; set; } = true;
        public int portion_id { get; set; }
        public string portion_name { get; set; }

        //
        public string kitchen_group_name { get; set; }
        public int kitchen_group_sort_order { get; set; } = 0;
        //
        public string unit { get; set; }
        public decimal multiplier { get; set; } = 1;
        public int status_id { get; set; } = 1;
        public string status_name { get; set; } = "New";

        public decimal cost { get; set; }
        public decimal profit { get; set; }
        public decimal reqular_price { get; set; }
        public decimal price { get; set; }

        [NotMapped, JsonIgnore]
        public decimal total_price { get {
                return price + total_modifier_amount;
            } }

        public decimal total_modifier_amount { get; set; }

        public decimal quantity { get; set; } 
        public decimal sub_total { get; set; }

        public string sale_product_modifier_name { get; set; }


        //Discount
        public decimal sale_product_discount_value { get; set; }
        public string  sale_product_discount_type { get; set; }
        public decimal sale_product_discount_amount { get; set; } //show in receipt only
        public decimal sale_discount_value { get; set; } //percent only
        public decimal sale_discount_amount { get; set; }  
        public decimal total_discount_amount { get; set; }  //sale product discount amount + sale_discount_amount //report must use this column

        public decimal total_amount { get; set; }

        public decimal net_sale { get; set; } //net sale is sub toal - discount

 


        //Tax

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
        public string discount_note { get; set; }
        public string discount_code { get; set; }


        public string revenue_group_name { get; set; }


        public bool is_park { get; set; }
        public bool is_redeem_park { get; set; }
        public int? park_sale_product_id { get; set; }
        public int? redeem_sale_product_id { get; set; }
        public string park_note { get; set; }
        public DateTime? park_expired_date { get; set; }

    }

    public class SaleProductModifierShareModel : CoreGUIDModel
    {
        public Guid sale_product_id { get; set; }
        public string   modifier_name { get; set; }
       public Guid product_modifier_id { get; set; }
       public Guid? modifier_id { get; set; }
        public decimal  price { get; set; }
        public decimal  reqular_price { get; set; }

    }
}
