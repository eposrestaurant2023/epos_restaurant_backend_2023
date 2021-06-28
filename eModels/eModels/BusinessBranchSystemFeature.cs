using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_business_branch_system_feature")]
    public class BusinessBranchSystemFeatureModel
    {

        public Guid business_branch_id { get; set; }

        public Guid system_feature_id { get; set; }

        public bool status { get; set; }


    }
}
