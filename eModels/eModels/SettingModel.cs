using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;     

namespace eModels
{
    [Table("tbl_setting")]
    public  class SettingModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int id { get; set; }

        public string setting_value { get; set; }
        public string setting_title { get; set; }
        public string setting_description { get; set; }
        public bool status { get; set; }
    }
}
