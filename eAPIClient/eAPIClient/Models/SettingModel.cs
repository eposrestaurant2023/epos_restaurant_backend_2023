using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eAPIClient.Models
{
    [Table("tbl_setting")]
    public  class SettingModel
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }       
        public string setting_title { get; set; }
        public string setting_description { get; set; }
        public string setting_value { get; set; }
        public bool status { get; set; }
    }
}
