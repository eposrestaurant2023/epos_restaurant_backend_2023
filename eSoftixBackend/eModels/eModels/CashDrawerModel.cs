using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_cash_drawer")]
    public class CashDrawerModel: eShareModel.CashDrawerModel
    {
        
        public Guid project_id { get; set; }
        [ForeignKey("project_id")]
        public ProjectModel project { get; set; }

        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
    }
}
