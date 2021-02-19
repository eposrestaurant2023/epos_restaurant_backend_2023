using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{
    public class ContactModel:CoreModel
    {
        public int customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }

        public string contact_name { get; set; }
        public string phone_1 { get; set; }
        public string phone_2 { get; set; }
        public string postion { get; set; }
        public string gender { get; set; }
        public string email_address { get; set; }
        public string telegram { get; set; }
    }
}
