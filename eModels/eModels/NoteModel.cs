using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    public  class NoteModel     : CoreModel
    {
        public int category_note_id { get; set; }
        [ForeignKey("category_note_id")]
        public CategoryNoteModel category_note { get; set; }

        [MaxLength(250)]
        public string note_label { get; set; }
    }
}
