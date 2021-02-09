using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eModels
{
    [Table("tbl_stock_location")]
    public class StockLocationModel
    {
        [Key]
        public int id { get; set; }

        public int outlet_id { get; set; }
        [ForeignKey("outlet_id")]
        public OutletModel outlet { get; set; }
        public string stock_location_name { get; set; }
        public bool is_default { get; set; }

    }
}
