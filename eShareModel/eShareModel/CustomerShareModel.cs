using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using eShareModel;

namespace eShareModel
{
    public class CustomerShareModel : CoreGUIDModel
    {
        public int customer_group_id { get; set; }

        public string customer_code { get; set; }

        private string _customer_name_en;
        public string customer_name_en
        {
            get { return _customer_name_en; }
            set { _customer_name_en = value;
                if (string.IsNullOrEmpty(customer_name_kh))
                {
                    customer_name_kh = value;
                }
            }
        }
       
        public string customer_name_kh { get; set; }


        public string gender { get; set; }


        public string email { get; set; }


        public string address { get; set; }


        public string phone_1 { get; set; }

        public string phone_2 { get; set; }
        public string photo { get; set; }
        public string   nationality { get; set; }


        [Column(TypeName = "date")]
        public DateTime date_of_birth { get; set; }   = DateTime.Now.AddYears(-18);

        public string note { get; set; }
    }
}
