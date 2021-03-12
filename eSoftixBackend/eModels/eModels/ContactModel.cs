using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{
    [Table("tbl_contact")]
    public class ContactModel:CoreModel
    {
        public int? customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }
        [Required(ErrorMessage ="Please enter contact name")]
        public string contact_name { get; set; }
        [Required(ErrorMessage = "Please enter phone number")]
        public string phone_1 { get; set; }
        public string phone_2 { get; set; }
        public string postion { get; set; }
        public string gender { get; set; }
        public string email_address { get; set; }
        public string telegram { get; set; }
        public int? project_id { get; set; }
        [ForeignKey("project_id")]
        public ProjectModel project { get; set; }

    }
}
