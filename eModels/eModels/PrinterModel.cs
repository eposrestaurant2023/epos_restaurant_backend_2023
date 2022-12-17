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
        public int port { get; set; } = 9100;

        public int group_item_type_id { get; set; } = 1;
        public string note { get; set; } = "";

        public bool allow_choose { get; set; } = false;
    }
}
