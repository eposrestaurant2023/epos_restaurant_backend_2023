
<template>
    <v-dialog :fullscreen="mobile" v-model="open" @update:modelValue="onClose" :style="mobile ? '' : 'width: 100%;max-width:1200px'"> 
        <v-card>
            <ComToolbar @onClose="onClose">
                <template #title>
                    Table No: {{ params.table.tbl_no }}
                </template>
            </ComToolbar>
            <v-card-text class="p-2">
                <ComPlaceholder :is-not-empty="params.data.length > 0">
                    <v-row class="!-m-1">
                        <v-col class="!p-0" cols="12" md="6" v-for="(s, index) in params.data" :key="index">
                        <ComSaleListItem :sale="s" @click="openOrder(s.name)"/>
                        </v-col>
                    </v-row> 
                </ComPlaceholder>
            </v-card-text>
            <v-card-actions class="justify-end">
                <ComPrintButton doctype="Sale" title="Print All Bill" @onPrint="onPrintAllBill"/>
                 
                <v-btn variant="flat" color="primary">Quick Pay</v-btn>
                <v-btn variant="flat" color="primary">Quick Pay without Print</v-btn>
                <v-btn variant="flat" color="success" @click="onNewOrder">New Sale Order</v-btn>
                <v-btn variant="flat" color="error" @click="onClose">Cancel</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { inject, ref, useStore, useRouter,keyboardDialog,createDocumentResource } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import { useDisplay } from 'vuetify'
import ComSaleListItem from './ComSaleListItem.vue';
import ComPrintButton from '@/components/ComPrintButton.vue';
 
 const { mobile } = useDisplay()
const store = useStore()
const gv = inject("$gv")
const router = useRouter()

const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
//parameter list
// table: Object,
// data:Object

 

const emit = defineEmits(["resolve"])


let open = ref(true);
const setting = JSON.parse(localStorage.getItem("setting"));
function onPrintAllBill(r) {
   
    props.params.data.forEach((d) => {
        createDocumentResource({
            url: 'frappe.client.get',
            doctype: 'Sale',
            name:d.name,
            onSuccess(d){
               onPrintReceipt(r,d)
            },
            auto:true
        })
        
    })

}

function onPrintReceipt(r, doc){

if (doc.sale_products.length >0){
    const data = {
                action: "print_receipt",
                print_setting: r,
                setting: gv.setting?.pos_setting,
                sale: doc
            }
            if(localStorage.getItem("is_window")=="1"){ 
                window.chrome.webview.postMessage(JSON.stringify(data));
            }

 
  }
}

function openOrder(sale_id) {
    router.push({ name: "AddSale", params: { name: sale_id } });
    emit('resolve', false);
}

async function onNewOrder() {

    emit('resolve', {action:"new_sale"});
  

}
function onClose() {
    emit("resolve", false);
}
</script>