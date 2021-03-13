using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eModels
{
    [Table("tbl_product_category")]
    public class ProductCategoryModel : CoreModel
    {
        public ProductCategoryModel()
        {
            products = new List<ProductModel>();
            modifier_group_product_categories = new List<ModifierGroupProductCategoryModel>();
        }



        public int product_group_id { get; set; }
        [ForeignKey("product_group_id")]
        public ProductGroupModel product_group { get; set; }



        private string _product_category_en;
        [MaxLength(200)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string product_category_en
        {
            get { return _product_category_en; }
            set { _product_category_en = value;
                if (string.IsNullOrEmpty(product_category_kh))
                {
                    product_category_kh = value;
                }
            }
        }

        [MaxLength(200)]
        public string product_category_kh { get; set; }
        public bool is_ingredient_category { get; set; }
        public bool is_deleted_group { get; set; }
        public bool status_group { get; set; } = true;
        public List<ProductModel> products { get; set; }
        public List<ModifierGroupProductCategoryModel> modifier_group_product_categories { get; set; }

        public bool has_ingredient_product { get; set; }


    }
}
