using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingServiceReportModel
{
  public  class CashierShiftModel
    {
        public string id { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public bool is_deleted { get; set; }
        public string deleted_by { get; set; }
        public DateTime? deleted_date { get; set; }
        public bool status { get; set; }
        public string outlet_id { get; set; }
        public string cashier_shift_number { get; set; }
        public string working_day_id { get; set; }
        public bool is_closed { get; set; }
        public string closed_by { get; set; }
        public DateTime? closed_date { get; set; }
        public double? open_amount { get; set; }
        public double? close_amount { get; set; }
        public string open_note { get; set; }
        public string close_note { get; set; }
        public string shift_name { get; set; }
        public string opened_station_id { get; set; }
        public string closed_station_id { get; set; }
        public string closed_station_name_en { get; set; }
        public string closed_station_name_kh { get; set; }
        public string opened_station_name_en { get; set; }
        public string opened_station_name_kh { get; set; }
        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }
        public string working_date { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
        public string cash_drawer_id { get; set; }
        public string cash_drawer_name { get; set; }
        public bool is_synced { get; set; }
        public string working_day_number { get; set; }
    }
}
