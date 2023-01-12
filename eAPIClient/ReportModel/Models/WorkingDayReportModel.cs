﻿using ReportModel.Interface;
using System;                      

namespace ReportModel
{ 
    public class WorkingDayReportModel : IWorkingDayReportModel
    {
        public string id { get; set; }
        public string business_branch_id { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public bool is_deleted { get; set; }
        public string deleted_by { get; set; }
        public string deleted_date { get; set; }
        public bool status { get; set; }
        public string outlet_id { get; set; }
        public string working_date { get; set; }
        public bool is_closed { get; set; }
        public string closed_by { get; set; }
        public string closed_date { get; set; }
        public string close_note { get; set; }
        public string open_note { get; set; }
        public string working_day_number { get; set; }
        public string opened_station_id { get; set; }
        public string closed_station_id { get; set; }
        public string closed_station_name_en { get; set; }
        public string closed_station_name_kh { get; set; }
        public string opended_station_name_en { get; set; }
        public string opended_station_name_kh { get; set; }
        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }
        public string cash_drawer_id { get; set; }
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string cash_drawer_name { get; set; }
        public string business_branch_name_en { get; set; }
        public string business_branch_name_kh { get; set; }
        public bool is_synced { get; set; }
    }

}
