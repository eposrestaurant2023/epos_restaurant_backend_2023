﻿
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eModels
{
    [Table("tbl_modifier")]
   public class ModifierModel : CoreModel
    {
        public ModifierModel()
        {
            product_modifiers = new List<ProductModifierModel>();
        }
        public int? modifier_group_id { get; set; }
        [ForeignKey("modifier_group_id")]
        public ModifierGroupModel modifier_group { get; set; }


        [Required(ErrorMessage = "Field cannot be blank.")]
        public string modifier_name { get; set; }        
        public List<ProductModifierModel> product_modifiers { get; set; }
        public List<ModifierGroupItemModel> modifier_group_items { get; set; }

    }

    [Table("tbl_modifier_group_item")]
    public class ModifierGroupItemModel
    {
        public int modifier_group_id { get; set; }
        [ForeignKey("modifier_group_id")]
        public ModifierGroupModel modifier_group { get; set; }
        public int modifier_id { get; set; }
        [ForeignKey("modifier_id")]
        public ModifierModel modifier { get; set; }

        public int sort_order { get; set; }
        public decimal price { get; set; }
        public bool is_deleted { get; set; } = false;

    }

    [Table("tbl_modifier_group")]
    public class ModifierGroupModel : CoreModel
    {
        public ModifierGroupModel()
        {
            
            modifier_group_product_categories = new List<ModifierGroupProductCategoryModel>();
            modifier_group_items = new List<ModifierGroupItemModel>();
            histories = new List<HistoryModel>();
            attach_files = new List<AttachFilesModel>();
        }
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string modifier_group_name_en { get; set; }
        public string modifier_group_name_kh { get; set; }
      
        public List<ModifierGroupProductCategoryModel> modifier_group_product_categories { get; set; }
        public List<ModifierGroupItemModel> modifier_group_items { get; set; }
        public List<HistoryModel> histories { get; set; }
        public List<AttachFilesModel> attach_files { get; set; }
    }

    [Table("tbl_modifier_group_product_category")]
    public class ModifierGroupProductCategoryModel
    {
        [Key]
        public int id { get; set; }

        public int modifer_group_id { get; set; }
        [ForeignKey("modifer_group_id")]
        public ModifierGroupModel modifier_group { get; set; }

        public int product_category_id { get; set; }
        [ForeignKey("product_category_id")]
        public ProductCategoryModel product_category { get; set; }

        public bool is_deleted { get; set; }
    }

    [Table("tbl_product_modifier")]

    public class ProductModifierModel: CoreModel
    {
        public int modifier_id { get; set; }
        [ForeignKey("modifier_id")]
        public ModifierModel modifier { get; set; }

        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        public decimal price { get; set; }

        public int modifier_group_id { get; set; }

        public string modifier_name { get; set; }


    }
}
