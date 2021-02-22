using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eShareModel;

namespace eModels
{
    [Table("tbl_purchase_order_product")]
    public class PurchaseOrderProductModel : CoreModel
    {
        public PurchaseOrderProductModel()
        {
            
        }

        public bool is_inventory_product { get; set; }
        public bool is_fulfilled { get; set; }
        public int purchase_order_id { get; set; }
        [ForeignKey("purchase_order_id")]
        public PurchaseOrderModel purchase_order { get; set; }
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
           
        public int product_type_id { get; set; } = 1;
        [ForeignKey("product_type_id")]
        public ProductTypeModel product_type { get; set; }
         
        public bool is_allow_discount { get; set; } = true;

        public string note { get; set; }

        private decimal _multipler = 1;

        public decimal multiplier
        {
            get { return _multipler; }
            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                _multipler = value;

            }
        }

        public string unit { get; set; }

        private decimal _quantity = 1;
        public decimal quantity
        {
            get { return _quantity <= 0 ? 1 : _quantity; }
            set { _quantity = value; }
        }

        public decimal cost { get; set; }
        public decimal regular_price { get; set; }

        private decimal _selling_price;

        public decimal selling_price
        {
            get { return _selling_price; }
            set
            {
                _selling_price = value;
                if (value <= discount)
                {
                    discount = value;
                }
            }
        }


        private decimal _discount;

        public decimal discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                if (discount_type == "Percent" && (_discount > 100))
                {
                    _discount = (selling_price > 100 ? 100 : selling_price);
                }
                else if (discount_type != "Percent" && (_discount > selling_price))
                {
                    _discount = selling_price;
                }
            }
        }
        public decimal invoice_discount_amount { get; set; }
        public decimal grand_total { get; set; }

        private string _discount_type = "Percent";//  Percentage , Amount

        public string discount_type
        {
            get { return _discount_type; }
            set
            {
                _discount_type = value;
                if (value != "Percent" && discount > selling_price)
                {
                    discount = selling_price;
                }
                else if (value == "Percent" && discount > 100)
                {
                    discount = 100;
                }
            }
        }


        private decimal _sub_total;
        public decimal sub_total
        {
            get
            {
                _sub_total = quantity * cost;
                return _sub_total;
            }
            set { _sub_total = value; }
        }

        private decimal _total_discount;
        public decimal total_discount
        {
            get
            {
                if (discount_type == "Percent")
                    _total_discount = sub_total * discount / 100;
                else
                    _total_discount = discount;

                return _total_discount;
            }
            set { _total_discount = value; }
        }


        private decimal _total_amount;
        public decimal total_amount
        {
            get
            {
                _total_amount = sub_total - total_discount;
                return _total_amount;
            }
            set { _total_amount = value; }
        }



        private bool _is_add_note;
        [NotMapped, JsonIgnore]
        public bool is_add_note
        {
            get
            {
                return _is_add_note;
            }
            set { _is_add_note = value; }
        }


        [NotMapped, JsonIgnore]
        public bool is_can_delete { get; set; } = true;
    }

}
