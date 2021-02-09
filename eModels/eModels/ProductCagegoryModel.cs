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

        public List<ProductModel> products { get; set; }
    }
}
