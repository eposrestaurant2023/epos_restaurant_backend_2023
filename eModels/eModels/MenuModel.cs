using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_menu")]
    public class MenuModel : CoreModel
    {
        
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        public int? parent_id { get; set; }
        [ForeignKey("parent_id")]
        public MenuModel parent { get; set; }

        private string _menu_name_en;
        [MaxLength(100)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string menu_name_en
        {
            get { return _menu_name_en; }
            set { _menu_name_en = value;
                if (string.IsNullOrEmpty(menu_name_kh))
                {
                    menu_name_kh = value;
                }
            }
        }

        public string menu_name_kh { get; set; }
        public List<MenuModel> menus { get; set; }

    }

}
