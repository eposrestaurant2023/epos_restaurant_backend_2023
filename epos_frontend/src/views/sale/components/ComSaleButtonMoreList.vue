<template>
     <ComLoadingDialog v-if="isLoading" />
    <v-list-item prepend-icon="mdi-format-list-bulleted" :title="($t('Reference')+' #')" @click="onReferenceNumber()" />
    <v-list-item prepend-icon="mdi-eye-outline" :title="$t('View Bill')" @click="onViewBill()" v-if="sale.sale.sale_products.length > 0"/>
    <template v-if="mobile">
        <v-list-item @click="onRemoveSaleNote()" v-if="sale.sale.note">
            <template v-slot:prepend>
                <v-icon icon="mdi-note-outline" color="error"></v-icon>
            </template>
            <v-list-item-title class="text-red-700">{{ $t('Remove Note') }}</v-list-item-title>
        </v-list-item>
        <v-list-item prepend-icon="mdi-note-outline" :title="$t('Note')" @click="sale.onSaleNote(sale.sale)" v-else />
    </template>
    <v-list-item prepend-icon="mdi-currency-usd" :title="$t('Commission')" @click="onAddCommission()" />
    <v-list-item prepend-icon="mdi-bulletin-board" :title="$t('Change Price Rule')" @click="onChangePriceRule()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-silverware" :title="$t('Change POS Menu')" @click="onChangePOSMenu()" />
    <v-list-item v-if="isWindow" prepend-icon="mdi-cash-100" :title="$t('Open Cash Drawer')" @click="onOpenCashDrawer()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-grid-large" :title="$t('Change or Merge Table')" @click="onChangeTable()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-cash-100" :title="$t('Split Bill')" @click="onSplitBill()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0 && setting.use_guest_cover == 1" prepend-icon="mdi-account-multiple-outline" :title="`${$t('Change Guest Cover')} (${sale.sale.guest_cover})`" @click="onUpdateGuestCover()" />
    <v-list-item prepend-icon="mdi-cart" :title="$t('Change Sale Type')" @click="onChangeSaleType()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-chair-school" :title="$t('Seat') + '#'" @click="onSeatNumber()" />
    <v-list-item prepend-icon="mdi-cash-100" :title="$t('Tax Setting')" @click="onChangeTaxSetting()" v-if="sale.setting.tax_rules.length > 0"/>
    <v-list-item v-if="sale.sale.sale_products?.filter(r=>r.name == undefined).length>0" @click="onClearOrder()">
        <template #prepend>
            <v-icon color="error" icon="mdi-autorenew"></v-icon>
        </template>
        <v-list-item-title class="text-orange-700">{{ $t('Cancel Order') }}</v-list-item-title>
    </v-list-item>
    <v-divider inset></v-divider>
    <v-list-item v-if="sale.sale.name" @click="onDeleteBill()">
        <template #prepend>
            <v-icon color="error" icon="mdi-delete"></v-icon>
        </template>
        <v-list-item-title class="text-red-700">{{ $t('Delete Bill') }} {{ showSplitBill }}</v-list-item-title>
    </v-list-item>
</template>
<script setup>
import { useRouter, splitBillDialog,addCommissionDialog,ComSaleReferenceNumberDialog,viewBillModelModel,ref, inject,confirm, createResource ,
    keyboardDialog, changeTableDialog, changePriceRuleDialog, changeSaleTypeModalDialog, createToaster, changePOSMenuDialog ,i18n} from "@/plugin"
import { useDisplay } from 'vuetify'
import ComLoadingDialog from '@/components/ComLoadingDialog.vue';
import socket from '@/utils/socketio';

const { t: $t } = i18n.global;  

const { mobile } = useDisplay()
const toaster = createToaster({ position: 'top' })
const router = useRouter();
const sale = inject('$sale')
const gv = inject('$gv')
const product = inject('$product')
const setting = JSON.parse(localStorage.getItem("setting"))
const isWindow = localStorage.getItem('is_window') == 1
const isLoading = ref(false);

let deletedSaleProducts = [];
let productPrinters =[];


async function onViewBill() {
    const result = await viewBillModelModel({})
}
async function onUpdateGuestCover() {
    if (setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: $t('Guest Cover'), type: 'number', value: sale.sale.guest_cover });

        if (typeof result != 'boolean' && result != false || result == 0) {

            sale.sale.guest_cover = parseInt(result);
            if (sale.sale.guest_cover == undefined || isNaN(sale.sale.guest_cover)) {
                sale.sale.guest_cover = 0;
            }
        } else {
            return;
        }
    }
}

async function onChangeTable() {
    if (!sale.isBillRequested()) {
        const result = await changeTableDialog({});
        if (result) {
            if (result.action == "reload_sale") {
                await sale.LoadSaleData(result.name);
            }
        }
    }
}
async function onChangePriceRule() {
    if (sale.sale.sale_status != 'New') {
        toaster.warning($t('msg.This bill is not new order'));
        return;
    }
    if (!sale.isBillRequested()) {
        const result = await changePriceRuleDialog({})
        if (result == true) {
            if(product.setting.pos_menus.length>0){
                product.loadPOSMenu()
            }else{
                product.getProductMenuByProductCategory(db,"All Product Categories")
            }
            
            window.postMessage("close_modal","*");
            toaster.success("msg.Change price rule successfully");
        }
    }
}
async function onChangePOSMenu() {
    const result = await changePOSMenuDialog({})
    if (result == true) {
        if(product.setting.pos_menus.length>0){
                product.loadPOSMenu()
            }else{
                product.loadPOSMenu()
                product.getProductMenuByProductCategory(db,"All Product Categories")
            }
        window.postMessage("close_modal","*");
        toaster.success("msg.Change POS Menu successfully");
        
    }

}
function onRemoveSaleNote() {
    sale.sale.note = ''
}
async function onChangeSaleType() {

    const result = await changeSaleTypeModalDialog({})
}

function onOpenCashDrawer() {
    gv.authorize("open_cashdrawer_require_password", "open_cashdrawer").then((v) => {
        if (v) {
            window.chrome.webview.postMessage(JSON.stringify({ action: "open_cashdrawer" }));
        }
    });
}
async function onSeatNumber(){ 
    const result = await keyboardDialog({ title: $t('Change Seat Number'), type: 'number', value: sale.sale.seat_number });

        if (typeof result == 'number') {

            sale.sale.seat_number = parseInt(result);
            if (sale.sale.seat_number == undefined || isNaN(sale.sale.seat_number)) {
                sale.sale.seat_number = 0;
            }

        } else {
            return;
        }
}
async function onReferenceNumber(){
    const reference_number = await ComSaleReferenceNumberDialog({
        data: sale.sale
    })
    if(typeof(reference_number) != 'boolean')
        sale.sale.reference_number = reference_number
}
async function onDeleteBill() {
    //check authorize and     check reason 
    gv.authorize("delete_bill_required_password", "delete_bill", "delete_bill_required_note", "Delete Bill Note").then(async (v) => {
        if (v) {
            if(v.show_confirm==1){
                if(await confirm({title:$t('Delete Sale Order'), text:'msg.are you sure to delete this sale order'}) == false){
                    window.postMessage("close_modal","*");
                    return;
                }
            }
          
            //cancel payment first
            isLoading.value = true;

            //send deleted sale product to temp deleted
            const _sale = JSON.parse(JSON.stringify(sale.sale));
            generateSaleProductPrintToKitchen(_sale,v.note);
            

            const deleteSaleResource = createResource({
                url:"epos_restaurant_2023.api.api.delete_sale",
                params:{
                    name:sale.sale.name,
                    auth:{full_name:v.user, username:v.username, note:v.note}
                },
                onError(err){
                    isLoading.value = false;
                }
            });

            await deleteSaleResource.fetch().then((v)=>{
                isLoading.value = false;
                toaster.success("msg.Delete sale order successfully");
                //print to kitchen
                onProcessPrintToKitchen(_sale);

                sale.newSale();
                if (sale.setting.table_groups.length > 0) {
                    router.push({ name: 'TableLayout' });
                }else {
                    router.push({ name: "AddSale" });
                }               
            })           
        }
    })
}


function generateSaleProductPrintToKitchen(doc,note){
    deletedSaleProducts = [];
    (doc.sale_products||[]).forEach((sp)=>{
        if(sp.sale_product_status=="Submitted"){
            sp.note = note;
            sp.deleted_item_note = "Bill Deleted";
            deletedSaleProducts.push(sp);
        }
    });

    //generate deleted product to product printer list
    deletedSaleProducts.filter(r => JSON.parse(r.printers).length > 0).forEach((r) => {
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


function onProcessPrintToKitchen(doc){
    const data = {
            action: "print_to_kitchen",
            setting: setting?.pos_setting,
            sale: doc,
            product_printers: productPrinters
        }

        if (localStorage.getItem("is_window") == 1) {
            window.chrome.webview.postMessage(JSON.stringify(data));
        } else {
            socket.emit("PrintReceipt", JSON.stringify(data))
        }
        deletedSaleProducts = [];
        productPrinters = [];
}

async function onClearOrder(){
    if(await confirm({title:$t('Cancel sale order'), text:'msg.are your sure to cancel this sale order'})){
        const sale_products = JSON.parse(JSON.stringify(sale.sale.sale_products.filter(r=>r.name != undefined))); 
    sale.sale.sale_products = sale_products || [];
    sale.updateSaleSummary();
    //add to audit trail log 
    //future update
    
    }
    
     
}

async function onAddCommission(){
    if(!sale.isBillRequested()) {
        const result = await addCommissionDialog({ title: 'title', name: 'Sale Commission', data: sale.sale });
        if (result != false) { 
            sale.sale = result.data
        }
    }
}


//split bills method
async function onSplitBill(){
    if(!sale.isBillRequested()) {
        if(sale.sale.sale_products.length == 0){
            toaster.warning("msg.Please select a menu item to submit order");
            return;
        }
        else if(sale.sale.sale_status != 'Submitted' || sale.sale.sale_products.find(r=>r.sale_product_status != 'Submitted')){
            toaster.warning($t('msg.please save or submit your current order first',[$t('Submit')]))
        }else{
            const res = await splitBillDialog({ title: $t('Split Bill'), name: 'Split Bill', data: sale.sale });     
            if (res != false) {  
                sale.getTableSaleList()
            }
        }
        
    }
    
}

async function onChangeTaxSetting(){
    const resp = await sale.onChangeTaxSetting($t('Change Tax Setting'),sale.sale.tax_rule,sale.sale.change_tax_setting_note,gv );     
}


</script>