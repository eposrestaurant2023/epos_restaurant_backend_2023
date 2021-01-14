using System;
using System.ComponentModel.DataAnnotations;

namespace eModels
{
    public  class CoreModel :KeyModel
    {
        [MaxLength(100)]
        public string created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;

        public bool is_deleted { get; set; } = false;

        [MaxLength(100)]
        public string deleted_by { get; set; }
        public DateTime? deleted_date { get; set; }  

        public bool status { get; set; } = true;
    }
}
