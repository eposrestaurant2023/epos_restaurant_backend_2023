import Enumerable from 'linq'
import moment from '@/utils/moment.js';
import { noteDialog, printPreviewDialog,changeTaxSettingModal,SaleProductComboMenuGroupModal, keyboardDialog, createResource, createDocumentResource, addModifierDialog, useRouter, confirmDialog, saleProductDiscountDialog } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";
import { webserver_port } from "../../../../../sites/common_site_config.json"
import socket from '@/utils/socketio';


const toaster = createToaster({ position: "top" });

export default class Sale {
    constructor() {
        this.mobile = false;
        this.platform = {};
        this.promotion = null;
        this.working_day = "";
        this.cashier_shift = "";
        this.setting = null;
        this.orderBy = null;
        this.tbl_number = null;
        this.table_id = null;
        this.price_rule = null;
        this.sale_type = '';
        this.customer = '';
        this.customer_photo = '';
        this.customer_name = '';
        this.customer_group = '';
        this.exchange_rate = 1;
        this.guest_cover = 0;
        this.orderTime = undefined;
        this.router = useRouter();
        this.name = "";
        this.action = "";
        this.pos_receipt = undefined;
        this.no_loading = false;
        this.sale = {
            sale_products: []
        };
        this.vueInstance=null;
        this.vue=null;
        this.newSaleResource = null;
        this.saleResource = null;
        this.paymentInputNumber = "";
        this.isPrintReceipt = false;

        //use this variable to show toast after database submit in resource
        this.message = undefined;

        //user this variable to temporary store product that need to send to kitchen pritner 
        //before submit order or close order
        this.productPrinters = [];

        //temporary all deleted sale product, we use this for send data to kitchen printers
        this.deletedSaleProducts = []

        //tempporary store autditrail list and will submit to database after submit
        //auditrail login is store in tabComment
        this.auditTrailLogs = [];
        this.auditTrailResource = createResource({
            url: "frappe.client.insert"
        }
        );

        //use this resource to load sale list in current table number
        this.tableSaleListResource = null;
        this.orderChanged = false;
        this.printWaitingOrderAfterPayment = false;
        this.createNewSaleResource();

    }
   
    createNewSaleResource() {
        
        const parent = this;
        this.newSaleResource = createResource({
            url: "frappe.client.insert",
            onSuccess(doc) {

                parent.onProcessTaskAfterSubmit(doc);

                parent.action = "";
                if (parent.message != undefined) {
                    toaster.success(parent.message);
                    parent.message = undefined;
                }
                else {
                    toaster.success("Updated");
                }
            }
        })
    }

    async newSale() {
        const tax_rule = this.setting.tax_rule;
        this.orderChanged = false;
        this.sale = {
            doctype: "Sale",
            sale_status: "New",
            cashier_shift: this.cashier_shift,
            working_day: this.working_day,
            exchange_rate: this.exchange_rate,
            table_id: this.table_id,
            tbl_number: this.tbl_number,
            pos_profile: this.setting?.pos_profile,
            pos_station_name: localStorage.getItem("device_name"),
            customer: this.customer || this.setting?.customer,
            customer_photo: this.customer_photo || this.customer_name ? this.customer_photo : this.setting?.customer_photo,
            customer_name: this.customer_name || this.setting?.customer_name,
            customer_group: this.customer_group || this.setting?.customer_group,
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
            commission_amount: 0            
        } 
        console.log(this.vueGlobalProperties)
        this.onSaleApplyTax(tax_rule,this.sale);      
    }

    async LoadSaleData(name) {        
        return new Promise(async (resolve) => {
            const parent = this;
            this.saleResource = createDocumentResource({
                url: "frappe.client.get",
                doctype: "Sale",
                name: name,
                setValue: {
                    onSuccess(doc) {
                        parent.sale = doc;
                        parent.onProcessTaskAfterSubmit(doc);
                        parent.action = "";
                        if (parent.message != undefined) {
                            toaster.success(parent.message);
                            parent.message = undefined;
                        }
                        else {
                            toaster.success("Updated");
                        }
                    },
                },
            });

            await this.saleResource.get.fetch().then(async (doc) => {
                this.sale = doc;
                this.action = "";
                //check if current table dont hanve any sale list data then load it
                if (!this.tableSaleListResource?.data) {
                    this.getTableSaleList();
                }
                resolve(doc);
            });
            resolve(false);
        })
    }

    getTableSaleList() {
        const parent = this;
        this.tableSaleListResource = createResource({
            url: "frappe.client.get_list",
            params: {
                doctype: "Sale",
                fields: ["name", "creation", "grand_total", "total_quantity", "tbl_group", "tbl_number", "guest_cover", "grand_total", "sale_status", "sale_status_color", "sale_status_priority", "customer", "customer_name", "phone_number", "customer_photo"],
                filters: {
                    pos_profile: localStorage.getItem("pos_profile"),
                    table_id: JSON.parse(localStorage.getItem("table_groups")) && JSON.parse(localStorage.getItem("table_groups")).length > 0 ? parent.sale.table_id : '',
                    docstatus: 0
                },
                limit_page_length: 500,
            },
            auto: true,
        })
    }

    getSaleProductGroupByKey() {
        if (!this.sale.sale_products) {
            return []
        } else {
            const group = Enumerable.from(this.sale.sale_products).groupBy("{order_by:$.order_by,order_time:$.order_time}", "", "{order_by:$.order_by,order_time:$.order_time}", "$.order_by+','+$.order_time");
            return group.orderByDescending("$.order_time").toArray();
        }
    }

    getSaleProducts(groupByKey) {
        if (groupByKey) {
            return Enumerable.from(this.sale.sale_products).where(`$.order_by=='${groupByKey.order_by}' && $.order_time=='${groupByKey.order_time}'`).orderByDescending("$.modified").toArray()
        } else {
            return Enumerable.from(this.sale.sale_products).orderByDescending("$.modified").toArray()
        }
    }

    addSaleProduct(p) {
        //check for append quantity rule
        //product code, allow_append_qty,price, unit,modifier, portion, is_free,sale_product_status
        //and check system have feature to send to kitchen
        let strFilter = `$.product_code=='${p.name}' && $.append_quantity ==1 && $.price==${p.price} && $.portion=='${this.getString(p.portion)}'  && $.modifiers=='${p.modifiers}'  && $.unit=='${p.unit}' && $.is_free==0`

        if (!this.setting?.pos_setting?.allow_change_quantity_after_submit) {
            strFilter = strFilter + ` && $.sale_product_status == 'New'`
        }
        if(p.is_combo_menu && p.use_combo_group){
            strFilter = strFilter + ` && $.combo_menu_data == '${p.combo_group_data}'`
        }
        
        let sp = Enumerable.from(this.sale.sale_products).where(strFilter).firstOrDefault()

        if (sp != undefined) {
            sp.quantity = parseFloat(sp.quantity) + 1;
            this.clearSelected();
            sp.selected = true;
            this.updateSaleProduct(sp);
        } else {
            this.clearSelected();
            let tax_rule ="";    
            if((p.tax_rule||"")==""){
                tax_rule = JSON.parse(JSON.stringify(this.setting.tax_rule)) ;
            }
            else{
                tax_rule = JSON.parse(p.tax_rule_data);
            }

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
                discount_amount: 0,
                sale_discount_amount: 0,
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
                order_by: this.getOrderBy(),
                order_time: this.getOrderTime(),
                printers: p.printers,
                product_variants: [],
                is_combo_menu: p.is_combo_menu,
                use_combo_group: p.use_combo_group,
                combo_menu: p.combo_menu,
                combo_menu_data: (p.combo_menu_data || p.combo_group_data),
                product_tax_rule: p.tax_rule          
            }       
            this.onSaleProductApplyTax(tax_rule,saleProduct); 
            this.sale.sale_products.push(saleProduct);
            this.updateSaleProduct(saleProduct);
        }
        this.updateSaleSummary()
    }
    
    cloneSaleProduct(sp, quantity) {
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

    getOrderTime() {
        if (!this.orderTime) {
            this.orderTime = moment(new Date()).format('yyyy-MM-DD HH:mm:ss.SSS');
            return this.orderTime;
        } else {
            return this.orderTime
        }
    }

    getOrderBy() {
        if (!this.orderBy) {

            return JSON.parse(localStorage.getItem("current_user")).full_name;
        } else {
            return this.orderBy
        }
    }
    
    onSelectSaleProduct(sp) {
        this.clearSelected();
        sp.selected = true;
    }

    clearSelected() {
        Enumerable.from(this.sale.sale_products).where(`$.selected==true`).forEach("$.selected=false");
    }

    updateSaleProduct(sp) {
        //set property for re render comhappyhour check
        sp.is_render = false; 
        //end
        sp.sub_total = sp.quantity * sp.price + sp.quantity * sp.modifiers_price;
        sp.discount = parseFloat(sp.discount)
        if (sp.discount) {
            if (sp.discount_type == "Percent") {
                sp.discount_amount = (sp.sub_total * sp.discount / 100);
            } else {
                sp.discount_amount = sp.discount;
            }
            sp.sale_discount_percent = 0;
            sp.sale_discount_amount = 0;
        } else {
            sp.discount_amount = 0;
            //check if sale have discount then add discount to sale
        }
        if (sp.sale_discount_percent) {
            sp.sale_discount_amount = (sp.sub_total * sp.sale_discount_percent / 100);
        }
        sp.total_discount = sp.discount_amount + sp.sale_discount_amount;

        this.onCalculateTax(sp);
        sp.amount = sp.sub_total - sp.total_discount + sp.total_tax;

        //set property for re render comhappyhour check
        sp.is_render = true;
    }

    //on sale product apply tax setting
    onSaleProductApplyTax(tax_rule, sp){
        sp.tax_rule = tax_rule.name||"";
        sp.tax_1_rate = tax_rule.tax_1_rate||0;
        sp.percentage_of_price_to_calculate_tax_1 = tax_rule.percentage_of_price_to_calculate_tax_1||100;
        sp.calculate_tax_1_after_discount = tax_rule.calculate_tax_1_after_discount||false;

        sp.tax_2_rate = tax_rule.tax_2_rate||0;
        sp.percentage_of_price_to_calculate_tax_2 = tax_rule.percentage_of_price_to_calculate_tax_2||100;
        sp.calculate_tax_2_after_discount = tax_rule.calculate_tax_2_after_discount||false;
        sp.calculate_tax_2_after_adding_tax_1 = tax_rule.calculate_tax_2_after_adding_tax_1||false;

        sp.tax_3_rate = tax_rule.tax_3_rate||0;
        sp.percentage_of_price_to_calculate_tax_3 = tax_rule.percentage_of_price_to_calculate_tax_3||100;
        sp.calculate_tax_3_after_discount = tax_rule.calculate_tax_3_after_discount||false;
        sp.calculate_tax_3_after_adding_tax_1 = tax_rule.calculate_tax_3_after_adding_tax_1||false;
        sp.calculate_tax_3_after_adding_tax_2 = tax_rule.calculate_tax_3_after_adding_tax_2||false;


        this.updateSaleProduct(sp);
    }

    //on calculate tax
    onCalculateTax(sp){        

        //tax 1
        sp.taxable_amount_1 = sp.sub_total;     
        //tax 1 taxable amount
        //if cal tax1 taxable after disc.
        if(sp.calculate_tax_1_after_discount){
            sp.taxable_amount_1 = (sp.sub_total - sp.total_discount);
        }
        sp.taxable_amount_1 *= ((sp.percentage_of_price_to_calculate_tax_1 || 0)/100);

        //cal tax 1 amount
        sp.tax_1_amount = sp.taxable_amount_1 * ((sp.tax_1_rate ||0)/100);

        
        //tax 2
        //tax 2 taxable amount
        sp.taxable_amount_2 = sp.sub_total;
        //if cal tax2 taxable after disc.
        if(sp.calculate_tax_2_after_discount){
            sp.taxable_amount_2 = (sp.sub_total - sp.total_discount);
        }

        //if cal tax2 taxable after add tax1
        if(sp.calculate_tax_2_after_adding_tax_1){
            sp.taxable_amount_2 += sp.tax_1_amount;
        }
        sp.taxable_amount_2 *= ((sp.percentage_of_price_to_calculate_tax_2 || 0)/100);       

        //cal tax2 amount
        sp.tax_2_amount = sp.taxable_amount_2 * ((sp.tax_2_rate ||0)/100);


        //tax 3
        //tax 3 taxable amount
        sp.taxable_amount_3 = sp.sub_total;
        //if cal tax3 taxable after disc.
        if(sp.calculate_tax_3_after_discount){
            sp.taxable_amount_3 = (sp.sub_total - sp.total_discount);
        }
       
        //if cal tax3 taxable after add tax1
        if(sp.calculate_tax_3_after_adding_tax_1){
            sp.taxable_amount_3 += sp.tax_1_amount;
        }

        //if cal tax3 taxable after add tax2
        if(sp.calculate_tax_3_after_adding_tax_2){
            sp.taxable_amount_3 += sp.tax_2_amount;
        }
        sp.taxable_amount_3 *= ((sp.percentage_of_price_to_calculate_tax_3 ||0)/100);    

        //cal tax3 amount
        sp.tax_3_amount = sp.taxable_amount_3 * ((sp.tax_3_rate||0) /100);


        sp.total_tax = sp.tax_1_amount + sp.tax_2_amount + sp.tax_3_amount;

    }
    
    //on sale apply  tax setting
    onSaleApplyTax(tax_rule,s){
        s.tax_rule = tax_rule.name||"";
        s.tax_1_rate = tax_rule.tax_1_rate||0;
        s.percentage_of_price_to_calculate_tax_1 = tax_rule.percentage_of_price_to_calculate_tax_1||100;
        s.tax_2_rate  = tax_rule.tax_2_rate||0;
        s.percentage_of_price_to_calculate_tax_2 = tax_rule.percentage_of_price_to_calculate_tax_2||100;
        s.tax_3_rate = tax_rule.tax_3_rate||0;
        s.percentage_of_price_to_calculate_tax_3 = tax_rule.percentage_of_price_to_calculate_tax_3||100;

        //update tax setting of sale product
        (s.sale_products||[]).forEach((sp)=>{
            if((sp.product_tax_rule||"")==""){
                this.onSaleProductApplyTax(tax_rule,sp);
            }
        });        
    }

    //update sale summary
    updateSaleSummary(sale_status = '') {
        const sp = Enumerable.from(this.sale.sale_products);
        this.sale.total_quantity = this.getNumber(sp.sum("$.quantity"));
        this.sale.sub_total = this.getNumber(sp.sum("$.sub_total"));
        //calculate sale discount
        this.sale.sale_discountable_amount = this.getNumber(sp.where("$.allow_discount==1 && $.discount==0").sum("$.sub_total"));
        this.sale.discount = this.getNumber(this.sale.discount);
        this.sale.sale_discount = 0;
        if (this.sale.discount_type == "Percent") {
            this.sale.sale_discount = this.sale.sale_discountable_amount * (this.sale.discount / 100);
        } else {
            this.sale.sale_discount = this.sale.discount;
        }

        this.sale.product_discount = this.getNumber(sp.sum("$.discount_amount"));
        this.sale.total_discount = this.sale.product_discount + this.sale.sale_discount;

        //tax
        this.sale.tax_1_amount = this.getNumber(sp.sum("$.tax_1_amount"));
        this.sale.tax_2_amount = this.getNumber(sp.sum("$.tax_2_amount"));
        this.sale.tax_3_amount = this.getNumber(sp.sum("$.tax_3_amount"));
        this.sale.total_tax = this.getNumber(sp.sum("$.total_tax"));

        //grand_total
        this.sale.grand_total = (this.sale.sub_total - this.sale.total_discount) + this.sale.total_tax
        this.sale.balance = this.sale.grand_total;

        // commission
        if (this.sale.commission_type == "Percent") {
            this.sale.commission_amount = (this.sale.grand_total * this.sale.commission / 100);
        } else {
            this.sale.commission_amount = this.sale.commission;
        }

        this.orderChanged = true;

        socket.emit("ShowOrderInCustomerDisplay", this.sale, sale_status);
    }

    updateQuantity(sp, n) {
        sp.quantity = n;
        this.updateSaleProduct(sp)
        this.updateSaleSummary();
    }

    async onChangePrice(sp) {
        const result = await keyboardDialog({ title: "Change price", type: 'number', value: sp.price });
        if (result != false) {
            sp.price = parseFloat(this.getNumber(result));
            this.updateSaleProduct(sp);
            this.updateSaleSummary();
        }
    }
    
    async onChangeQuantity(sp, gv) {
        if (!this.isBillRequested()) {
            const result = await keyboardDialog({ title: "Change Quantity", type: 'number', value: sp.quantity });
            if (result) {
                let quantity = this.getNumber(result);
                if (this.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status == "New") {
                    if (quantity == 0) {
                        quantity = 1
                    }
                    this.updateQuantity(sp, quantity);
                } else {
                    sp.selected = false;
                    //do add record
                    if (quantity > sp.quantity) {
                        this.cloneSaleProduct(sp, quantity);
                    } else {
                        if (sp.quantity - quantity > 0) {
                            //do delete record
                            gv.authorize("delete_item_required_password", "delete_item", "delete_item_required_note", "Delete Item Note", "", false).then(async (v) => {
                                if (v) {
                                    sp.deleted_item_note = v.note;
                                    this.onRemoveSaleProduct(sp, sp.quantity - quantity);
                                }

                            });
                        }
                    } 
                }
            }
        }
    }

    async onSaleProductNote(sp) {
        if (!this.isBillRequested()) {
            const result = await noteDialog({ title: "Note", name: 'Items Note', data: sp });
            if (result != false) {
                sp.note = result
                socket.emit("ShowOrderInCustomerDisplay", this.sale);
            }
        }
    }

    async onSaleNote(p) {
        if (!this.isBillRequested()) {
            const result = await noteDialog({ title: "Note", name: 'Sale Note', data: p });
            if (result != false) {
                p.note = result
            }
        }
    }

    async onSaleProductFree(sp) {
        let freeQty = 0;
        const result = sp.quantity == 1 ? 1 : await keyboardDialog({ title: "Change Free Quantity", type: 'number', value: sp.quantity });
        if (result != false) {
            freeQty = parseFloat(this.getNumber(result));
            if (freeQty > sp.quantity) {
                freeQty = sp.quantity;
            }

            if (freeQty == sp.quantity) {
                sp.is_free = 1;
                sp.backup_modifier_price = sp.modifiers_price
                sp.backup_product_price = sp.price
                sp.price = 0;
                sp.modifiers_price = 0;
                this.updateSaleProduct(sp);
                this.updateSaleSummary();
            }
            else {
                let freeSaleProduct = JSON.parse(JSON.stringify(sp))
                freeSaleProduct.quantity = freeQty;
                freeSaleProduct.backup_product_price = sp.price
                freeSaleProduct.backup_modifier_price = sp.modifiers_price
                freeSaleProduct.price = 0;
                freeSaleProduct.modifiers_price = 0;
                freeSaleProduct.selected = false;
                freeSaleProduct.is_free = true;
                this.updateSaleProduct(freeSaleProduct);
                this.sale.sale_products.push(freeSaleProduct)

                //old record 

                sp.quantity = sp.quantity - freeQty;
                this.updateSaleProduct(sp);

            }

            this.updateSaleSummary();
        }

    }
    // change tax setting
    async onChangeTaxSetting(title, _tax_rule_name,_change_tax_setting_note,gv, sale_product){
        if(!this.isBillRequested()){
            
            // const tax_rule_name = tax_rule
            await  gv.authorize("change_tax_setting_required_password", "change_tax_setting", "change_tax_setting_required_note", "Change Tax Setting", "", true).then(async (v) => {
                if(v){ 
                    const resp = await changeTaxSettingModal({
                        title: title,
                        data: {
                            tax_rule: _tax_rule_name,
                            note: _change_tax_setting_note,
                            category_note_name: v.category_note_name
                        }
                    })
                    if(resp != false){ 
                        const _tax_rule = JSON.parse(resp.tax_rule.tax_rule_data);   
                        if(sale_product){ 
                            sale_product.product_tax_rule = _tax_rule.name;
                            this.onSaleProductApplyTax(_tax_rule,sale_product); 
                            // this.onCalculateTax(sale_product);
                            this.updateSaleProduct(sale_product);
                        }
                        else{
                            this.onSaleApplyTax(_tax_rule, this.sale);
                        }
                        this.updateSaleSummary(); 
                    }
                }
            });
        }
    }

    async  onSaleProductChangeTaxSetting(sp,gv){
        const resp = await this.onChangeTaxSetting("Change Tax Setting",sp.product_tax_rule,sp.change_tax_setting_note, gv,sp);     
    }
    
    async onDiscount(title, amount, discount_value, discount_type, discount_codes,discount_note, sp, category_note_name) {

        const result = await saleProductDiscountDialog({
            title: title,
            value: amount,
            data: {
                discount_value: discount_value,
                discount_type: discount_type,
                discount_codes: discount_codes,
                discount_note: discount_note,
                sale_product: sp,
                category_note_name: category_note_name
            }
        })
        if (result != false) {
            if (sp) {
                sp.discount = result.discount
                sp.discount_type = result.discount_type
                sp.discount_note = result.discount_note
                this.updateSaleProduct(sp)
            } else {
                this.sale.discount = result.discount
                this.sale.discount_type = result.discount_type
                this.sale.discount_note = result.discount_note
            }
            this.updateSaleSummary()
        }
    }
    async onSaleProductSetSeatNumber(sp) {
        if (!this.isBillRequested()) {
            const result = await keyboardDialog({ title: "Set Seat Number", type: 'number', value: sp.seat_number })
            if (result != false) {
                sp.seat_number = result
                socket.emit("ShowOrderInCustomerDisplay", this.sale);
            }
        }
    }

    onSaleProductCancelFree(sp) {
        if (!this.isBillRequested()) {
            sp.is_free = 0
            sp.price = sp.backup_product_price
            sp.modifiers_price = sp.backup_modifier_price
            sp.free_note = ''
            this.updateSaleProduct(sp)
            this.updateSaleSummary()
        }
    }
    getNumber(val) {
        val = (val = val == null ? 0 : val)
        if (isNaN(val)) {
            return 0;
        }
        return parseFloat(val);
    }
    getString(val) {
        val = (val = val == null ? "" : val)
        return val;
    }

    onRemoveSaleProduct(sp, quantity) {
        if (sp.quantity == quantity) {
            if (sp.sale_product_status == 'Submitted') {
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
            if (sp.sale_product_status == 'Submitted') {
                let deletedRecord = JSON.parse(JSON.stringify(sp))
                deletedRecord.quantity = quantity;
                this.deletedSaleProducts.push(deletedRecord)
            }
        }
        this.updateSaleSummary();
    }

    async OnEditSaleProduct(sp) {
        if(sp.is_combo_menu && sp.use_combo_group){
            const result = await SaleProductComboMenuGroupModal();
            if(result){
                if(result.combo_groups.length > 0){
                    let combo_menu_items = ''
                    if (result.combo_groups.length > 0) {
                        result.combo_groups.forEach(r => {
                            combo_menu_items = combo_menu_items + r.product_name + ' x' + r.quantity + ', '
                        });
                        sp.combo_menu = combo_menu_items.slice(0, combo_menu_items.length - 2)
                        sp.combo_menu_data = JSON.stringify(result.combo_groups)
                    }else{
                        sp.combo_menu = ''
                        sp.combo_group_data = '[]'
                    }
                }else{
                    sp.combo_menu = ''
                    sp.combo_group_data = "[]"
                }
                this.updateSaleProduct(sp);
                this.updateSaleSummary();
                toaster.success("Update sale product successfully")
            }
        }
        else{
            const result = await addModifierDialog();
            if (result) {
                if (result.portion != undefined) {
                    sp.portion = this.getString(result.portion.portion);
                    sp.price = this.getNumber(result.portion.price);
                }

                if (result.modifiers != undefined) {

                    sp.modifiers = this.getString(result.modifiers.modifiers);
                    sp.modifiers_price = this.getNumber(result.modifiers.price);
                    sp.modifiers_data = result.modifiers.modifiers_data;
                } else {
                    sp.modifiers = "";
                    sp.modifiers_price = 0;
                    sp.modifiers_data = "[]";
                }
                this.updateSaleProduct(sp);
                this.updateSaleSummary();
                toaster.success("Update sale product successfully")
            }
        }
    }

    onCheckPriceSmallerThanZero() {
        if (this.sale.sale_products.filter(r => r.amount < 0).length > 0) {
            toaster.warning("Product price cannot smaller than zero");
            return true
        }
        else if (this.sale.grand_total < 0) {
            toaster.warning("Sale price cannot smaller than zero");
            return true
        }
        else {
            return false
        }
    }

    onSubmit() {
        return new Promise(async (resolve) => {
            if (this.sale.sale_products.length == 0) {
                toaster.warning("Please select a menu item to submit order");
                resolve(false);
            }
            else if (this.onCheckPriceSmallerThanZero()) {
                resolve(false);
            }
            else {
                let doc = JSON.parse(JSON.stringify(this.sale));
                this.generateProductPrinters();
                if (this.sale.sale_status != "Hold Order") {
                    doc.sale_products.filter(r => r.sale_product_status == "New").forEach(x => {
                        x.sale_product_status = "Submitted";
                    })
                }
                if (this.getString(this.sale.name) == "") {
                    if (this.newSaleResource == null) {
                        this.createNewSaleResource();
                    }
                    await this.newSaleResource.submit({ doc: doc })

                } else {
                    await this.saleResource.setValue.submit(doc);
                }
                //refresh tabl 
                resolve(doc);
            }

        })

    }

    async onSubmitQuickPay() {
        return new Promise(async (resolve) => {
            if (this.sale.sale_products.length == 0) {
                toaster.warning("Please select a menu item to process payment");
                resolve(false);
            } else {

                if (await confirmDialog({ title: "Quick Pay", text: "Are you sure you process quick pay and close order?" })) {
                    this.sale.payment = [];
                    this.sale.payment.push({
                        payment_type: this.setting?.default_payment_type,
                        input_amount: this.sale.grand_total,
                        amount: this.sale.grand_total
                    })
                    this.sale.sale_status = "Submitted";
                    this.sale.docstatus = 1;
                    this.action = "quick_pay";
                    this.generateProductPrinters();
                    if (this.getString(this.sale.name) == "") {
                        if (this.newSaleResource == null) {
                            this.createNewSaleResource();
                        }
                        this.printWaitingOrderAfterPayment = true;
                        await this.newSaleResource.submit({ doc: this.sale })
                    } else {
                        await this.saleResource.setValue.submit(this.sale);
                    }
                    resolve(true);
                }
            }
        })
    }

    async onSubmitPayment(isPrint = true) {
        this.isPrintReceipt = isPrint;
        return new Promise(async (resolve) => {
            let balance = Number(this.sale.balance.toFixed(this.setting.pos_setting.main_currency_precision));

            if (balance > 0) {
                toaster.warning("Please enter all payment amount.");
                resolve(false);
            } else {
                if (await confirmDialog({ title: "Payment", text: "Are you sure you process payment and close order?" })) {

                    socket.emit("ShowOrderInCustomerDisplay", this.sale, "paid");


                    this.generateProductPrinters();


                    this.sale.sale_status = "Closed";
                    this.sale.docstatus = 1;
                    this.action = "payment";
                    if (this.getString(this.sale.name) == "") {
                        if (this.newSaleResource == null) {
                            this.createNewSaleResource();
                        }

                        this.printWaitingOrderAfterPayment = true;
                        await this.newSaleResource.submit({ doc: this.sale })
                    } else {
                        await this.saleResource.setValue.submit(this.sale);
                    }

                    resolve(true);
                }

            }





        })
    }

    onProcessTaskAfterSubmit(doc) {
        if (this.action == "submit_order") {
            this.onPrintToKitchen(doc);
            //print waiting doc

            if (this.setting.pos_setting.print_waiting_order_after_submit_order) {
                this.onPrintWaitingOrder(doc);

            }

        } else if (this.action == "print_bill") {

            if (this.pos_receipt == undefined || this.pos_receipt == null) {
                this.pos_receipt = this.setting?.default_pos_receipt;
            }
            this.onPrintReceipt(this.pos_receipt, "print_receipt", doc);
            this.onPrintToKitchen(doc);
        }
        else if (this.action == "quick_pay") {
            this.onPrintReceipt(this.setting?.default_pos_receipt, "print_receipt", doc);
            this.onPrintToKitchen(doc);

            if (this.printWaitingOrderAfterPayment) {

                this.onPrintWaitingOrder(doc);
            }

        } else if (this.action == "payment") {
            if (this.isPrintReceipt == true) {
                this.onPrintReceipt(this.pos_receipt, "print_receipt", doc);
            }
            //open cashdrawer
            if (localStorage.getItem("is_window") == "1") {
                window.chrome.webview.postMessage(JSON.stringify({ action: "open_cashdrawer" }));
            }
            this.onPrintToKitchen(doc);

            if (this.printWaitingOrderAfterPayment) {
                this.onPrintWaitingOrder(doc);
            }

        }

        this.submitToAuditTrail(doc);
        this.sale = {};

        this.orderTime = "";

        socket.emit("RefreshTable");

    }

    submitToAuditTrail(d) {
        this.auditTrailLogs.forEach((r) => {

            r.reference_name = d.name;
            this.auditTrailResource.submit({ doc: r })
        });
        this.auditTrailLogs = [];

    }

    onPrintToKitchen(doc) {
        const data = {
            action: "print_to_kitchen",
            setting: this.setting?.pos_setting,
            sale: doc,
            product_printers: this.productPrinters
        }

        if (localStorage.getItem("is_window") == 1) {
            window.chrome.webview.postMessage(JSON.stringify(data));
        } else {
            socket.emit("PrintReceipt", JSON.stringify(data))
        }
        this.productPrinters = [];
    }

    generateProductPrinters() {
        this.productPrinters = [];
        this.sale.sale_products.filter(r => r.sale_product_status == 'New' && JSON.parse(r.printers).length > 0).forEach((r) => {
            const pritners = JSON.parse(r.printers);
            pritners.forEach((p) => {
                this.productPrinters.push({
                    printer: p.printer,
                    group_item_type: p.group_item_type,
                    product_code: r.product_code,
                    product_name_en: r.product_name,
                    product_name_kh: r.product_name_kh,
                    portion: r.portion,
                    unit: r.unit,
                    modifiers: r.modifiers,
                    note: r.note,
                    quantity: r.quantity,
                    is_deleted: false,
                    is_free: r.is_free == 1,
                    combo_menu:r.combo_menu

                })
            });
        });

        //generate deleted product to product printer list
        this.deletedSaleProducts.filter(r => JSON.parse(r.printers).length > 0).forEach((r) => {
            const pritners = JSON.parse(r.printers);
            pritners.forEach((p) => {
                this.productPrinters.push({
                    printer: p.printer,
                    group_item_type: p.group_item_type,
                    product_code: r.product_code,
                    product_name_en: r.product_name,
                    product_name_kh: r.product_name_kh,
                    portion: r.portion,
                    unit: r.unit,
                    modifiers: r.modifiers,
                    note: r.note,
                    quantity: r.quantity,
                    is_deleted: true,
                    is_free: r.is_free == 1,
                    deleted_note: r.deleted_item_note
                })
            });
        });
    }


    getPrintReportPath(doctype, name, reportName, isPrint = 0) {
        let url = "";
        const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + webserver_port;
        url = serverUrl + "/printview?doctype=" + doctype + "&name=" + name + "&format=" + reportName + "&no_letterhead=0&letterhead=Defualt%20Letter%20Head&settings=%7B%7D&_lang=en&d=" + new Date()
        if (isPrint) {
            url = url + "&trigger_print=" + isPrint
        }
        return url;
    }

    async onPrintReceipt(receipt, action, doc) {
        const data = {
            action: action,
            print_setting: receipt,
            setting: this.setting?.pos_setting,
            sale: doc
        }
        if (receipt.pos_receipt_file_name && localStorage.getItem("is_window")) {
            window.chrome.webview.postMessage(JSON.stringify(data));
        } else {
            if (receipt.pos_receipt_file_name) {
                socket.emit('PrintReceipt', JSON.stringify(data));
            }
            else {
                this.onOpenBrowserPrint("Sale", doc.name, receipt.name)
            }
        }
    }

    onPrintWaitingOrder(doc) {
        if (this.setting.pos_setting.print_waiting_order_after_submit_order) {
            if (this.orderChanged) {
                const data = {
                    action: "print_waiting_order",
                    setting: this.setting?.pos_setting,
                    sale: doc
                }

                if (localStorage.getItem("is_window") == "1") {
                    window.chrome.webview.postMessage(JSON.stringify(data));
                } else {
                    socket.emit('PrintReceipt', JSON.stringify(data));
                }
                this.printWaitingOrderAfterPayment = false;
                this.orderChanged = false;
            }
        }
    }

    onOpenBrowserPrint(doctype, docname, filename) {
        window.open(this.getPrintReportPath(doctype, docname, filename, 1)).print();
        window.close();
    }

    isBillRequested() {
        if (this.sale.sale_status == 'Bill Requested') {
            toaster.warning("This sale order is already print bill. Please cancel print bill first.");
            return true;
        } else {
            return false;
        }
    }
    onAddPayment(paymentType, amount) {
        const single_payment_type = this.sale.payment.find(r => r.is_single_payment_type == 1);
        if (single_payment_type) {
            toaster.warning("You cannot add other payment type with " + single_payment_type.payment_type);
        } else {
            if (paymentType.is_single_payment_type == 1) {
                this.sale.payment = [];
                amount = parseFloat(this.sale.grand_total);

            }
            if (!this.getNumber(amount) == 0) {
                this.sale.payment.push({

                    payment_type: paymentType.payment_method,
                    input_amount: parseFloat(amount),
                    amount: parseFloat(amount / paymentType.exchange_rate),
                    exchange_rate: paymentType.exchange_rate,
                    currency: paymentType.currency,
                    is_single_payment_type: paymentType.is_single_payment_type,
                    required_customer: paymentType.required_customer
                });
                this.updatePaymentAmount();

                this.paymentInputNumber = this.sale.balance.toFixed(this.setting.pos_setting.main_currency_precision);

            } else {
                toaster.warning("Please enter payment amount");
            }
        }

    }
    updatePaymentAmount() {
        const payments = Enumerable.from(this.sale.payment);
        const total_payment = payments.sum("$.amount");
        this.sale.total_paid = total_payment;
        this.sale.balance = this.sale.grand_total - total_payment;

        if (this.sale.balance < 0) {
            this.sale.balance = 0;
        }

        this.sale.changed_amount = total_payment - this.sale.grand_total;
        this.sale.changed_amount = Number(this.sale.changed_amount.toFixed(this.setting.pos_setting.main_currency_precision));
        if (this.sale.changed_amount <= 0) {
            this.sale.changed_amount = 0;
        }

        this.action
    }
    isOrdered(message = "Please save or submit your current order."){
        if((this.sale.sale_products || []).length > 0 ){
            const sp = Enumerable.from(this.sale.sale_products);
            if (sp.where("$.name==undefined").toArray().length > 0) {
                toaster.warning(message);
                return true
            } 
        }
        return false
    }

    


}
