
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_table_group")]
    public   class TableGroupModel : CoreModel
    {
        public TableGroupModel()
        {
            tables = new List<TableModel>();
            table_group_screens = new List<TableGroupScreenModel>();
        }


        [Required(ErrorMessage = "Please select a station.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a station.")]
        public int outlet_id { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }

        private string _table_group_name_en;
        [MaxLength(50)]
        [Required(ErrorMessage = "Field cannot be blank.")]
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


        [MaxLength(50)]
        public string table_group_name_kh { get; set; }


        public string photo { get; set; }

        public List<TableModel> tables { get; set; }
        public List<TableGroupScreenModel> table_group_screens { get; set; }
    }
}
