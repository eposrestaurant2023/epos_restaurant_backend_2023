using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    public class StateModel
    {
        public StateModel()
        {
            filters = new List<FilterModel>();
            pager = new PagerModel();

        }
        public PagerModel pager { get; set; }
        public string api_url { get; set; }
        public List<FilterModel> filters { get; set; }
        public FilterModel filter { get; set; }
        public string page_title { get; set; } = "";

        public DateRangeModel date_range { get; set; } = new DateRangeModel(3);


        private CustomerGroupModel _customer_group = new CustomerGroupModel();

        public CustomerGroupModel customer_group
        {
            get { return _customer_group; }
            set
            {
                _customer_group = value;
                if (value == null)
                    customer_group = new CustomerGroupModel();
            }
        }

        private ProductCategoryModel _product_category = new ProductCategoryModel();

        public ProductCategoryModel product_category
        {
            get { return _product_category; }
            set
            {
                _product_category = value;
                if (value == null)
                    product_category = new ProductCategoryModel();
            }
        }


        public CustomerModel customer { get; set; } = new CustomerModel();
        public OutletModel outlet { get; set; } = new OutletModel();
    }

    public class DateRangeModel
    {
        public DateRangeModel()
        {

        }
        public int id { get; set; }
        public DateRangeModel(int SelectedValue = 3)
        {
            id = SelectedValue;
            if (SelectedValue == 0) //not select or today
            {
                start_date = DateTime.Now;
                end_date = DateTime.Now;
            }
            else if (SelectedValue == 1) // 2- this week
            {
                DayOfWeek day = DateTime.Now.DayOfWeek;
                int days = day - DayOfWeek.Monday;
                start_date = DateTime.Now.AddDays(-days);
                end_date = start_date.AddDays(6);
            }
            else if (SelectedValue == 2) // 3 = last week
            {
                start_date = DateTime.Now.AddDays(-(int)(DateTime.Now).DayOfWeek - 6);
                end_date = start_date.AddDays(6);
            }
            else if (SelectedValue == 3) //3 this month
            {
                start_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                end_date = start_date.AddMonths(1).AddDays(-1);
            }
            else if (SelectedValue == 4) //4 Month To Date
            {
                start_date = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
                end_date = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", DateTime.Now));
            }
            else if (SelectedValue == 5) // 5 last month
            {
                start_date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1);
                end_date = start_date.AddMonths(1).AddDays(-1);
            }
            else if (SelectedValue == 6) // 6 this year
            {
                start_date = new DateTime(DateTime.Now.Year, 1, 1);
                end_date = start_date.AddYears(1).AddDays(-1);
            }
            else if (SelectedValue == 7) // Year To Date
            {
                start_date = new DateTime(DateTime.Now.Year, 1, 1).AddYears(-1);
                end_date = start_date.AddYears(1).AddDays(-1);
            }
            else if (SelectedValue == 8)
            {
                start_date = DateTime.Now;
                end_date = DateTime.Now;
            }
            is_visible = true;
        }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        public bool is_visible { get; set; }


    }
    public class FilterModel
    {
        public FilterModel()
        {

        }
        public FilterModel(string _key)
        {
            key = _key;
            is_clear_all = true;
        }
        public FilterModel(string _key, string _remove_key)
        {
            key = _key;
            remove_key = _remove_key;
            is_clear_all = true;
        }

        [Key]
        public int id { get; set; }
        public string key { get; set; }

        private string _remove_key = "";
        public string remove_key
        {
            get
            {
                if (_remove_key == "")
                {
                    _remove_key = key;
                }
                return _remove_key;
            }
            set { _remove_key = value; }
        }
        public string value1 { get; set; } = "";
        public string value2 { get; set; } = "";
        public string value_clear_state { get; set; } = "";
        public string filter_operator { get; set; } = "eq";
        public string filter_join_operator { get; set; } = JoinOperator.and.ToString();
        public DateTime date1 { get; set; } = DateTime.Now;
        public DateTime date2 { get; set; } = DateTime.Now;
        public int? province_id { get; set; } = 0;
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string keyword { get; set; }
        public DateRangeModel date_range { get; set; } = new DateRangeModel(3);
        public bool is_clear_all { get; set; } = false;
        public string procedure_name { get; set; }
        public string procedure_parameter { get; set; }
        public string filter_title { get; set; } = "";
        public string filter_info_text { get; set; } = "";
        public int sort_order { get; set; } = 0;
        public int purchase_payment_id { get; set; }
        public int payment_id { get; set; }
        public int vendor_id { get; set; }
        public int sale_id { get; set; }
        public int customer_id { get; set; }
        public int position_id { get; set; }
        public int outlet_id { get; set; }
        public int purchase_id { get; set; }
        public bool will_remove { get; set; } = false;
        public string state_property_name { get; set; }
        public string sql_statement { get; set; }
    }

    public enum FilterOperator
    {
        eq, ne, gt, ge, lt, le, and, or, not, contains, suspicious_case, crime_case, notfilter
    }
    public enum JoinOperator
    {
        and, or
    }
    public class SelectPeriodOption
    {
        public SelectPeriodOption(int id)
        {
            this.id = id;
        }
        public SelectPeriodOption()
        {
        }
        public int id { get; set; }
        public string peroid_name { get; set; }
        public DateTime period_value { get; set; } = DateTime.Now.AddDays(1);
    }

}
