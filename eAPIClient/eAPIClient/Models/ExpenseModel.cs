using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eAPIClient.Models
{
    [Table("tbl_expense")]
    public class ExpenseModel:eShareModel.ExpenseShareModel
    {
    }
}
