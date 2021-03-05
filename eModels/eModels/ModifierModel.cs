
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using eShareModel;

namespace eModels
{
    [Table("tbl_modifier")]
   public class ModifierModel : CoreModel
    {
        public ModifierModel()
        {
            product_modifiers = new List<ProductModifierModel>();
            modifier_ingredients = new List<ModifierIngredientModel>();
        }
 
        public int? modifier_group_id { get; set; }
        [ForeignKey("modifier_group_id")]
        public ModifierGroupModel modifier_group { get; set; }


        [Required(ErrorMessage = "Field cannot be blank.")]
        public string modifier_name { get; set; }

        

        public List<ProductModifierModel> product_modifiers { get; set; }
        public List<ModifierGroupItemModel> modifier_group_items { get; set; }
        public List<ModifierIngredientModel> modifier_ingredients { get; set; }

    }

    [Table("tbl_modifier_group_item")]
    public class ModifierGroupItemModel : CoreModel
    {
        public ModifierGroupItemModel()
        {
            children = new List<ModifierGroupItemModel>();
        }
        public int? parent_id { get; set; }
        [ForeignKey("parent_id")]
        public ModifierGroupItemModel parent { get; set; }
        public List<ModifierGroupItemModel> children { get; set; }

        public int? modifier_group_id { get; set; }
        [ForeignKey("modifier_group_id")]
        public ModifierGroupModel modifier_group { get; set; }
        public int? modifier_id { get; set; }
        [ForeignKey("modifier_id")]
        public ModifierModel modifier { get; set; }

        public int sort_order { get; set; }
        public bool is_section { get; set; }
        public bool is_required { get; set; }
        public bool is_multiple_select { get; set; }
        public string section_name { get; set; }
        public decimal price { get; set; }
         

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

        private string _modifier_group_name_en;
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string modifier_group_name_en
        {
            get { return _modifier_group_name_en; }
            set
            {
                _modifier_group_name_en = value;
                if (string.IsNullOrEmpty(modifier_group_name_kh))
                {
                    modifier_group_name_kh = value;
                }
            }
        }
        public string modifier_group_name_kh { get; set; }
        

        public List<ModifierGroupProductCategoryModel> modifier_group_product_categories { get; set; }
        public List<ModifierGroupItemModel> modifier_group_items { get; set; }
        public List<HistoryModel> histories { get; set; }
        public List<AttachFilesModel> attach_files { get; set; }

        [NotMapped, JsonIgnore]
        public string modifier_group_display_name
        {
            get
            {
                return (string.IsNullOrEmpty(modifier_group_name_kh) ? modifier_group_name_en : $"{modifier_group_name_en} ({modifier_group_name_kh})");
            }
        }
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
        public ProductModifierModel()
        {
            children = new List<ProductModifierModel>();
        }

     
        public int? parent_id { get; set; }
        [ForeignKey("parent_id")]
        public ProductModifierModel parent { get; set; }
        public List<ProductModifierModel> children { get; set; }



        public int? modifier_id { get; set; }
        [ForeignKey("modifier_id")]
        public ModifierModel modifier { get; set; }

        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }

        public decimal price { get; set; }

        public int modifier_group_id { get; set; }

        public string modifier_name { get; set; }

        public string section_name { get; set; }

    
        public bool is_required { get; set; }
        public bool is_multiple_select { get; set; }

        public bool is_section { get; set; } = false;
        public int sort_order { get; set; }

    } 
}
