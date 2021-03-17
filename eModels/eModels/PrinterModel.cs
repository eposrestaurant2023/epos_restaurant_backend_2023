using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eModels
{
    [Table("tbl_printer")]
    public class PrinterModel   : CoreModel
    {


        public Guid business_branch_id { get; set; }
        [ForeignKey("business_branch_id")]
        public BusinessBranchModel business_branch { get; set; }

        [MaxLength(50)]
        public string printer_name { get; set; } = "";
        public string ip_address { get; set; } = "";
        public string port { get; set; } = "";
 

        public string note { get; set; } = "";
    }
}
