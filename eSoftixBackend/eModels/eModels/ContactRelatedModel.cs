using eShareModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_contact_related")]
    public class ContactRelatedModel:CoreModel
    {

        public int contact_id { get; set; }
        [ForeignKey("contact_id")]
        public ContactModel contact { get; set; }
        public Guid? project_id { get; set; } 
        public Guid? customer_id { get; set; } 
        public Guid? business_branch_id { get; set; } 

    }
}
