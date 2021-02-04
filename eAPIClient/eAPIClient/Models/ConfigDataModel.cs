using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace eAPIClient.Models
{
    [Table("tbl_config_data")]

    public class ConfigDataModel
    {
        public int id { get; set; }

        public Guid business_branch_id { get; set; }
        public string data  { get; set; }
        public string note{ get; set; }
    }

}
