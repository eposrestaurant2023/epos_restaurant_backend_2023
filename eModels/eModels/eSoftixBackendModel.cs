using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
 

namespace eSoftixBackend
{
    class eSoftixBackendModel
    {

    }
    public class ProjectModel: eShareModel.CoreGUIDModel
    {

        public string project_name { get; set; }
        public string server_id { get; set; }

        public Guid customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }

        public List<BusinessBranchModel> business_branches { get; set; }

    }
     

    public class BusinessBranchModel : eShareModel.CoreGUIDModel
    {

        public BusinessBranchModel()
        {
            outlets = new List<OutletModel>();
            stock_locations = new List<StockLocationModel>();
        }
        public string business_branch_name_en { get; set; }
        public string business_branch_name_kh { get; set; }

        public string address_en { get; set; }
        public string address_kh { get; set; }

        [MaxLength(50)]
        public string email { get; set; }

        [MaxLength(50)]
        public string phone_1 { get; set; }

        [MaxLength(50)]
        public string phone_2 { get; set; }

        [MaxLength(50)]
        public string website { get; set; }


        public string logo { get; set; }
        public string note { get; set; }
        public string color { get; set; }

        public List<OutletModel> outlets { get; set; }
        public List<StockLocationModel> stock_locations { get; set; }


    }

        public class CustomerModel : eShareModel.CoreGUIDModel
    {
          

            
            public string customer_code { get; set; } = "New";

            
            public string province { get; set; }
            public string customer_name_kh { get; set; }
            public string customer_name_en { get; set; }

            public string customer_code_name { get; set; }

            public string gender { get; set; }


            public string email { get; set; }


            public string address { get; set; }

    
            public string phone_1 { get; set; }

            public string phone_2 { get; set; }
            public string photo { get; set; }

            public string position { get; set; }
            public string company_name { get; set; }

            public string nationality { get; set; }

            public DateTime date_of_birth { get; set; } = DateTime.Now.AddYears(-18);

            public string note { get; set; }
            public string telegram { get; set; }
      

        }

    public class OutletModel : eShareModel.CoreGUIDModel
    {
        public OutletModel()
        {
            stations = new List<StationModel>();
        } 
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
         
        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }
       public List<StationModel> stations { get; set; }


    }


    public class StationModel : eShareModel.CoreGUIDModel
    {
        public Guid outlet_id { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }


         public string station_name_en { get; set; }
         public string station_name_kh { get; set; }
        public bool is_already_config { get; set; } = false;

    }

    public class StockLocationModel
    {
       
        public Guid id { get; set; }
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        public string stock_location_name { get; set; }
        public bool is_default { get; set; }
 
    }


}
