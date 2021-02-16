using eShareModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;              
namespace eModels
{

    [Table("tbl_config_data")]
    public class ConfigDataModel    :ConfigDataShareModel {
        public Guid business_branch_id { get; set; }
    }
}
