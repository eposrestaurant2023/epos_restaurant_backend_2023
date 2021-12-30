using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
    [Table("tbl_translate_text")]
    public class TranslateTextModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }

        public string language_code { get; set; }

        public string key { get; set; }
        public string value { get; set; }
    }
}
