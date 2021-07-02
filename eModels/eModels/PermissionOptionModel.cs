using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace eModels
{
    [Table("tbl_permission_option")]
    public class PermissionOptionModel
    {
        [Key]
        public int id { get; set; }
        public PermissionOptionModel()
        {
            permission_option_roles = new List<PermissionOptionRoleModel>();
        }
        public int? parent_id { get; set; }
        [ForeignKey("parent_id")]
        public PermissionOptionModel parent { get; set; }
        public List<PermissionOptionModel> permission_options { get; set; }


        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(50)]
        public string option_name { get; set; }

        public string note { get; set; }

        public string roles { get; set; }

        public int sort_order { get; set; } = 0;
        public bool show_in_menu { get; set; } = false;

        public string icon { get; set; }

        [MaxLength(100)]
        public string url { get; set; }


        public bool is_match_all { get; set; } = false;

        [MaxLength(50)]
        public string report_title { get; set; }

        [MaxLength(50)]
        public string report_title_kh { get; set; }

        public bool is_front_end { get; set; } = false;
        public bool is_report { get; set; } = false;

        public List<PermissionOptionRoleModel> permission_option_roles { get; set; }

        [NotMapped, JsonIgnore]
        public bool is_checked { get; set; }

        [NotMapped, JsonIgnore]
        public bool is_open_child { get; set; }

        public bool is_public_report { get; set; } = true;

        public bool status { get; set; }
    }  
}
