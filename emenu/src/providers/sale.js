import { createToaster } from '@meforma/vue-toaster';	
const toaster = createToaster({position:'top'});
export default class Sale {
    constructor() {   
        this.working_day={};
		this.cashier_shift={};
        this.default_customer = {};
        this.tbl_number = null;
        this.table_id = null; 
        this.sale_type = ''; 
        this.setting = {}

        this.sale = {
            sale_products: []
        };
    }
    
    async newSale() { 
        const tax_rule = this.setting.pos_profile.tax_rule;
        this.sale = {
            doctype: "Sale",
            sale_status: "New",
            cashier_shift: this.cashier_shift.name,
            shift_name:this.cashier_shift.shift_name,
            working_day: this.working_day.name,
            exchange_rate: this.setting.setting.exchange_rate,

            table_id: this.table_id,
            tbl_number: this.tbl_number,
            pos_profile: this.setting.pos_profile.name,
            // pos_station_name: localStorage.getItem("device_name"),
            customer: this.default_customer.name,
            customer_photo: this.default_customer.customer_photo,
            customer_name: this.default_customer.customer_name_en,
            customer_group: this.default_customer.customer_group,
            price_rule: this.price_rule || this.setting?.price_rule,
            business_branch: this.setting?.business_branch,
            sale_products: [],
            product_variants: [],
            sale_type: this.sale_type || this.setting?.default_sale_type,
            discount_type: "Percent",
            grand_total: 0,
            guest_cover: this.guest_cover,
            discount: 0,
            sub_total: 0,
            payment: [],
            posting_date: moment(new Date()).format('yyyy-MM-DD'),
            commission_type: "Percent",
            commission: 0,
            commission_note: '',
            commission_amount: 0,
            created_by:make_order_auth.name     
        }  
        this.onSaleApplyTax(tax_rule,this.sale); 
    }


    onSaleOrderClick(){
        console.log("Sale Order Click ")
    }

    onAddtoCart(p,qty = 1, portion=null,modifiers=null){
        toaster.warning(p.name_en)
        console.log({"product":p,"portion":portion,"modifiers":modifiers})
    }



   
}
