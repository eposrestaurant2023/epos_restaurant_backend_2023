using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShareModel
{
    public class BusinessBranchModel : CoreGUIDModel
    {
        public BusinessBranchModel()
        {
           
        }

        private string _business_branch_name_en;

        [Required(ErrorMessage = "Field cannot be blank.")]
        [MaxLength(100)]
        public string business_branch_name_en
        {
            get { return _business_branch_name_en; }
            set
            {
                _business_branch_name_en = value;
                if (string.IsNullOrEmpty(business_branch_name_kh))
                {
                    business_branch_name_kh = value;
                }
            }
        }

    

        [MaxLength(100)]
        public string business_branch_name_kh { get; set; }

        public string address_en { get; set; }
        public string address_kh { get; set; }

        [MaxLength(50)]
        public string email { get; set; }

        [MaxLength(50)]
        public string phone_1 { get; set; }

        [MaxLength(50)]
        public string phone_2 { get; set; }

        [MaxLength(50)]
        public string website { get; set; }
 

        public string logo { get; set; }
        public string note { get; set; }
        public string color { get; set; }





    }
}
