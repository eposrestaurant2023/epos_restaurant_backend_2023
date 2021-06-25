using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;
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
        public string project_type_name { get; set; }



        public List<ProjectModel> projects { get; set; }

        public string note { get; set; }

        public string icon { get; set; }
        public int color { get; set; }


    }
}
