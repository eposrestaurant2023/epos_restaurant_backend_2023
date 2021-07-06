using eShareModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace eAPIClient.Models
{
    [Table("tbl_note")]
    public class NoteModel : CoreGUIDModel
    {
        public int note_id { get; set; }
        public string note { get; set; }
        public int category_note_id { get; set; }
    }

    public class ShareNoteModel: CoreModel
    { 
        public string note { get; set; }
        public int category_note_id { get; set; }
    }

    [Table("tbl_category_note")]
    public class CategoryNoteModel : KeyGUIDModel
    {
        public int category_note_id { get; set; }
        public string category_note_name_en { get; set; }
        public string category_note_name_kh { get; set; }
        public bool is_multiple_select { get; set; }
    }
    public class ShareCategoryNoteModel 
    {
        public int id { get; set; }
        public string category_note_name_en { get; set; }
        public string category_note_name_kh { get; set; }
        public bool is_multiple_select { get; set; }
    }

}
