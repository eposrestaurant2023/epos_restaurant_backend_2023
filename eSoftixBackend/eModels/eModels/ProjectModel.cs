using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_project")]
    public class ProjectModel : CoreGUIDModel
    {
        public ProjectModel()
        {
            project_contacts = new List<ContactModel>();
        }
        [Range(1, int.MaxValue, ErrorMessage = "Please select Project Type")]
        [Display(Name = "Project Type")]
        public int project_type_id { get; set; }
        [ForeignKey("project_type_id")]
        public ProjectTypeModel project_type { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please select customer")]
        [Display(Name = "Customer ")]
        public Guid customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customers { get; set; }

        [Required(ErrorMessage ="Please enter project name")]
        [Display(Name = "Project Name")]

        public string project_name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? start_date { get; set; }
        public string customer_code_name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? closed_date { get; set; }

        public bool is_closed { get; set; }
        public string closed_note { get; set; }

        public bool is_paid { get; set; }
        [Column(TypeName = "date")]
        public DateTime? paid_date { get; set; }
        public string note { get; set; }
        public string server_id { get; set; }

        public List<ContactModel> project_contacts { get; set; }


    }
}
