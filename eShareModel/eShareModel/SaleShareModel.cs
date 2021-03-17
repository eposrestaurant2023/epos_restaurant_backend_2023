using System;                          
using System.ComponentModel.DataAnnotations.Schema;     

namespace eShareModel
{
    public class SaleShareModel : CoreGUIDModel
    {
        public string sale_number { get; set; }

        public Guid outlet_id { get; set; }

        public Guid business_branch_id { get; set; }
        public Guid? customer_id { get; set; }

        public int guest_cover { get; set; } = 0;
        public int table_id { get; set; }
        public string table_name { get; set; }
        public Guid working_day_id { get; set; }
        public Guid cashier_shift_id { get; set; }

        public string working_day_number { get; set; }
        public string cashier_shift_number { get; set; }

        public string document_number { get; set; } = "New";
        [Column(TypeName = "date")]
        public DateTime working_date { get; set; } = DateTime.Now;



       
        public bool is_partially_paid { get; set; }

        public decimal total_quantity { get; set; }
        public decimal sub_total { get; set; }


        //Discount
        public decimal discountable_amount { get; set; } 
        public decimal sale_product_discount_amount { get; set; }
        public string discount_type { get; set; } = "Percent"; //Percent and Amount;
        public decimal discount { get; set; } = 0;
        public decimal total_discount { get; set; }

        
        public decimal total_amount { get; set; }
        public decimal balance { get; set; }
        public decimal paid_amount { get; set; }
        public bool is_paid { get; set; }
        public decimal total_cost { get; set; }



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


        //Other 
        public string sale_note { get; set; }
        public int status_id { get; set; }
        //
        public bool? is_closed { get; set; }
        public string closed_by { get; set; }
        public DateTime? closed_date { get; set; }
    }
}
