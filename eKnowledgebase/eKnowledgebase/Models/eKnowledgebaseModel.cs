using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eKnowledgebase.Models
{
    public class eKnowledgebaseModel
    {
         

            public eKnowledgebaseModel()
            {
                children = new List<eKnowledgebaseModel>();
            }

            public eKnowledgebaseModel(Guid _id)
            {
                parent_id = _id;
                children = new List<eKnowledgebaseModel>();
            }
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          
            public Guid id { get; set; }

        public string youtube_url { get; set; }
        public string? icon { get; set; }

            public string? title_en { get; set; }

            public string? title_kh { get; set; }
            public string? description_en { get; set; }
            public string? description_kh { get; set; }
            public string? photo_en { get; set; }
            public string? photo_kh { get; set; }

            public bool is_deleted { get; set; } = false;

            [NotMapped]
            [JsonIgnore]
            public bool is_select_parent { get; set; }
            public Guid? parent_id { get; set; }
            [ForeignKey("parent_id")]
            public eKnowledgebaseModel? parent { get; set; }

            public List<eKnowledgebaseModel> children { get; set; }

            public int sort_order { get; set; }



        
    }
}
