using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
   [Table("tbl_product_group")]
   public class ProductGroupModel : CoreModel
   {
        public ProductGroupModel()
        {
            product_categories = new List<ProductCategoryModel>();
        }

        [MaxLength(50)]
        public string product_group_code { get; set; }

        private string _product_group_en;
        [MaxLength(200)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string product_group_en
        {
            get { return _product_group_en; }
            set
            {
                _product_group_en = value;
                if (string.IsNullOrEmpty(product_group_kh))
                {
                    product_group_kh = value;
                }
            }
        }

        [MaxLength(200)]
        public string product_group_kh { get; set; }

        public int sort_order { get; set; }
        public bool is_built_in { get; set; }


        [NotMapped, JsonIgnore]
        public bool is_view_deleted_category { get; set; } = false;


        public List<ProductCategoryModel> product_categories { get; set; }
    }
}
