using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace eModels
{
    [Table("tbl_product_category")]
    public class ProductCategoryModel : CoreModel
    {
        public ProductCategoryModel()
        {
            products = new List<ProductModel>();
        }
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string product_category { get; set; }

        [Required(ErrorMessage = "Field cannot be blank.")]
        public string product_category_kh { get; set; }
        public List<ProductModel> products { get; set; }
        public string background_color { get; set; } = String.Format("#{0:X6}", (new Random()).Next(0x1000000));

        public string prefix { get; set; }
        public string digit { get; set; }
        public string format { get; set; }
        public int counter { get; set; }
    }

    [Table("tbl_product")]
    public class ProductModel : CoreModel
    {
        public ProductModel()
        {
       
            
            histories = new List<HistoryModel>();
           
        }

        public decimal quantity { get; set; }
        public int total_variants { get; set; }
        public string keyword { get; set; }
        public List<HistoryModel> histories { get; set; }
        public int created_outlet_id { get; set; }

        private bool _is_auto_generate_product_code = false;

        public bool is_auto_generate_product_code
        {
            get { return _is_auto_generate_product_code; }
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
                _is_auto_generate_product_code = value;
            }
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

        private string _product_name;
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string product_name
        {
            get { return _product_name; }
            set
            {
                _product_name = value;
                if (string.IsNullOrEmpty(product_name_kh))
                {
                    product_name_kh = value;
                }
            }
        }


        [Required(ErrorMessage = "Field cannot be blank.")]
        public string product_name_kh { get; set; }

        public string product_code { get; set; }
        public string product_code_1 { get; set; }
        public string product_code_2 { get; set; }
        public string photo { get; set; } = "placeholder.png";
        public string note { get; set; }
        public decimal price { get; set; }
        public decimal cost { get; set; }

        public string unit { get; set; } = "Unit";



        public bool is_allow_discount { get; set; } = true;

        public string background_color { get; set; } = String.Format("#{0:X6}", (new Random()).Next(0x1000000));
        public string text_color { get; set; } = "#333333";
        [NotMapped, JsonIgnore]
        public string product_display_name
        {
            get
            {
                return (string.IsNullOrEmpty(product_code) ? "" : (product_code + " - ")) + "" + product_name;
            }
        }


        //variant
        public bool has_variant { get; set; }
        public bool track_quantity_on_variant { get; set; }

        public bool use_variant_1 { get; set; }
        public string variant_1_name { get; set; }

        public bool use_variant_2 { get; set; }
        public string variant_2_name { get; set; }
        public bool use_variant_3 { get; set; }
        public string variant_3_name { get; set; }
        public bool use_variant_price { get; set; }

        public bool is_inventory_product { get; set; }

         



    }







    [Table("tbl_product_type")]
    public class ProductTypeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string product_type_name { get; set; }
        public int sort_order { get; set; } = 0;

        public string background_color { get; set; }

        [NotMapped]
        public bool is_active { get; set; }

        public bool status { get; set; }

    }



    public class SelectedProductModel
    {
        public ProductModel product { get; set; }
       
        // Both Data
        public string unit { get; set; } = "Unit";
        public decimal price { get; set; }
        public decimal quantity { get; set; } = 1;
        public bool is_allow_discount { get; set; }

    }
     
   
}
