using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eModels
{
    [Table("tbl_menu")]
    public class MenuModel : CoreModel
    {
        public MenuModel()
        {
            product_menus = new List<ProductMenuModel>();
        }

        public int? root_menu_id { get; set; }

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
        public string photo { get; set; } = "";
        public List<MenuModel> menus { get; set; }

        public List<ProductMenuModel> product_menus { get; set; }


        public string text_color { get; set; } = "#333333";
        public string background_color { get; set; } = "#ececec";
 

        private string _menu_path;

        public string menu_path
        {
            get {
                if (_menu_path == "")
                {
                    _menu_path = menu_name_en;
                }
                return _menu_path; }
            set { _menu_path = value; }
        }



    }

    [Table("tbl_product_menu")]
    public class ProductMenuModel
    {

        [Key]
        public int id { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual ProductModel product { get; set; }
        public int menu_id { get; set; }
        [ForeignKey("menu_id")]
        public virtual MenuModel menu { get; set; }

        public bool is_deleted { get; set; }

    }

}
