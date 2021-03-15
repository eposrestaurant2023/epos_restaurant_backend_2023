using eShareModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    public  class NoteModel     : CoreModel
    {

        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        public int category_note_id { get; set; }
        [ForeignKey("category_note_id")]
        public CategoryNoteModel category_note { get; set; }

        [MaxLength(250)]
        public string note_label { get; set; }
    }
}
