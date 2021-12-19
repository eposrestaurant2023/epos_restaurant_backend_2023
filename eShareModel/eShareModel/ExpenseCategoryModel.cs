using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
    [Table("tbl_expense_category")]
    public class ExpenseCategoryModel : CoreGUIDModel
    {
        public string expense_category_name { get; set; }
        public int sort_order { get; set; }
    }

   [Table("tbl_expense_item")]
   public class ExpenseItemModel:CoreGUIDModel
   {
        public Guid expense_category_id { get; set; }
        [ForeignKey("expense_category_id")]
        public ExpenseCategoryModel expense_category { get; set; }
        public string expense_item_name { get; set; }
        public int sort_order { get; set; }
   }


}
