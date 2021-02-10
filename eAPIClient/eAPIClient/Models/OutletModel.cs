using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eAPIClient.Models
{
    [Table("tbl_outlet")]
    public class OutletModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string outlet_name_en { get; set; }
        public string outlet_name_kh { get; set; }
    }
}
