
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
   public class SaleStatusShareModel
    {
        [Key,]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string status_name { get; set; }
        public string background { get; set; }
        public string foreground { get; set; }
        public int priority { get; set; }
        public bool is_sale_lock { get; set; }
        public bool is_active_order { get; set; }
    }
}
