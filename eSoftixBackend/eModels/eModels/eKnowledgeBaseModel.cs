using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_eknowledge_base")]
    public class eKnowledgeBaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }

        public string icon { get; set; }
        public string title_en { get; set; }
        public string title_kh { get; set; }
        public string description_en { get; set; }
        public string description_kh { get; set; }


        public Guid? parent_id { get; set; }
        [ForeignKey("parent_id")]
        public eKnowledgeBaseModel parent { get; set; }

        public List<eKnowledgeBaseModel> children { get; set; }

        public int sort_order { get; set; }



    }
}
