<template>
    <v-dialog v-model="open" width="960">
        <v-card>

            <ComToolbar @onClose="onClose">
                <template #title>
                    Table No: {{ params.table.tbl_no }}
                </template>

            </ComToolbar>
            <v-card-text>
                
                <v-row no-gutters>
                    <v-col v-for="s in params.data">

                        <v-card :color="s.sale_status_color" class="pa-2 ma-2" @click="openOrder(s.name)">
                            <v-card-text>
                                {{ s.name }} <br /> <CurrencyFormat :value="s.grand_total" /> <br /> {{ s.customer }} - {{ s.customer_name }} 
                                <span v-if="s.guest_cover>0">
                                    ({{ s.guest_cover }})
                                </span>
                                <br/>
                                <Timeago   :long="long" :datetime="s.creation"/>
                               
                            </v-card-text>

                        </v-card>
                    </v-col>

                </v-row>




            </v-card-text>
            <v-card-action class="text-right">
                <v-btn @click="onPrintAllBill">Print All Bill</v-btn>
                <v-btn>Quick Pay</v-btn>
                <v-btn>Quick Pay without Print</v-btn>
                <v-btn @click="onNewOrder">New Sale Order</v-btn>
                <v-btn @click="onClose">Cancel</v-btn>
            </v-card-action>

        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, useStore, useRouter,keyboardDialog } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import { Timeago } from 'vue2-timeago'
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


const open = ref(true);
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