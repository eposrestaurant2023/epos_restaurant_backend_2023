using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace eAPIClient.Models
{
    [Table("tbl_note")]
    public class NoteModel 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }
 
        public string note { get; set; }
        public int category_note_id { get; set; }
        public string category{ get; set; }

        public bool is_predefine_note { get; set; } = false;
        public int product_id { get; set; }
    }

    public class ShareNoteModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }
        public string note { get; set; }
        public int category_note_id { get; set; }

        public string category{ get; set; }
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
