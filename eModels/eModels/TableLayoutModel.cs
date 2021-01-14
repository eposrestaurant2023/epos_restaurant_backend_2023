using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_table_layout")]
    public  class TableLayoutModel   : CoreModel
    {
        [Required(ErrorMessage = "Please select a table group.")]
        public Guid table_group_id { get; set; }
        [ForeignKey("table_group_id")]
        public TableGroupModel table_group { get; set; }


        private string _table_name_layout_en;
        [MaxLength(50)]
        [Required(ErrorMessage = "Field cannot be blank.")]
        public string table_layout_name_en
        {
            get { return _table_name_layout_en; }
            set { _table_name_layout_en = value;
                if (string.IsNullOrEmpty(table_layout_name_kh))
                {
                    table_layout_name_kh = value;
                }
            }
        }

        [MaxLength(50)]
        public string table_layout_name_kh { get; set; }

        public string image_namge { get; set; }
    }
}
