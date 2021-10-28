
using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_working_day")]
    public class WorkingDayModel : WorkingDayShareModel
    {
        public WorkingDayModel()
        {
            cashier_shifts = new List<CashierShiftModel>();
            cash_drawer_amounts = new List<CashDrawerAmountModel>();
        } 

       
        public List<CashierShiftModel> cashier_shifts { get; set; }
        public List<CashDrawerAmountModel> cash_drawer_amounts{ get; set; }

        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }
        [ForeignKey("opened_station_id")]
        public StationModel opened_station { get; set; }

        [ForeignKey("closed_station_id")]
        public StationModel closed_station { get; set; }


    }


    [Table("tbl_cashier_shift")]
    public class CashierShiftModel : CashierShiftShareModel
    {
        [ForeignKey("working_day_id")]
        public WorkingDayModel working_day { get; set; }

        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }
        
        [ForeignKey("opened_station_id")]
        public StationModel opened_station { get; set; }
        
        [ForeignKey("closed_station_id")]
        public StationModel closed_station { get; set; }
    }

    public class ListSummaryModel
    {
        public string group { get; set; }
        public int sort_order { get; set; }
        public string title { get; set; }
        public string value { get; set; }
    }

    public class CashierShiftSaleProductSummaryModel
    {
        public int product_id { get; set; }
        public int sort_order { get; set; }
        public string product_group_en { get; set; }
        public string product_category_en { get; set; }
        public string product_name_en { get; set; }
        public bool is_free { get; set; }
        public string portion_name { get; set; }
        public decimal quantity { get; set; }
        public decimal total_amount { get; set; }
        public string revenue_group_name { get; set; }
    }
}
