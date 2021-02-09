
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_product_type")]
    public class ProductTypeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }    

        [MaxLength(100)]
        public string product_type_name { get; set; }

        public int sort_order { get; set; } = 0;
        public bool status { get; set; }

    }
}
