using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
 
    [Table("tbl_expense")]
    public class ExpenseModel:eShareModel.ExpenseShareModel
    {
        public ExpenseModel()
        {
            histories = new List<HistoryModel>();
            attach_files = new List<AttachFilesModel>();
        }
        public List<HistoryModel> histories { get; set; }
        public List<AttachFilesModel> attach_files { get; set; }
    }
}
