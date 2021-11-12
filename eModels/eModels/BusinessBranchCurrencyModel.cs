using eShareModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;    

namespace eModels
{
    [Table("tbl_business_branch_currency")]
    public class BusinessBranchCurrencyModel : BusinessBranchCurrencyShareModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }

        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }
        [ForeignKey("currency_id")]
        public CurrencyModel currency { get; set; }

        public bool is_deleted { get; set; }

        [NotMapped]
        public bool is_selected { get; set; }
    }
}
