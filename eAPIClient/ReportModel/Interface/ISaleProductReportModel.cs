namespace ReportModel.Interface
{
   public  interface ISaleProductReportModel
    {
        public int product_category_id { get; set; }
        public int product_group_id { get; set; }
        public int product_id { get; set; }
        public string revenue_group_name { get; set; }
        public string product_name_en { get; set; }
        public string product_name_kh { get; set; }
        public string product_group_en { get; set; }
        public string product_group_kh { get; set; }
        public string sale_product_modifier_name { get; set; }
        public string portion_name { get; set; }
        public bool is_free { get; set; }
        public string sale_product_discount_type { get; set; }
        public double sale_product_discount_value { get; set; }
        public double sale_product_discount_amount { get; set; }
        public double sale_discount_amount { get; set; }
        public double total_discount_amount { get; set; }
        public string tax_1_name { get; set; }
        public double total_tax_1_amount { get; set; }
        public string tax_2_name { get; set; }
        public double total_tax_2_amount { get; set; }
        public string tax_3_name { get; set; }
        public double total_tax_3_amount { get; set; }
        public string note { get; set; }
        public double price { get; set; }
        public double quantity { get; set; }
        public double sub_total { get; set; }
        public double net_sale { get; set; }
        public double total_amount { get; set; }
        public string product_category_kh { get; set; }
        public string product_category_en { get; set; }

        public bool is_park { get; set; }
        public bool is_redeem_park { get; set; }
        public bool is_time_charge { get; set; }
        public double multiplier { get; set; }
        public string park_sale_product_id { get; set; }
        public string redeem_sale_product_id { get; set; }
        public string park_note { get; set; }
        public string park_expired_date { get; set; }
        public int sort_order { get; set; }

    } 

}
