using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_printer")]
    public class PrinterModel   : CoreModel
    {
        [MaxLength(50)]
        public string printer_name { get; set; }


    }
}
