using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_business_branch_setting")]
    public class BusinessBranchSettingModel
    {
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        public int setting_id { get; set; }
        [ForeignKey("setting_id")]
        public SettingModel setting { get; set; }
    }
}
