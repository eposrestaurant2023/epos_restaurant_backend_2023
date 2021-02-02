using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace eModels
{
    

    [Table("tbl_product")]
    public class ProductModel : CoreModel
    {
        public ProductModel()
        {      
            histories = new List<HistoryModel>();   
        }

        
         

        [Required(ErrorMessage = "Please select a product type.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a product type.")]
        public int product_type_id { get; set; } = 1; //Default Product
        [ForeignKey("product_type_id")]
        public ProductTypeModel product_type { get; set; }


        [Required(ErrorMessage = "Please select a category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int product_category_id { get; set; }
        [ForeignKey("product_category_id")]
        public ProductCategoryModel product_category { get; set; }

        [MaxLength(50)]
        public string product_code { get; set; }

        private string _product_name_en;
        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(250)]
        public string product_name_en
        {
            get { return _product_name_en; }
            set
            {
                _product_name_en = value;
                if (string.IsNullOrEmpty(product_name_kh))
                {
                    product_name_kh = value;
                }
            }
        }
                                                               
        [MaxLength(250)]
        public string product_name_kh { get; set; }   

        public string photo { get; set; } = "placeholder.png";
        public string note { get; set; }

        public string unit { get; set; } = "Unit";

        public bool is_allow_discount { get; set; } = true;
        public bool is_allow_free { get; set; } = true;
        public bool is_open_product { get; set; } = false;

        private bool _is_auto_generate_code = false;
        public bool is_auto_generate_code
        {
            get { return _is_auto_generate_code; }
            set
            {
                if (value)
                {
                    product_code = "Auto";
                }
                else
                {
                    if (product_code == "Auto")
                    {
                        product_code = "";
                    }
                }
                _is_auto_generate_code = value;
            }
        }

        public bool is_inventory_product { get; set; }
        public List<HistoryModel> histories { get; set; }

        [NotMapped, JsonIgnore]
        public string product_display_name
        {
            get
            {
                return (string.IsNullOrEmpty(product_code) ? "" : (product_code + " - ")) + "" + product_name_en;
            }
        }
    }  
    
     
}
