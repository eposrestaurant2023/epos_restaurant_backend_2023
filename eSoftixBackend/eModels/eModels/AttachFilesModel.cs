using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{

    public class AttachFileModel : CoreModel
    {
        public string file_title { get; set; }
        public string file_name { get; set; }
        public string note { get; set; }
        public string file_type { get; set; }
        public long file_size { get; set; }

        public string file_extension { get; set; }


    }


    [Table("tbl_attach_files")]
    public class AttachFilesModel : AttachFileModel
    {
        public bool is_document_file { get; set; } = false;
        

        public int? customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }

        public int? project_id { get; set; }
        [ForeignKey("project_id")]
        public ProjectModel project { get; set; }

    }

    public class InputFileData
    {
        public MultipartFormDataContent multipartForm { get; set; }
        public string ImageUrl { get; set; } = "";
        public string SaveFolderPath { get; set; } = "";
    }

}
