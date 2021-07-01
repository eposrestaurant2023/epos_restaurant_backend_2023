using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eModels
{
    [Table("tbl_sale_type")]
    public class SaleTypeModel 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }
        public Guid business_branch_id { get; set; }

        [ForeignKey("business_branch_id")]

        public BusinessBranchModel business_branch { get; set; }

        public bool is_build_in { get; set; }

        public string sale_type_name { get; set; }

        public bool is_order_use_table { get; set; } = true;
        public bool is_deleted { get; set; } = false;

    }
}
