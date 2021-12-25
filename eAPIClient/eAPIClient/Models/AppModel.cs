using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
    public class AppModel
    {
        [Key]
        public int id { get; set; }
    }
    public class KeyModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
    }


    public class KeyGUIDModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }
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



    public class CoreNoIdentityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [MaxLength(100)]
        public string created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;

        public bool is_deleted { get; set; } = false;
        [MaxLength(100)]
        public string deleted_by { get; set; }
        public DateTime? deleted_date { get; set; }
        public bool status { get; set; } = true;

        [NotMapped]
        [JsonIgnore]
        public bool is_change_status { get; set; } = false;

        [NotMapped]
        [JsonIgnore]
        public bool is_editing { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_deleting { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_restoring { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_saving { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_selected { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_loading { get; set; } = false;
    }



    public class GetOdataResponse
    {

        public GetOdataResponse() { }
        public GetOdataResponse(bool status)
        {
            IsSuccess = status;
        }
        public GetOdataResponse(bool status, int count, object content)
        {
            Count = count;
            IsSuccess = status;
            Content = content;
        }

        [JsonPropertyName("value")]
        public object Content { get; set; }
        [JsonPropertyName("@odata.count")] public int Count { get; set; } = 0;
        public bool IsSuccess { get; set; } = true;

    }

    public class GetResponse
    {

        public GetResponse() { }
        public GetResponse(bool status)
        {
            IsSuccess = status;
        }
        public GetResponse(bool status, object content, int _status_code = 200)
        {
            IsSuccess = status;
            Content = content;
            status_code = _status_code;

        }
        public object Content { get; set; }
        public bool IsSuccess { get; set; } = true;
        public int status_code { get; set; }


    }

    public class PostReponse
    {

        public PostReponse() { }
        public PostReponse(bool status)
        {
            IsSuccess = status;
        }

        public PostReponse(bool status, string _content, int _status_code = 200)
        {
            IsSuccess = status;
            Content = _content;
            status_code = _status_code;
        }

        public bool IsSuccess { get; set; } = true;
        public string Content { get; set; } = "";
        public string value { get; set; } = "";
        public int status_code { get; set; }

    }


    public class FilterModel
    {
        public FilterModel()
        {

        }

        public string procedure_name { get; set; }
        public string procedure_parameter { get; set; }

    }

    public class PrintRequestModel
    {
        public string action { get; set; }
        public Guid sale_id { get; set; }
        public Guid id { get; set; }
        public string receipt_name { get; set; }
        public int copies { get; set; }
        public string printed_by { get; set; }

    }

    public class SystemFeatureModel
    {
        public string feature_code { get; set; }
    } public class BusinessBranchModel
    {

        public Guid id { get; set; }
        public string business_branch_name_en { get; set; }
        public string business_branch_name_kh { get; set; }
        public string address_en { get; set; }
        public string address_kh { get; set; }
        public string phone_1 { get; set; }
        public string phone_2 { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string logo { get; set; }
        public string contact_name { get; set; }


    }



    public class OutletModel {

        public Guid id { get; set; }
        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }

        public List<StationModel> stations { get; set; }
    }

    public class StationModel {

        public Guid id { get; set; }
        public string station_name_en { get; set; }
        public string station_name_kh { get; set; }


    }

    public class CheckWorkingModel {
        public WorkingDayModel working_day { get; set; }
        public CashierShiftModel cashier_shift { get; set; }
    }

    public class CustomerBusinessBranchModel
    {
        public Guid customer_id { get; set; }
        public CustomerModel customer { get; set; }
         

        public Guid business_branch_id { get; set; }
      

        public bool is_synced { get; set; }
    }


    public class DynamicModel
    {
        public string transaction_type { get; set; }
        public string id { get; set; }

        public string sale_data { get; set; }
        public string sale_product_data { get; set; }
        public string grand_total_data { get; set; }
        public string setting_data { get; set; }
        public string sale_payment_data { get; set; }
        public string working_day_info { get; set; }
        public string working_day_data { get; set; }
        public string deleted_sale_data { get; set; }
        public string foc_sale_product_data { get; set; }
        public string cashier_shift_info { get; set; }
        public string cashier_shift_data  { get; set; }
    }


    public class SettingModel
    {
        
        public int id { get; set; }
        public string setting_value { get; set; }
    }

}
