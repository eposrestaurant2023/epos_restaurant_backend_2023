using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using eShareModel;
namespace eModels
{
    [Table("tbl_role")]
    public class RoleModel : CoreModel
    {
        public RoleModel()
        {                                      
            permission_option_roles = new List<PermissionOptionRoleModel>();
        }


        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(50)]
        public string role_name { get; set; }
        public string description { get; set; }

        public bool is_buildin { get; set; }
        private HashSet<int> _SelectedValues;
        [NotMapped]
        public HashSet<int> SelectedValues
        {
            get {
                if (permission_option_roles.Any())
                {
                    _SelectedValues = new HashSet<int>();
                    foreach (var s in permission_option_roles)
                    {
                        
                        _SelectedValues.Add(s.permission_option_id);
                    }
                }
                return _SelectedValues; 
            }
            set { _SelectedValues = value; }
        }

        private string _options_id;
        [NotMapped]
        public string options_id
        {
            get {
                if (SelectedValues != null)
                {
                    _options_id = string.Join(",", SelectedValues);
                }
                return _options_id; 
            }
            set { _options_id = value; }
        }
        public List<PermissionOptionRoleModel> permission_option_roles { get; set; }  
    }
}
