<template>
     <ComLoadingDialog v-if="isLoading" />
    <v-list-item prepend-icon="mdi-eye-outline" title="View Bill" @click="onViewBill()" v-if="sale.sale.sale_products.length > 0"/>
    <template v-if="mobile">
        <v-list-item @click="onRemoveSaleNote()" v-if="sale.sale.note">
            <template v-slot:prepend>
                <v-icon icon="mdi-note-outline" color="error"></v-icon>
            </template>
            <v-list-item-title class="text-red-700">Remove Note</v-list-item-title>
        </v-list-item>
        <v-list-item prepend-icon="mdi-note-outline" title="Note" @click="sale.onSaleNote(sale.sale)" v-else />
    </template>
    <v-list-item prepend-icon="mdi-bulletin-board" title="Change Price Rule" @click="onChangePriceRule()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-silverware" title="Change POS Menu" @click="onChangePOSMenu()" />
    <v-list-item v-if="isWindow" prepend-icon="mdi-cash-100" title="Open Cash Drawer" @click="onOpenCashDrawer()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-grid-large" title="Change/Merge Table" @click="onChangeTable()" />
    <!-- <v-list-item prepend-icon="mdi-cash-100" title="Merge Table/Bill" @click="onViewInvoice()"/> -->
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-cash-100" title="Split Bill" @click="onViewInvoice()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-account-multiple-outline" :title="`Change Guest Cover (${sale.sale.guest_cover})`" @click="onUpdateGuestCover()" />
    <v-list-item prepend-icon="mdi-cart" title="Change Sale Type" @click="onChangeSaleType()" />
    <v-list-item prepend-icon="mdi-cash-100" title="Tax Setting" @click="onViewInvoice()" />
    <v-list-item v-if="setting.table_groups && setting.table_groups.length > 0" prepend-icon="mdi-chair-school" title="Seat#" @click="onSeatNumber()" />
    
    <v-list-item v-if="sale.sale.sale_products?.filter(r=>r.name == undefined).length>0" @click="onClearOrder()">
        <template #prepend>
            <v-icon color="error" icon="mdi-autorenew"></v-icon>
        </template>
        <v-list-item-title class="text-orange-700">Cancel Order</v-list-item-title>
    </v-list-item>
    <v-divider inset></v-divider>
    <v-list-item v-if="sale.sale.name" @click="onDeleteBill()">
        <template #prepend>
            <v-icon color="error" icon="mdi-delete"></v-icon>
        </template>
        <v-list-item-title class="text-red-700">Delete Bill</v-list-item-title>
    </v-list-item>
</template>
<script setup>
import { useRouter, viewBillModelModel,ref, inject,confirm, keyboardDialog, changeTableDialog, changePriceRuleDialog, changeSaleTypeModalDialog, createToaster, changePOSMenuDialog ,createResource } from "@/plugin"
import { useDisplay } from 'vuetify'
import ComLoadingDialog from '@/components/ComLoadingDialog.vue';
const { mobile } = useDisplay()
const toaster = createToaster({ position: 'top' })
const router = useRouter();
const sale = inject('$sale')
const gv = inject('$gv')
const product = inject('$product')
const setting = JSON.parse(localStorage.getItem("setting"))
const isWindow = localStorage.getItem('is_window') == 1
const isLoading = ref(false);


async function onViewBill() {
    const result = await viewBillModelModel({})
}
async function onUpdateGuestCover() {
    if (setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: "Guest Cover", type: 'number', value: sale.sale.guest_cover });

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
        toaster.warning("This sale order is not new order.");
        return;
    }
    if (!sale.isBillRequested()) {
        const result = await changePriceRuleDialog({})
        if (result == true) {
            product.loadPOSMenu()
            toaster.success("Price Rule Was Change Successfull");
        }
    }
}
async function onChangePOSMenu() {

    const result = await changePOSMenuDialog({})
    if (result == true) {
        product.loadPOSMenu()
        toaster.success("POS Menu Was Change Successfull");
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
    const result = await keyboardDialog({ title: "Change Seat Number", type: 'number', value: sale.sale.seat_number });

        if (typeof result == 'number') {

            sale.sale.seat_number = parseInt(result);
            if (sale.sale.seat_number == undefined || isNaN(sale.sale.seat_number)) {
                sale.sale.seat_number = 0;
            }

        } else {
            return;
        }
}
async function onDeleteBill() {
    //check authorize and     check reason
 
    gv.authorize("delete_bill_required_password", "delete_bill", "delete_bill_required_note", "Delete Bill Note").then(async (v) => {
        if (v) {
            if(v.show_confirm==1){
                if(await confirm({title:'Delete Sale Order', text:'Are you sure you want delete this sale order?'}) == false){
                    return;
                }
            }
          
            //cancel payment first
            isLoading.value = true;
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
                toaster.success("Delete sale order successfully");
                sale.newSale();
                if (sale.setting.table_groups.length > 0) {
                    router.push({ name: "setting" });
                }else {
                    router.push({ name: "AddSale" });
                }
               
            })


           
        }
    })

}

async function onClearOrder(){
    if(await confirm({title:'Cancel sale order', text:'Are your sure you want to cancel this sale order?'})){
        const sale_products = JSON.parse(JSON.stringify(sale.sale.sale_products.filter(r=>r.name != undefined))); 
    sale.sale.sale_products = sale_products || [];
    sale.updateSaleSummary();
    //add to audit trail log 
    //future update
    
    }
    
     
}


</script>