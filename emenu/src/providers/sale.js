 
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
    async newSale() { 
        this.sale = {
            doctype: "Sale",
            sale_status: "New",
            table_id: '',
            tbl_number: '',
            customer: '',
            customer_photo: '',
            customer_name: '',
            price_rule: '',
            business_branch: this.setting?.business_branch,
            sale_products: [],
            discount_type: "Percent",
            grand_total: 0,
            discount: 0,
            sub_total: 0,
            payment:[],
            posting_date: moment(new Date()).format('yyyy-MM-DD')
            
        }
       
    }

    addSaleProduct(p) {
        //check for append quantity rule
        //product code, allow_append_qty,price, unit,modifier, portion, is_free,sale_product_status
        //and check system have feature to send to kitchen
        
        let strFilter = `$.product_code=='${p.name}' && $.append_quantity ==1 && $.price==${p.price} && $.portion=='${this.getString(p.portion)}'  && $.modifiers=='${p.modifiers}'  && $.unit=='${p.unit}'  && $.is_free==0`
    
        if (!this.setting?.pos_setting?.allow_change_quantity_after_submit) {
            strFilter = strFilter + ` && $.sale_product_status == 'New'`
        }
        let sp = Enumerable.from(this.sale.sale_products).where(strFilter).firstOrDefault()
        if (sp != undefined) {
            sp.quantity = parseFloat(sp.quantity) + 1;
            this.clearSelected();
            sp.selected = true;
            this.updateSaleProduct(sp);
        } else {
            this.clearSelected(); 
            var saleProduct = {
                menu_product_name: p.menu_product_name,
                product_code: p.name,
                product_name: p.name_en,
                product_name_kh: p.name_kh,
                unit: p.unit,
                quantity: 1,
                sub_total: 0,
                total_discount: 0,
                total_tax: 0,
                discount_amount:0,
                sale_discount_amount:0,
                note: '',
                price: p.price,
                modifiers_price: this.getNumber(p.modifiers_price),
                product_photo: p.photo,
                selected: true,
                modified: moment(new Date()).format('yyyy-MM-DD HH:mm:ss.SSS'),
                append_quantity: p.append_quantity,
                allow_discount: p.allow_discount,
                allow_free: p.allow_free,
                allow_change_price: p.allow_change_price,
                is_open_product: p.is_open_product,
                portion: this.getString(p.portion),
                modifiers: p.modifiers || '',
                modifiers_data: p.modifiers_data,
                is_free: 0,
                sale_product_status: "New",
                discount_type: "Percent",
                discount: 0,
                // order_by : this.getOrderBy(),
                // order_time : this.getOrderTime()
            }
            this.sale.sale_products.push(saleProduct);

            this.updateSaleProduct(saleProduct);
        }
        this.updateSaleSummary()
    }



    
    cloneSaleProduct(sp,quantity){
        this.clearSelected();
        const sp_copy = JSON.parse(JSON.stringify(sp));
        sp_copy.selected = true;
        sp_copy.quantity = quantity - sp_copy.quantity;
        sp_copy.sale_product_status = "New";
        sp_copy.name = "";
        sp_copy.order_by = this.orderBy;
        sp_copy.order_time = this.getOrderTime();
        this.updateSaleProduct(sp_copy);
        this.sale.sale_products.push(sp_copy);
        this.updateSaleSummary()
  
        

    }

    // getOrderTime(){
    //     if(!this.orderTime){   
    //         this.orderTime=  moment(new Date()).format('yyyy-MM-DD HH:mm:ss.SSS');
    //         return this.orderTime;
    //     }else {
    //         return this.orderTime
    //     }
        
    // }

    
    // getOrderBy(){
    //     if(!this.orderBy){   
            
    //         return JSON.parse( localStorage.getItem("current_user")).full_name;
    //     }else {
    //         return this.orderBy
    //     }
        
    // }




    onSelectSaleProduct(sp) {
        this.clearSelected();
        sp.selected = true;
    }

    clearSelected() {
        Enumerable.from(this.sale.sale_products).where(`$.selected==true`).forEach("$.selected=false");
    }
    updateSaleProduct(sp) {
        sp.sub_total = sp.quantity * sp.price + sp.quantity * sp.modifiers_price;
        sp.discount = parseFloat(sp.discount)
        if(sp.discount){ 
            if (sp.discount_type=="Percent"){
                sp.discount_amount = (sp.sub_total * sp.discount/100); 
            }else {
                sp.discount_amount = sp.discount;
            }
            sp.sale_discount_percent = 0;
            sp.sale_discount_amount = 0;
        }else {
            sp.discount_amount = 0;
            //check if sale have discount then add discount to sale
            
        }
        if(sp.sale_discount_percent){
            sp.sale_discount_amount = (sp.sub_total * sp.sale_discount_percent/100); 
             
        }
        sp.total_discount = sp.discount_amount + sp.sale_discount_amount;
        sp.total_tax = 0
        sp.amount = sp.sub_total - sp.total_discount + sp.total_tax;
    }

    updateSaleSummary() {
        const sp = Enumerable.from(this.sale.sale_products);
        this.sale.total_quantity = this.getNumber(sp.sum("$.quantity"));
        this.sale.sub_total = this.getNumber(sp.sum("$.sub_total"));
        //calculate sale discount
        this.sale.sale_discountable_amount = this.getNumber(sp.where("$.allow_discount==1 && $.discount==0").sum("$.sub_total"));
        this.sale.discount = this.getNumber(this.sale.discount);
        this.sale.sale_discount = 0;
        if (this.sale.discount_type=="Percent"){  
            this.sale.sale_discount = this.sale.sale_discountable_amount * (this.sale.discount / 100);
        }else {
            this.sale.sale_discount = this.sale.discount;
        }
        
        this.sale.product_discount = this.getNumber(sp.sum("$.discount_amount"));
        this.sale.total_discount = this.sale.product_discount + this.sale.sale_discount;
    }

    updateQuantity(sp, n) {
        sp.quantity = n;
        this.updateSaleProduct(sp)
        this.updateSaleSummary();
    }

    onRemoveSaleProduct(sp, quantity) { 
        if (sp.quantity == quantity) {
            if(sp.sale_product_status=='Submitted'){
                this.deletedSaleProducts.push(sp)
                // this.auditTrailLogs.push({
                //     doctype:"Comment",
                //     subject:"Delete Sale Product",
                //     comment_type:"Comment",
                //     reference_doctype:"Sale",
                //     reference_name:"New",
                //     comment_by:"cashier@mail.com",
                //     content:`User sengho delete sale prodcut. Product Name: ABC 001, Qty:1, Amount:15.50, Reason:Wrong Order`
                // })
            }
            
            this.sale.sale_products.splice(this.sale.sale_products.indexOf(sp), 1);
        } else {
            sp.quantity = sp.quantity - quantity;

            if(sp.sale_product_status=='Submitted'){
                let deletedRecord = JSON.parse(JSON.stringify(sp))
               
             

                deletedRecord.quantity = quantity;
            
                this.deletedSaleProducts.push(deletedRecord)
            }
           
            
        }
        this.updateSaleSummary();
        
    }
    updatePaymentAmount(){
        const payments = Enumerable.from(this.sale.payment);
        const total_payment  = payments.sum("$.amount") ;
        this.sale.total_paid = total_payment ;
        this.sale.balance =  this.sale.grand_total - total_payment  ;
        
        if(this.sale.balance<0){
            this.sale.balance = 0;
        }
        
        this.sale.changed_amount = total_payment - this.sale.grand_total;
        this.sale.changed_amount = Number(this.sale.changed_amount.toFixed(this.setting.pos_setting.main_currency_precision));
        if(this.sale.changed_amount<=0){
            this.sale.changed_amount = 0;
        }

        this.action
    }

   
}
