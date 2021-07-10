using eModels.Attribute;
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
            project_system_features = new List<ProjectSystemFeatureModel>();
            business_branches = new List<BusinessBranchModel>();
            contacts = new List<ContactRelatedModel>();
        }
        [Range(1, int.MaxValue, ErrorMessage = "Please select Project Type")]
        [Display(Name = "Project Type")]
        public int project_type_id { get; set; }
        [ForeignKey("project_type_id")]
        public ProjectTypeModel project_type { get; set; }


        [NotEmpty(ErrorMessage = "Please select customer")]
        [Display(Name = "Customer ")]
        public Guid customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }


        [Required(ErrorMessage ="Please enter project name")]
        [Display(Name = "Project Name")]

        public string project_name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? start_date { get; set; } = DateTime.Now;
        public string customer_code_name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? closed_date { get; set; }

        public bool is_closed { get; set; }
        public bool is_expired { get; set; }
        public string closed_note { get; set; }
        public string project_code { get; set; }
        public bool is_paid { get; set; }
        [Column(TypeName = "date")]
        public DateTime? paid_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? expired_date { get; set; }

        public bool is_full_license { get; set; }

        public int total_business_branches { get; set; }
        public int total_outlets { get; set; }
        public int total_stations { get; set; }
        public int total_stock_location { get; set; }

        public string note { get; set; }
        public string server_id { get; set; }
        public string closed_by { get; set; }


        


        public List<BusinessBranchModel> business_branches { get; set; }

        public List<ContactRelatedModel> contacts { get; set; }
        public List<ProjectSystemFeatureModel> project_system_features { get; set; }
    }
}
