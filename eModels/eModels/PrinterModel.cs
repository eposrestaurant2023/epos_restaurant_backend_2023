using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string ip_address_port { get; set; } = "";
 

        public string note { get; set; } = "";
    }
}
