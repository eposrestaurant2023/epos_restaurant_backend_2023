<template>
    <v-dialog v-model="open" persistent style="max-width: 800px;">
        <v-card>
            <v-toolbar color="default" title="Notice">
                <v-toolbar-items>
                    <v-btn icon @click="onClose()">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </v-toolbar-items>
            </v-toolbar>
            <v-card-text>
                <div class="mb-2">
                    <v-row v-if="cashierShiftResource.doc">
                        <!-- <v-col md="6">
                            <v-text-field label="Working Day" v-model="cashierShiftResource.doc.working_day" variant="solo"
                                readonly hide-details></v-text-field>
                        </v-col>
                        <v-col md="6">
                            <v-text-field label="Cashier Shift" v-model="cashierShiftResource.doc.name" variant="solo"
                                readonly hide-details></v-text-field>
                        </v-col>

                        <v-col md="6">
                            <v-text-field hide-details label="Close Date" v-model="current_date" variant="solo"
                                readonly></v-text-field>
                        </v-col>
                        <v-col md="6">
                            <v-text-field hide-details label="POS Profile" v-model="cashierShiftResource.doc.pos_profile"
                                variant="solo" readonly></v-text-field>
                        </v-col> -->
                        <v-col md="6">
                            <v-select
                                height="100%"
                                density="comfortable"
                                label="Payment Type"
                                v-model="cash.payment_type"
                                :items="paymentInCash.data"
                                item-value="payment_type"
                                item-title="payment_type"
                                @click="onSelectPaymentType"
                                hide-details 
                                hide-no-data
                                clearable
                                variant="solo"
                            ></v-select> 
                        </v-col>
                        <v-col md="6">
                            <v-card>
                                <v-card-subtitle class="!pt-1 !pb-0 !text-xs">Cash Drawer Balance</v-card-subtitle>
                                <v-card-text class="!pt-0 !pb-1">
                                    <div style="font-size: 17px;" v-if="paymentInCash.data.find(r=>r.payment_type == cash.payment_type)">
                                        <CurrencyFormat :value="paymentInCash.data.find(r=>r.payment_type == cash.payment_type).payment_amount ?? 0"/>
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-text-field hide-details label="Input Amount" v-model="cash.input_amount"
                                variant="solo"></v-text-field>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-text-field hide-details label="Amount" v-model="cash.amount"
                                variant="solo" readonly></v-text-field>
                        </v-col>
                        <v-col cols="12"> 
                            <ComInput type="textarea" v-model="cash.created_by" label="Created By"/>
                        </v-col>
                        <v-col cols="12">
                            <ComInput type="textarea" label="Note" v-model="cash.note"/>
                        </v-col>
                    </v-row>
                </div>
                <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" variant="flat" @click="onClose(false)" color="error">
                            Close
                        </v-btn>
                        <v-btn variant="flat" @click="onOK()" color="primary">
                            OK
                        </v-btn>
                    </div>
                </div>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { defineEmits, ref, createResource, onMounted, createDocumentResource} from '@/plugin'
import moment from '@/utils/moment.js';
const emit = defineEmits(['resolve'])
const props = defineProps({
    params: Object
})

let open = true
const setting = JSON.parse(localStorage.getItem("setting"))
const current_date = moment(new Date).format('DD-MM-YYYY');
let doc = ref({
    closed_note: "",
    is_closed: 1,
    closed_date: moment(new Date()).format('YYYY-MM-DD HH:mm:ss'),
    cash_float: []
})

const cashierShiftResource = ref({});
let cash = ref({});
let paymentInCash = ref({
    input_amount: 0,
    amount: 0
});

 

let cashierShiftInfo = createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    onSuccess(doc){
        paymentInCash = createResource({
            url: "epos_restaurant_2023.api.api.get_payment_cash",
            auto:true,
            params: {
                cashier_shift: doc.name
            },
            onSuccess(payment){
                if(1 != 2){
                    if(payment.length > 0){
                        cash.value.payment_type = payment[0].payment_type
                    }
                        
                }
            }
        })
    }
 
});

onMounted(async () => {
    await cashierShiftInfo.fetch(); 
    if (!cashierShiftInfo.data) {
        toaster.warning("There's no cashier shift opened", {
            position: "top",
        });
        router.push({ name: "Home" });

    }
    cashierShiftResource.value = createDocumentResource({
        url: 'frappe.client.get',
        doctype: 'Cashier Shift',
        name: cashierShiftInfo.data.name,
        setValue: {
            async onSuccess(doc) {
                //
            },

        },

    });

})
function onSelectPaymentType($event){
    console.log($event)
}
function onClose() {
    emit('resolve', false)
}
function onOK() {
    emit('resolve', true)
}

</script>