using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    [Table("tbl_user")]
    public class UserModel : CoreModel
    {
        [Required(ErrorMessage = "Please select a role")]                     
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public RoleModel role { get; set; }



        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(20)]
        public string username { get; set; }

        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(50)]
        public string full_name { get; set; }

        [MaxLength(20)]
        public string password { get; set; } = "";

        [MaxLength(2)]
        public int user_code { get; set; }
        public int pin_code { get; set; }

        [MaxLength(50)]
        public string email { get; set; }

        [MaxLength(50)]
        public string phone_1 { get; set; }

        [MaxLength(50)]
        public string phone_2 { get; set; }           

        public string note { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_of_birth { get; set; } = DateTime.Now.AddYears(-18);

        public bool is_allow_front_end_login { get; set; } = true;
        public bool is_allow_backend_login { get; set; } = true;

        public bool is_default { get; set; } = false;
        public bool is_buildin { get; set; } = false;
        public string photo { get; set; } = "";

        [NotMapped,  JsonIgnore ]
        public string new_password { get; set; }    

    }
}