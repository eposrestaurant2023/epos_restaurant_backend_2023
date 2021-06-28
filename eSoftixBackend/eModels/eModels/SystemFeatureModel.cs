using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_system_feature")]
   public class SystemFeatureModel:SystemFeatureModelCore
    {
        public List<BusinessBranchSystemFeatureModel> business_branch_system_features { get; set; }
        public List<ProjectSystemFeatureModel> project_system_features { get; set; }
    }
}
