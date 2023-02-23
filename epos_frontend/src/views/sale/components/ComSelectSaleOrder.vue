
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
                <v-btn variant="flat" color="primary" @click="onPrintAllBill">Print All Bill</v-btn>
                <v-btn variant="flat" color="primary">Quick Pay</v-btn>
                <v-btn variant="flat" color="primary">Quick Pay without Print</v-btn>
                <v-btn variant="flat" color="success" @click="onNewOrder">New Sale Order</v-btn>
                <v-btn variant="flat" color="error" @click="onClose">Cancel</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, useStore, useRouter,keyboardDialog } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import { useDisplay } from 'vuetify'
import ComSaleListItem from './ComSaleListItem.vue';
 
 const { mobile } = useDisplay()
const store = useStore()
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
function onPrintAllBill() {
    props.params.data.forEach((d) => {
        window.chrome.webview.postMessage(d);
    })

}

function openOrder(sale_id) {
    router.push({ name: "AddSale", params: { name: sale_id } });
    emit('resolve', false);
}

async function onNewOrder() {

    let guest_cover = 0;

    if (setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: "Guest Cover", value: guest_cover, type: 'number' });

        if (result || String(result) == "") {
            guest_cover = parseInt(result);
            if (guest_cover == undefined || isNaN(guest_cover)) {
                guest_cover = 0;
            }
        } else {
            return;
        }
    }


    store.state.sale.sale.guest_cover = guest_cover;
    emit('resolve', false)
    router.push({ name: "AddSale" });

}
function onClose() {
    emit("resolve", false);
}
</script>