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
        [Range(1,int.MaxValue,ErrorMessage ="Please select a customer group.")]
        public int customer_group_id { get; set; }
        public string customer_group_name { get; set; }

        public string customer_code { get; set; }

        private string _customer_name_en;
        [MaxLength(150)]
        [Required(ErrorMessage ="Field can not be blank.")]
        public string customer_name_en 
        {
            get { return _customer_name_en; }
            set
            {
                _customer_name_en = value;
                if (string.IsNullOrEmpty(customer_name_kh))
                {
                    customer_name_kh = value;
                }
            }
        }
        [MaxLength(150)]
        public string customer_name_kh { get; set; }


        public string gender { get; set; } = "Not Set";


        public string email { get; set; }


        public string address { get; set; }

        public decimal total_receivable { get; set; }


        public string phone_1 { get; set; }

        public string phone_2 { get; set; }
        public string photo { get; set; }
        public string nationality { get; set; }


        [Column(TypeName = "date")]
        public DateTime date_of_birth { get; set; }   = DateTime.Now.AddYears(-18);

        public string note { get; set; }
        public bool is_synced { get; set; } = false;

        public Guid? last_update_business_branch_id { get; set; }

    }
}
