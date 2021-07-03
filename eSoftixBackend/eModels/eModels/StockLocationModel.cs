using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{
    [Table("tbl_stock_location")]
    public class StockLocationModel
    {
        [Key]
        public Guid id { get; set; }
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        [Required]
        public string stock_location_name { get; set; }
        public bool is_default { get; set; } 
        [NotMapped]
        public bool is_loading { get; set; }
        
        [NotMapped]
        public bool is_deleting { get; set; }
        public bool is_deleted { get; set; }


    }
}
