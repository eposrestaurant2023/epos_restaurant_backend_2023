using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_customer")]
    public class CustomerModel : CoreModel
    {
        [Required(ErrorMessage = "Please select a customer group.")]
        public Guid customer_group_id { get; set; }
        [ForeignKey("customer_group_id")]
        public CustomerGroupModel customer_group { get; set; }



    }
}
