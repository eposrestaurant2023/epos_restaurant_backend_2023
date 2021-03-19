using System;
using System.ComponentModel.DataAnnotations;

namespace eShareModel
{
    public   class SaleOrderShareModel
    {
        [Key]
        public Guid id { get; set; }       
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
    }
}
