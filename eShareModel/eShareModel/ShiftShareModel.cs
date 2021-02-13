
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
    [Table("tbl_shift")]
    public class ShiftShareModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string shift_name { get; set; }
        public int sort_order { get; set; }
    }
}
