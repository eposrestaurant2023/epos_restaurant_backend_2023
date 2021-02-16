using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
   public class ConfigDataShareModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }    
        public string data { get; set; }
        public string config_type { get; set; }
        public string note { get; set; }
    }
}
