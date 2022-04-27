
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShareModel
{
    public   class CustomerCardShareModel    : CoreGUIDModel
    {
        public Guid customer_id { get; set; }
        public string card_name { get; set; }
        public string card_code { get; set; }
        public decimal  discount_value { get; set; }

        [Column(TypeName = "date")]
        public DateTime expiry_date { get; set; }
    }
}
