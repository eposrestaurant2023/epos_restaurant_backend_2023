using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eShareModel
{
    public class SaleShareModel : CoreGUIDModel
    {     

        public string document_number { get; set; } = "New";
        [Column(TypeName = "date")]
        public DateTime sale_date { get; set; } = DateTime.Now;
        public string customer_note { get; set; }
        public string reference_number { get; set; }
        public string term_conditions { get; set; }
        public string sale_note { get; set; }
        public bool is_partially_paid { get; set; }
        public bool is_fulfilled { get; set; }
        public bool is_over_due { get; set; }

        [Column(TypeName = "date")]
        public DateTime? due_date { get; set; }      

        [NotMapped, JsonIgnore]
        public bool can_delete
        {
            get
            {
                //return paid_amount == 0 && !is_paid && !is_deleted && total_visited==0; 
                return !is_deleted && !is_fulfilled;
            }
        }
        [NotMapped, JsonIgnore]
        public bool can_restore
        {
            get
            {
                return is_deleted;
            }
        }

        [NotMapped, JsonIgnore]
        public bool can_edit
        {
            get
            {
                return !is_deleted;
            }
        }

    }


}
