using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShareModel;

namespace eSoftixBackend
{
    class eSoftixBackendModel
    {

    }
    public class ProjectModel:CoreGUIDModel
    {

        public string project_name { get; set; }
        public string server_id { get; set; }

        public Guid customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }

    }
     
        public class CustomerModel : CoreGUIDModel
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
   


}
