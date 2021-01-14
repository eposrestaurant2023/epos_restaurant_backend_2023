using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{
    [Table("tbl_category_note")]
    public class CategoryNoteModel
    {
        public CategoryNoteModel()
        {
            notes = new List<NoteModel>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int id { get; set; }

        [MaxLength(50)]
        public string category_note_name_en { get; set; }
        [MaxLength(50)]
        public string category_note_name_kh { get; set; }

        public int sort_order { get; set; }
        public bool is_multiple_select { get; set; } = false;

        public List<NoteModel> notes { get; set; }
    }
}
