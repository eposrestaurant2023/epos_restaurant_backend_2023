using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eModels
{
    [Table("tbl_setting")]
    public  class SettingModel
    {
        public SettingModel()
        {
            business_branch_settings = new List<BusinessBranchSettingModel>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int id { get; set; }       
        public string setting_title { get; set; }
        public string setting_description { get; set; }
        public string input_type { get; set; }
        public bool status { get; set; }

        [NotMapped ]
        [JsonIgnore]
        public bool is_saving { get; set; }
        public List<BusinessBranchSettingModel> business_branch_settings { get; set; }
    }
}
