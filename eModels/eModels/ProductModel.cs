using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    

    [Table("tbl_product")]
    public class ProductModel : CoreModel
    {
        public ProductModel()
        {      
            histories = new List<HistoryModel>();
            product_printers = new List<ProductPrinterModel>();
            product_portions = new List<ProductPortionModel>();
            product_menus = new List<ProductMenuModel>();
            product_modifiers = new List<ProductModifierModel>();
          

        }

        [Required(ErrorMessage = "Please select a category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int product_category_id { get; set; }
        [ForeignKey("product_category_id")]
        public ProductCategoryModel product_category { get; set; }

        public int product_type_id { get; set; } = 1;
        [ForeignKey("product_type_id")]
        public ProductTypeModel product_type { get; set; }

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

        [Required(ErrorMessage = "Please select unit.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select unit.")]
        public int unit_id { get; set; } = 1;
        [ForeignKey("unit_id")]
        public UnitModel unit{ get; set; }
        public int? vendor_id { get; set; }
        [ForeignKey("vendor_id")]
        public VendorModel vendor { get; set; }

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
        public List<ProductPortionModel> product_portions { get; set; }
        public List<ProductMenuModel> product_menus { get; set; }


        public List<ProductModifierModel> product_modifiers { get; set; }


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
        public ProductModel product{ get; set; }

        public string printer_name { get; set; }
        public string ip_address_port { get; set; }

        public bool is_deleted { get; set; } = false;

    }

    [Table("tbl_product_portion")]
    public class ProductPortionModel:CoreModel
    {
        public ProductPortionModel()
        {
            product_prices = new List<ProductPriceModel>();
            product_ingredients = new List<ProductIngredientModel>();

        }
        public List<ProductIngredientModel> product_ingredients { get; set; }
        public ProductPortionModel(List<PriceRuleModel> price_rules )
        {
            product_prices = new List<ProductPriceModel>();
            foreach(var r in price_rules)
            {
                product_prices.Add(new ProductPriceModel() { price_rule_id = r.id});
                
            }

          
        }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        [Required(ErrorMessage = "Please select unit.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select unit.")]
        public int unit_id { get; set; } = 1;
        [ForeignKey("unit_id")]
        public UnitModel unit { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string portion_name { get; set; }

 

        private decimal _multipler = 1;

        public decimal multiplier
        {
            get { return _multipler; }
            set {
                if (value == 0)
                {
                    value = 1;
                }
                _multipler = value;
                
            }
        }

        public decimal cost { get; set; }

        public List<ProductPriceModel> product_prices { get; set; }
     

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
}
