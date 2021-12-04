using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_sale")]
    public class SaleModel : SaleShareModel
    {
        public SaleModel()
        {
            sale_payments = new List<SalePaymentModel>();
            sale_products = new List<SaleProductModel>();
        }

        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }

        [ForeignKey("status_id")]
        public SaleStatusModel  sale_status { get; set; }


        [ForeignKey("working_day_id")]
        public WorkingDayModel working_day { get; set; }


        [ForeignKey("cashier_shift_id")]
        public CashierShiftModel cashier_shift { get; set; }

        public List<SalePaymentModel> sale_payments { get; set; }
        public List<SaleProductModel> sale_products { get; set; }

        public String payment_data { get; set; }

     

    }
}
