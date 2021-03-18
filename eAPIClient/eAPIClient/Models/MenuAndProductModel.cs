using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{


    [Table("tbl_menu")]
    public class MenuModel
    {
        public MenuModel()
        {
            product_menus = new List<ProductMenuModel>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int? parent_id { get; set; }
        public string menu_name_en { get; set; }
        public string menu_name_kh { get; set; }
        public string text_color { get; set; }
        public string background_color { get; set; }    
        public int root_menu_id { get; set; }

        public List<ProductMenuModel> product_menus { get; set; }

    }

     [Table("tbl_product")]
    public class ProductModel
    {
        public ProductModel()
        {
            product_printers = new List<ProductPrinterModel>();
            product_modifiers = new List<ProductModifierModel>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }


   

        private string _product_code ="";

        public string product_code
        {
            get { return _product_code; }
            set {
                _product_code = value; 
                keyword +=value +" ";
            }
        }


        private string _product_name_en ="";

        public string product_name_en
        {
            get { return _product_name_en; }
            set { 
                _product_name_en = value;
                keyword +=  value +" ";
            }
        }

        private string _product_name_kh = "";

        public string product_name_kh
        {
            get { return _product_name_kh; }
            set
            {
                _product_name_kh = value;
                keyword += value+" ";
            }
        } 
        public string photo { get; set; }
        public bool is_allow_discount { get; set; }
        public bool is_inventory_product { get; set; }

        public List<ProductPrinterModel> product_printers { get; set; }
        public List<ProductModifierModel> product_modifiers { get; set; }
        public List<ProductPortionModel> product_portions { get; set; }
        public string keyword
        {
            get;
            set;
        }


    }

    [Table("tbl_product_menu")]
    public class ProductMenuModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual ProductModel product { get; set; }
        public int menu_id { get; set; }
        [ForeignKey("menu_id")]
        public virtual MenuModel menu { get; set; }

    }

    [Table("tbl_product_printer")]
    public class ProductPrinterModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual ProductModel product { get; set; }
        public string printer_name { get; set; }
        public string ip_address { get; set; }
        public int port { get; set; }


    }


    
    [Table("tbl_product_modifier")]
    public class ProductModifierModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int? parent_id { get; set; }
        [ForeignKey("parent_id")]
        public ProductModifierModel parent { get; set; }    
        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual ProductModel product { get; set; }
        public string modifier_name { get; set; }
        public decimal price { get; set; } = 0;  

        public List<ProductModifierModel> children { get; set; }        
        public string section_name { get; set; }    
        public bool is_required { get; set; }
        public bool is_multiple_select { get; set; }     
        public bool is_section { get; set; } = false;     

    }
    
    
    [Table("tbl_product_portion")]
    public class ProductPortionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual ProductModel product { get; set; }
        public string portion_name { get; set; }
        public decimal cost { get; set; } = 0;
        public decimal multiplier { get; set; } = 0;

        public int unit_id { get; set; }

        public string prices { get; set; }



    }


    [Table("tbl_product_price")]
    public class ProductPriceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }

        public int product_portion_id { get; set; }

        public string prices { get; set; }
         
    
    }
}
