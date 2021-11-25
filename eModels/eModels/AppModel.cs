using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eModels
{

    class AppModel
    {
    }
   
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }


    public class StoreProcedureResultModel
    {
        public string result { get; set; }
    }

    public class StoreProcedureResultDecimalModel
    {
        public decimal result { get; set; }
    }

    [Table("tbl_document_number")]
    public class DocumentNumberModel : KeyModel
    {
        public string document_name { get; set; }
        public string prefix { get; set; }
        public string format { get; set; }
        public int counter { get; set; }
        public string counter_digit { get; set; }
    }

    [Table("tbl_country")]
    public class CountryModel
    {
        [Key]
        public int id { get; set; }
        public string country_name { get; set; }
        public string country_note { get; set; }
    }


    [Table("tbl_module_view")]

    public class ModuleViewModel
    {
        [Key]
        public int id { get; set; }

        public string module_name { get; set; }
        public string title { get; set; }
        public string default_filters { get; set; }
        public string default_order_by { get; set; }
        public string default_order_by_type { get; set; }
        public string permission_option { get; set; } = "";
        public bool is_default { get; set; } = false;

        public int sort_order { get; set; } = 0;

        [JsonIgnore]
        [NotMapped]
        public virtual List<FilterModel> filters
        {
            get
            {
                return JsonSerializer.Deserialize<List<FilterModel>>(default_filters.Replace("'", "\""));
            }
        }

    }


    public class PagerModel
    {
        private int _current_page;

        public int current_page
        {
            get
            {
                if (_current_page <= 0)
                {
                    _current_page = 1;
                }
                return _current_page;
            }
            set
            {
                _current_page = value;
                if (value <= 0)
                {
                    _current_page = 1;
                }
            }
        }

        public int per_page { get; set; } = 25;
        public string order_by { get; set; } = "id";
        public string order_by_type { get; set; } = "desc";
        public bool show_logo { get; set; } = false;
    }

  


    public class LineChartModel
    {
        public LineChartModel()
        {
            data = new List<ChartDataModel>();
        }
        public string[] label { get; set; }

        public List<ChartDataModel> data { get; set; }
    }

    public class ChartDataModel
    {
        public ChartDataModel()
        {

        }
        public string label { get; set; }

        public string color { get; set; }


        public double[] data { get; set; }


    }

    public class BarChartModel
    {
        public BarChartModel()
        {
            data = new List<ChartDataModel>();
        }
        public string[] label { get; set; }

        public List<ChartDataModel> data { get; set; }
    }


    public static class BaseColor
    {
        public static string Default => "#7c7f84";
        public static string DefaultLight => "#e4e6eb";
        public static string Primary => "#0288d1";
        public static string PrimaryLight => "#03a9f4";
        public static string Secondary => "#f4f9ff";
        public static string Danger => "#ec2147";
        public static string Warning => "#f8b425";
        public static string HvoverChartBorder => "#0060ff";
        public static string ChartBorder => "#FFFFFF";
        public static string Success => "#02a499";
    }



    [Table("tbl_number")]
    public class NumberModel
    {
        public int number { get; set; }
    }

    public class ApiResponseModel
    {
        public ApiResponseModel()
        {
            customers = new List<CustomerModel>();
            vendors = new List<VendorModel>();
        }
        public string message { get; set; }

        public List<CustomerModel> customers { get; set; }
        public List<VendorModel> vendors { get; set; }
    }




    public class DashboardKPIModel
    {
        public decimal today_sale_amount { get; set; }
        public decimal today_sale_quatity { get; set; }
        public decimal mtd_sale_amount { get; set; }
        public decimal mtd_sale_quantity { get; set; }
        public decimal total_active_order { get; set; }
        public decimal total_closed_order { get; set; }
        public decimal total_order { get; set; }
        public decimal total_deleted_order { get; set; }
    }

    public class CustomerKPIModel
    {
        public decimal total_receivable { get; set; }
        public int mtd_visited { get; set; }
        public decimal mtd_sale_amount { get; set; }
        public int total_sale { get; set; }
    }

    public class ServerConfigModel
    {
        public string project_id { get; set; }
        public string server_id { get; set; }
        public string hardware_server_id { get; set; }

    }

    public class QuantityAdjustmentModel
    {
        
        public int product_id { get; set; }
      
        public Guid stock_location_id { get; set; }
        public decimal quantity { get; set; }
    }

    [Table("tbl_predefine_discount_code")]
    public class PredefineDiscountCodeModel
    {
        [Key]
        public int id { get; set; }
        public string discount_code { get; set; }
        public decimal discount_value { get; set; }

        public int sort_order { get; set; } = 0;
    }

    [Table("tbl_predefine_note")]
    public class PredefineNoteModel
    {
        [Key]
        public int id { get; set; }
        public int category_note_id { get; set; }
        public string note { get; set; }
         

        public int sort_order { get; set; } = 0;
    }


    [Table("tbl_test")]
    public class TestModel
    {
        [Key]
        public Guid id { get; set; }

        public string name { get; set; }
    }

    public class OptionModel
    {
        public string options { get; set; }
        public int role_id { get; set; }
    }

    public class ReceiptListModel
    {


        public string receipt_name { get; set; }
        public string description { get; set; }
        public string invoice_file_name { get; set; }
        public int number_of_invoice_copies { get; set; } = 1;
        public int number_of_receipt_copies { get; set; } = 1;
        public string receipt_file_name { get; set; }
        public int sort_order { get; set; } 
        public bool is_build_in { get; set; } 

    }


}
