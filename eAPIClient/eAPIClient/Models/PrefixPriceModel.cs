using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
    [Table("tbl_prefix_price")]
    public class PrefixPriceModel
    {
        [Key]
        public int id { get; set; }
        public string prefix_price_name { get; set; }
        public string prefix_price_value { get; set; }
        public int payment_type_id { get; set; }
        public string note { get; set; }
        public bool status { get; set; }
    }
}

