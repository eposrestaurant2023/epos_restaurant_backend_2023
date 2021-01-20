﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_table")]
    public  class TableModel  : CoreModel
    {
        [Required(ErrorMessage = "Please select a table group.")]
        public int table_group_id { get; set; }
        [ForeignKey("table_group_id")]
        public TableGroupModel table_group { get; set; }

        [MaxLength(50)]
        public string table_name { get; set; }


        public double position_x_percent { get; set; }
        public double position_y_percent { get; set; }

    }
}
