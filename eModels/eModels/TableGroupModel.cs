
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
            table_layout = new TableLayoutModel();
        }


        [Required(ErrorMessage = "Please select a station.")]
        public Guid station_id { get; set; }
        [ForeignKey("station_id")]
        public StationModel station { get; set; }


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

        public List<TableModel> tables { get; set; }

        public TableLayoutModel table_layout { get; set; }
    }
}
