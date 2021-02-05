using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eAPIClient.Models
{
 
    public class UserModel 
    {
        [Key]
        public int id { get; set; }

        public string username { get; set; }
        public string full_name { get; set; }
        [MaxLength(20)]
        public string password { get; set; } = "";
        public int pin_code { get; set; }

    }
    public class UserBusinessBranchModel
    {
        public int user_id { get; set; }

        public UserModel user { get; set; }


        public Guid business_branch_id { get; set; }
   
    }
}