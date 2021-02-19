
using eModels.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using eShareModel;
using System.Text.Json.Serialization;

namespace eModels
{
    [Table("tbl_stock_location")]
    public class StockLocationModel
    {
        [Key]
        public int id { get; set; }
        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        public string stock_location_name { get; set; }
        public bool is_default { get; set; }

        [NotMapped, JsonIgnore]
        public string bustiness_branch_name { get; set; }
    }
}
