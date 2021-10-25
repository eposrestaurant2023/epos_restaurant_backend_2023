using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{

    [Table("tbl_cash_drawer")]
    public class CashDrawerModel : CoreGUIDModel
    {
        public string cash_drawer_name { get; set; }

    }

}
