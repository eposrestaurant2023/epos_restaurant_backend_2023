
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_table_group")]
    public   class TableGroupModel : CoreNoIdentityModel
    {
        public TableGroupModel()
        {
            tables = new List<TableModel>();
        }
        public int outlet_id { get; set; }

        private string _table_group_name_en;

        public string table_group_name_en
        {
            get { return _table_group_name_en; }
            set { _table_group_name_en = value;
                if (string.IsNullOrEmpty(table_group_name_kh))
                {
                    table_group_name_kh = value;
                }
            }
        }
        public string table_group_name_kh { get; set; }
        public string photo { get; set; }
        public List<TableModel> tables { get; set; }
    }
}
