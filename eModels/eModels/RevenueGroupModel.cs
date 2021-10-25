using eShareModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace eModels
{
    [Table("tbl_revenue_group")]
  public  class RevenueGroupModel: CoreGUIDModel
    {
        public string revenue_group_name_en { get; set; }
        public string revenue_group_name_kh { get; set; }
        public int sort_order { get; set; }
    }
}
