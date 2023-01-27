import Enumerable from 'linq'
import { keyboardDialog,createResource,createDocumentResource,addModifierDialog,inject} from "@/plugin"
const setting = JSON.parse(localStorage.getItem("setting"))
export default class Sale {
	constructor() {
		this.sale = {
            sale_products:[]
        };

        this.newSaleResource= createResource({
            url:"frappe.client.insert",
            onSuccess(doc){
                alert("save done");
            }
        })
        // this.saleResource = createDocumentResource({
        //     url: "frappe.client.get",
        //     doctype: "Sale",
        //     name: "SO2023-0227",
        //     onSuccess(doc){
        //         sale = doc;
        //     },
        //     setValue: {
        //         onSuccess() {
        //             alert("update done")
        //         },
        //         onError() {

        //         },
        //       },
        //     auto: true
        // })
	}
    newSale(){
     
        this.sale={
            doctype:"Sale",
            pos_profile : setting.pos_profile,
            customer : setting.customer,
            customer_photo : setting.customer_photo,
            customer_name : setting.customer_name,
            working_day : "WD2023-0003",
            cashier_shift: "CS2023-0007",
            price_rule : setting.price_rule,
            sale_products : []
        }
    }

    getSaleProducts(){
        return  Enumerable.from(this.sale.sale_products).orderByDescending("$.modified")
    }

    addSaleProduct(p) {        
        //check for append quantity rule
        //product code, allow_append_qty,price, unit,modifier, portion, is_free,sale_product_status
        //and check system have feature to send to kitchen
 
        let strFilter=`$.product_code=='${p.name}' && $.append_quantity ==1 && $.price==${p.price} && $.portion=='${this.getString(p.portion)}'  && $.modifiers=='${p.modifiers}'  && $.unit=='${p.unit}'  && $.is_free==0`
        if (!setting.allow_change_quantity_after_submit){
            strFilter = strFilter + ` && $.sale_product_status == 'New'`
        }
        let sp = Enumerable.from(this.sale.sale_products).where(strFilter).firstOrDefault()
         

        if (sp != undefined) {
             sp.quantity =parseFloat( sp.quantity) + 1;
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
                price: p.price,
                modifiers_price : this.getNumber(p.modifiers_price),
                product_photo: p.photo,
                selected:true,
                modified:(new Date()),
                append_quantity:p.append_quantity,
                allow_discount:p.allow_discount,
                allow_free:p.allow_free,
                allow_change_price:p.allow_change_price,
                is_open_product:p.is_open_product,
                portion:this.getString(p.portion),
                modifiers:p.modifiers,
                modifiers_data:p.modifiers_data,
                is_free:0,
                sale_product_status :"New"
            }
            this.sale.sale_products.push(saleProduct);
            
            this.updateSaleProduct(saleProduct);
        }
         this.updateSaleSummary()
    }

    onSelectSaleProduct(sp){
        this.clearSelected();
        sp.selected = true;
    }

    clearSelected(){
        Enumerable.from(this.sale.sale_products).where(`$.selected==true`).forEach("$.selected=false");
    }
    updateSaleProduct(sp){
       
        sp.sub_total = sp.quantity * sp.price + sp.quantity * sp.modifiers_price;
        sp.total_discount = 0
        sp.total_tax = 0
        sp.amount = sp.sub_total - sp.total_discount + sp.total_tax;
        
    }   

    updateSaleSummary(){
        const sp = this.sale.sale_products ;
        this.sale.total_quantity = this.getNumber(Enumerable.from(sp).sum("$.quantity"));
        this.sale.sub_total =  this.getNumber(Enumerable.from(sp).sum("$.sub_total"));
        this.sale.product_discount =  this.getNumber(Enumerable.from(sp).sum("$.discount_amount"));
        this.sale.total_discount = this.getNumber(this.sale.product_discount + this.sale.sale_discount);

        //tax
        this.sale.tax_1_amount =  this.getNumber(Enumerable.from(sp).sum("$.tax_1_amount"));
        this.sale.tax_2_amount =  this.getNumber(Enumerable.from(sp).sum("$.tax_2_amount"));
        this.sale.tax_3_amount =  this.getNumber(Enumerable.from(sp).sum("$.tax_3_amount"));
        this.sale.total_tax =this.getNumber(  Enumerable.from(sp).sum("$.total_tax"));

        //grand_total
        this.sale.grand_total =  (this.sale.sub_total - this.sale.total_discount) + this.sale.total_tax 
    }

    updateQuantity(sp, n){
        sp.quantity = n;
        this.updateSaleProduct(sp)
        this.updateSaleSummary();
    }
    async onChangePrice(sp){
        const result = await keyboardDialog({title:"Change price", type:'number', value: sp.price});
        if (result != false){
            sp.price = parseFloat( this.getNumber(result));    
            this.updateSaleProduct();
            this.updateSaleSummary();
        }
        
    }
    async onChangeQuantity(sp){
        const result = await keyboardDialog({title:"Change Quantity", type:'number', value: sp.quantity});
        if (result != false){
            sp.quantity = parseFloat( this.getNumber(result));    
            this.updateSaleProduct();
            this.updateSaleSummary();
        }
        
    }
    async onSaleProductNote(sp){
        const result = await keyboardDialog({title:"Notice", type:'text', value: sp.note});

        if (result != false){
            sp.note = result
        }
        
    }
    async onSaleProductFree(sp){
        let freeQty = sp.quantity
        const result = await keyboardDialog({title:"Change Free Quantity", type:'number', value: freeQty});
        if (result != false){

            freeQty = parseFloat( this.getNumber(result));
            if(freeQty > sp.quantity)
                reeQty = sp.quantity;
            console.log(sp.quantity)
            console.log(freeQty)
            if(freeQty < sp.quantity){
                sp.quantity = sp.quantity - freeQty;
                alert(sp.quantity)
                const freeSaleProduct = sp;
                freeSaleProduct.quantity = freeQty;
                freeSaleProduct.free = true
                this.addSaleProduct(freeSaleProduct)
            }
            else{
                sp.free = true
            }
            this.updateSaleProduct(sp);
            this.updateSaleSummary();
        }
    }
    getNumber(val) {
        val =  (val = val == null ? 0 : val)
        if (isNaN(val)){
          return 0;
        }
        return parseFloat(val);
    }
    getString(val){
        val =  (val = val == null ? "" : val)
        return val;
    }

    onRemoveSaleProduct(sp,quantity){
        if(sp.quantity==quantity){ 
            this.sale.sale_products.splice(this.sale.sale_products.indexOf(sp), 1);
        }else {
            sp.quantity = sp.quantity-quantity;
        }
    }
    

    async OnEditSaleProduct(sp){
        product.setSelectedProductByMenuID(sp.menu_product_name)
        await addModifierDialog({});
        
    }

    submit(){
       this.newSaleResource.submit({doc:this.sale})
       //this.saleResource.setValue.submit({pos_station_name:"xx"})
    }

}
