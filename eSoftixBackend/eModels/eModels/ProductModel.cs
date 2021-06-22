using eShareModel;
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
            product_printers = new List<ProductPrinterModel>();
  
              

        }

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
        public bool is_menu_product { get; set; }
        public bool is_ingredient_product { get; set; }
        public decimal cost { get; set; }
        public List<HistoryModel> histories { get; set; }
     
        [NotMapped, JsonIgnore]
        public string product_display_name
        {
            get
            {
                return (string.IsNullOrEmpty(product_code) ? "" : (product_code + " - ")) + "" + product_name_en;
            }
        }


        public List<ProductPrinterModel> product_printers { get; set; }
     

      
        public decimal min_price { get; set; }
        public decimal max_price { get; set; }

    }


    [Table("tbl_product_printer")]
    public class ProductPrinterModel
    {
        [Key]
        public int id { get; set; }
        public int printer_id { get; set; }
        [ForeignKey("printer_id")]
        public PrinterModel printer { get; set; }



        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        public string printer_name { get; set; }
        public string ip_address_port { get; set; }

        public bool is_deleted { get; set; } = false;

    }

    
    public class SelectedProductModel
    {
        public ProductModel product { get; set; }
        // Both Data
        public string unit { get; set; } = "Unit";
        public decimal price { get; set; }
        public decimal cost { get; set; }
        public decimal quantity { get; set; } = 1;
        public bool is_allow_discount { get; set; }

    }

    [Table("tbl_product_type")]
    public class ProductTypeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [MaxLength(100)]
        public string product_type_name { get; set; }

        public int sort_order { get; set; } = 0;
        public bool status { get; set; }
        public string background_color { get; set; }

    }

}
