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

 
        public string product_code { get; set; }
        public string product_name_en { get; set; }
        public string product_name_kh { get; set; }
        public string photo { get; set; }
        public bool is_allow_discount { get; set; }
        public bool is_allow_free { get; set; }
        public bool is_inventory_product { get; set; }

        public List<ProductPrinterModel> product_printers { get; set; }
        public List<ProductModifierModel> product_modifiers { get; set; }

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
        public string ip_address_port { get; set; }


    }


    
    [Table("tbl_product_modifier")]
    public class ProductModifierModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual ProductModel product { get; set; }
        public string modifier_name { get; set; }
        public decimal price { get; set; } = 0;

    }



}
