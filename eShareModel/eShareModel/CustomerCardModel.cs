using System.Text.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eShareModel
{
    public   class CustomerCardShareModel    : CoreGUIDModel
    {
        public Guid customer_id { get; set; }
        public string card_name { get; set; }
        public string card_code { get; set; }
        public string discount_type { get; set; } = "Percent";
        public decimal  discount_value { get; set; }

        [Column(TypeName = "date")]
        public DateTime expiry_date { get; set; } = DateTime.Now.AddMonths(1).AddDays(-1);

        [NotMapped,JsonIgnore]
        public bool is_expired
        { get 
            { 
                return expiry_date <= DateTime.Now;
            }
        }
        
    }
}
