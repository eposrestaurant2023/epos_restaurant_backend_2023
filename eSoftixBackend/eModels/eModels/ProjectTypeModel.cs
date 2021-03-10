using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_project_type")]
    public  class ProjectTypeModel : CoreModel
    {
        public ProjectTypeModel()
        {
            projects = new List<ProjectModel>();
        }


        [MaxLength(50)]
        public string project_name { get; set; }



        public List<ProjectModel> projects { get; set; }

        public string note { get; set; }


    }
}
