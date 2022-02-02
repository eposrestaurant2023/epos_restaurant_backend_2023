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
        [Required(ErrorMessage = "Please select a role.")]
        [Range(1, int.MaxValue,ErrorMessage = "Please select a role.")]
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public RoleModel role { get; set; }

        [StringLength(20, ErrorMessage = "Full Name is too long.")]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string username { get; set; }

        [StringLength(50, ErrorMessage = "Full Name is too long.")]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string full_name { get; set; }

        [StringLength(20, ErrorMessage = "Password is too long.")]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string password { get; set; } = "";
      
        public string user_code { get; set; }
        public string pin_code { get; set; }
        public string email { get; set; }
        public string phone_1 { get; set; }
        public string phone_2 { get; set; }           

        public string note { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_of_birth { get; set; } = DateTime.Now.AddYears(-18);

        public bool is_allow_front_end_login { get; set; } = true;
        public bool is_allow_backend_login { get; set; } = true;

        public bool is_default { get; set; } = false;
        public bool is_buildin { get; set; } = false;
        public string photo { get; set; } = "";
    }
}