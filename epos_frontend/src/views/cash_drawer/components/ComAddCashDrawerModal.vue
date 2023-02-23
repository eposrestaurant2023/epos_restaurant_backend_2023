<template>
    <v-dialog v-model="open" style="max-width: 800px;">
        <v-card>
            <v-toolbar color="default" :title="params.name">
                <v-toolbar-items>
                    <v-btn icon @click="onClose()">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </v-toolbar-items>
            </v-toolbar>
            <v-card-text>
                <div class="mb-2">
                    <v-row v-if="cashierShiftResource.doc">
                        <v-col md="6">
                            <v-select
                                v-if="paymentInCash.data"
                                height="100%"
                                density="comfortable"
                                label="Payment Type"
                                v-model="cash.payment_type"
                                :items="paymentInCash.data"
                                item-value="payment_type"
                                item-title="payment_type"
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
                                    <div style="font-size: 17px;" v-if="paymentInCash.data">
                                        <CurrencyFormat :value="paymentInCash.data.find(r=>r.payment_type == cash.payment_type).payment_amount ?? 0"/>
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                        <v-col cols="12" md="6">
                            <ComInput v-model="cash.input_amount" keyboard type="number" label="Input Amount"/>
                        </v-col>
                        <v-col cols="12" md="6">
                            <ComInput disabled v-model="cash.amount" label="Amount"/>
                        </v-col>
                        <v-col cols="12">
                            <ComInput keyboard type="textarea" label="Note" v-model="cash.note"/>
                        </v-col>
                    </v-row>
                </div>
                <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" variant="flat" @click="onClose(false)" color="error">
                            Close
                        </v-btn>
                        <v-btn variant="flat" @click="onOK()" color="primary">
                            Save
                        </v-btn>
                    </div>
                </div>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { defineEmits, ref, createResource, onMounted, createDocumentResource, watch} from '@/plugin'
const emit = defineEmits(["resolve"])
const props = defineProps({
    params: Object
})
let open = true
const payment_types = JSON.parse(localStorage.getItem('setting')).payment_types;
const cashierShiftResource = ref({});
let cash = ref({
    transaction_status: props.params.name,
    input_amount: 0,
    amount: 0,
    created_by: ''
});
let paymentInCash = ref({});
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
watch(cash.value , (currentValue,oldValue) => { 
        if(payment_types.length > 0){
            const exchange_rate = payment_types.find(r=>r.payment_method == cash.value.payment_type).exchange_rate
            cash.value.amount = cash.value.input_amount / exchange_rate;
        }
 
    
})
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


function onClose() {
    emit('resolve',false);
}
function onOK() {
    emit('resolve', true)
}

</script>