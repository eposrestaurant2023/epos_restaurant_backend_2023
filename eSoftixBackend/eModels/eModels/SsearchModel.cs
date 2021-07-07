using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_search")]
    public class SearchModel
    {
        [Key]
        public Guid id { get; set; }
        public string icon { get; set; }
        public string photo { get; set; }
        public string alias { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string keyword { get; set; }
        public string url { get; set; }
    }
}
