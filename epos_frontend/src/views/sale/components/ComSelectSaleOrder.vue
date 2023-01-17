<template>
    <v-dialog v-model="open" persistent width="960">
        <v-card class="mx-auto my-2 py-2 ">
            <v-card-item>
                <v-card-title>Select Sale Order - {{ table.tbl_no }}</v-card-title>

            </v-card-item>

            <v-card-text>
                <v-btn v-for="s in data" @click="openOrder(s.name)">
                    {{ s.name }} - {{ s.grand_total }} - {{ s.customer }} - {{ s.customer_name }}
                </v-btn>


            </v-card-text>
            <v-card-action class="text-right">
                <v-btn @click="onPrintAllBill">Print All Bill</v-btn>
                <v-btn>Quick Pay</v-btn>
                <v-btn>Quick Pay without Print</v-btn>
                <v-btn @click="onNewOrder">New Sale Order</v-btn>
                <v-btn @click="closeDialog(false)">Cancel</v-btn>
            </v-card-action>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { defineProps, ref, useStore, useRouter } from '@/plugin'
import { closeDialog, openDialog } from 'vue3-promise-dialog'
import ComInputNumber from '../../../components/ComInputNumber.vue';
const store = useStore()
const router = useRouter()

const props = defineProps({
    table: Object,
    data: {
        type: Object,
    }
})

const open = ref(true);
const setting = JSON.parse(localStorage.getItem("setting"));
function onPrintAllBill() {
    props.data.forEach((d) => {
        window.chrome.webview.postMessage(d);
    })
   // closeDialog(false)
}

function openOrder(sale_id){
    router.push({name:"AddSale",params:{name:sale_id}});
     closeDialog(false)
}

async function onNewOrder() {
    
    let guest_cover = 0;
   
    if (setting.use_guest_cover == 1) {
        let result = await openDialog(ComInputNumber, { title: "Guest Cover" });
        console.log(result)
        if (result) {
            guest_cover = parseInt(result);
            if (guest_cover == undefined || isNaN(guest_cover)) {
                guest_cover = 0;
            }
        } else {
            return;
        }
    }
 
  
    store.state.sale.sale.guest_cover = guest_cover;
    router.push({ name: "AddSale" });

}
</script>