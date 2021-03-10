using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
    [Table("tbl_document_number")]
    public class DocumentNumberModel : KeyModel
    {
        public string document_name { get; set; }
        public string outlet_id { get; set; }
        public string prefix { get; set; }
        public string format { get; set; }
        public int counter { get; set; }
        public string counter_digit { get; set; }
    }
}
