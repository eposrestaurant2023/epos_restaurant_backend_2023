using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    [Table("tbl_customer_group")]
    public  class CustomerGroupModel   : CoreModel
    {
        public CustomerGroupModel()
        {
            customers = new List<CustomerModel>();
        }



        private string _customer_group_name_en;
        [Required(ErrorMessage ="Field cannot be blank.")]  
        [MaxLength(50)]
        public string customer_group_name_en
        {
            get { return _customer_group_name_en; }
            set { _customer_group_name_en = value;
                if (string.IsNullOrEmpty(customer_group_name_kh))
                {
                    customer_group_name_kh = value;
                }
            }
        }

        [MaxLength(50)]
        public string customer_group_name_kh { get; set; }



        public List<CustomerModel> customers { get; set; }

       
    }
}
