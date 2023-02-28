<template>
    <ComModal @onClose="onClose(false)" @onOk="onOk()" :loading="cashEditResource.loading">
        <template #title>
            <div>{{params.name ? `Edit ${params.name}` : `New ${params.data.cash_type}`}}</div>
        </template>
        <template #content>
            <div class="mb-2" v-if="cash">
                    <v-row>
                        <v-col cols="12" md="6">
                            <v-select v-if="paymentInCash.data" height="100%" density="comfortable" label="Currency Type"
                                v-model="cash.payment_type" :items="paymentInCash.data" item-value="payment_type"
                                item-title="payment_type" hide-details hide-no-data variant="solo"
                                @update:modelValue="updateAmount"
                                ></v-select>
                        </v-col>
                        <v-col cols="12" md="6">
                            <ComInput v-model="cash.input_amount" v-debounce="updateAmount" keyboard type="number" label="Input Amount" />
                        </v-col>
                        <v-col cols="12" md="12">
                            <ComInput disabled v-model="cash.amount" type="number" label="Amount" />
                        </v-col>
                        <v-col cols="12">
                            <ComInput keyboard type="textarea" label="Note" v-model="cash.note" />
                        </v-col>
                    </v-row>
                </div>
        </template>
    </ComModal>
</template>
<script setup>
import { defineEmits, ref, createResource, createDocumentResource, watch, createToaster,onUnmounted } from '@/plugin'
import moment from '@/utils/moment.js';
import ComModal from '../../../components/ComModal.vue';

const emit = defineEmits(["resolve"])
const props = defineProps({
    params: Object
})
let open = ref(true)
const toaster = createToaster({ position: 'top' })
const payment_types = JSON.parse(localStorage.getItem('setting')).payment_types;
const default_payment_type = JSON.parse(localStorage.getItem('setting')).default_payment_type;
const current_date = moment(new Date).format('DD-MM-YYYY');

const cashierShiftResource = ref({});
let cash = ref({
        transaction_status: props.params.data.cash_type,
        payment_type: default_payment_type,
        pos_profile: props.params.data.cashier_shift_info.pos_profile,
        cashier_shift: props.params.data.cashier_shift_info.name,
        working_day: props.params.data.cashier_shift_info.working_day,
        post_date: current_date,
        business_branch: props.params.data.cashier_shift_info.business_branch,
        created_by: '',
        input_amount: 0,
        amount: 0
    });
let cashResource = ref({})
let cashEditResource = ref({})
let paymentInCash = createResource({
    url: "epos_restaurant_2023.api.api.get_payment_cash",
    auto: true,
    params: {
        cashier_shift: props.params.data.cashier_shift_info.name
    },
    onSuccess(payment) {
        // new
        {
            console.log(payment)

        }
    }
})
cashResource.value = createResource({
    url: 'frappe.client.insert',
})
// if (props.params.name) {
//     LoadData() 
// } else {
//     cashResource.value = createResource({
//         url: 'frappe.client.insert',
//     })
// }

function updateAmount(){
    if (payment_types.length > 0) {
        const exchange_rate = payment_types.find(r => r.payment_method == cash.value.payment_type).exchange_rate
        cash.value.amount = cash.value.input_amount / exchange_rate;
 
    }
}
function onClose(value) {
    emit('resolve', value);
}
function onOk() {
    alert()
    if (cash.value.amount <= 0) {
        toaster.warning("Amount cannot smaller than or equal zero", {
            position: "top",
        });
        return
    }
    else if (!cash.value.note) {
        toaster.warning("Please note your reason.", {
            position: "top",
        });
        return
    }
    if (!props.params.name) {
        onAddNew()
    }
    // else {
    //     const saved = JSON.parse(JSON.stringify(cash.value))
    //     saved.input_amount = parseFloat(saved.input_amount)
    //     cashEditResource.value.setValue.submit(saved).then((res)=>{
    //         toaster.success(`Edit ${props.params.data.cash_type} is Successful`);
    //         onClose(true);
    //     });
        
    // }
}
function onAddNew() {
    cash.value.doctype = 'Cash Transaction'
    cash.value.input_amount = parseFloat(cash.value.input_amount)
    cash.value.amount = parseFloat(cash.value.amount)
    cashResource.value.submit({ doc: cash.value }).then((res) => {
        toaster.success(`Add ${props.params.name} is Successful`);
        onClose(true)
    })
}

async function LoadData() {
    cashEditResource.value = await createDocumentResource({
            url: "frappe.client.get",
            doctype: "Cash Transaction",
            name: props.params.name,
            auto: true,
            onError(err) {
                console.log(err)
            },
            onSuccess(doc) {
                cash.value = doc;
            }
        })
    cash.value = cashEditResource.value.doc
}
// onUnmounted(() => {
//     cash.value = {}
// })


</script>