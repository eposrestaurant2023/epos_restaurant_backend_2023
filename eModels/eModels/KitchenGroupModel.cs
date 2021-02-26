using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_kitchen_group")]
    public class KitchenGroupModel 
    {
        public KitchenGroupModel()
        {
            products = new List<ProductModel>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string kitchen_group_name { get; set; }
        public int sort_order { get; set; }
        public List<ProductModel> products { get; set; }
 
    }
}
