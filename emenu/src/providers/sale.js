import { createToaster } from '@meforma/vue-toaster';	
const toaster = createToaster({position:'top'});
export default class Sale {
    constructor() {   
        this.tbl_number = null;
        this.table_id = null;
        this.price_rule = null;
        this.sale_type = '';
        this.customer = '';
        this.customer_photo = '';
        this.customer_name = '';
        this.exchange_rate = 1;   
        this.name = "";
        this.action = "";
        this.pos_receipt = undefined;  
        this.sale = {
            sale_products: []
        };
    } 


    onSaleOrderClick(){
        console.log("Sale Order Click ")
    }

    onAddtoCart(p,qty = 1, portion=null,modifiers=null){
        toaster.warning(p.name_en)
        console.log({"product":p,"portion":portion,"modifiers":modifiers})
    }
   
}
