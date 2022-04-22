using eShareModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace eAPIClient.Models
{
    [Table("tbl_customer_card")]
    public class CustomerCardModel    : CustomerCardShareModel
    {
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }
    }
}
