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
    [Table("tbl_sale_type")]
    public class SaleTypeModel 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }
        public Guid outlet_id { get; set; }

        [ForeignKey("business_branch_id")]

        public BusinessBranchModel business_branch { get; set; }
    }
}
