using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
    
    public class SystemFeatureModelCore
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }

        public string feature_code { get; set; }
        public string feature_name { get; set; }
        public string feature_description { get; set; }
        public string permission_options { get; set; }
        public bool status { get; set; }


        
    }
}
