using System;
using System.ComponentModel.DataAnnotations;
using eShareModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_note")]
    public  class NoteModel: CoreModel
    {
        [Required(ErrorMessage ="Please Select a business branch.")]
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }


        [Required(ErrorMessage = "Please Select a category note.")]
        [Range(1,int.MaxValue)]
        public int category_note_id { get; set; }
        [ForeignKey("category_note_id")]
        public CategoryNoteModel category_note { get; set; }

        [MaxLength(250)]
        public string note { get; set; } = "";
    }
}
