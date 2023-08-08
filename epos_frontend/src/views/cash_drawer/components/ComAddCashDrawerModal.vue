<template>
    <ComModal @onClose="onClose(false)" @onOk="onOk()" :loading="cashEditResource.loading">
        <template #title>
            <div>{{params.name ?  `Edit ${params.name}` : $t(`New ${params.data.cash_type}`)}}</div>
        </template>
        <template #content>
     
            <div class="mb-2" v-if="cash">

                    <v-row>
                        <v-col cols="12" md="6">
                            <v-select height="100%" density="comfortable" :label="$t('Currency Type')"
                                v-model="cash.payment_type" 
                                :items="paymentTypeCash"
                                item-value="payment_method"
                                item-title="payment_method" hide-details hide-no-data variant="solo"
                                @update:modelValue="updateAmount"
                                ></v-select>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-text-field  
                            density="compact"
                            :label="$t('Input Amount')" 
                            v-model="cash.input_amount"
                            variant="solo"
                            append-inner-icon="mdi-keyboard"
                            @click:append-inner="OpenKeyboard()"
                            v-debounce="updateAmount" 
                            type="number"
                            hide-details
                            ></v-text-field>

                        </v-col>
                        <v-col cols="12" md="12">
                            <ComInput readonly v-model="cash.amount" type="number" :label="$t('Amount')" />
                        </v-col>
                        <v-col cols="12">
                            <ComInput keyboard type="textarea" :label="$t('Note')" v-model="cash.note" />
                        </v-col>
                    </v-row>
                </div>
        </template>
    </ComModal>
</template>
<script setup>
import { defineEmits, ref, createResource, createDocumentResource, createToaster,computed,inputNumberDialog,i18n } from '@/plugin'
import moment from '@/utils/moment.js';
import ComModal from '../../../components/ComModal.vue';

const { t: $t } = i18n.global;  

const emit = defineEmits(["resolve"])
const props = defineProps({
    params: Object
})

const toaster = createToaster({ position: 'top' })
const payment_types = JSON.parse(localStorage.getItem('setting')).payment_types;
const default_payment_type = JSON.parse(localStorage.getItem('setting')).default_payment_type;
const current_date = moment(new Date).format('DD-MM-YYYY');
const setting  = JSON.parse( localStorage.getItem("setting"));
const paymentTypeCash =computed(()=>  {
    return setting.payment_types.filter(r=>r.allow_cash_float == 1);
});

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
        //
    }
})
cashResource.value = createResource({
    url: 'frappe.client.insert',
})


function updateAmount(){
   
    if (payment_types.length > 0) {
        const exchange_rate = payment_types.find(r => r.payment_method == cash.value.payment_type).exchange_rate
        cash.value.amount = cash.value.input_amount / exchange_rate;
 
    }
}


async function OpenKeyboard(){
    const result =await inputNumberDialog({"title":$t('Enter Amount')});
    if(result){
     
    cash.value.input_amount = getNumber(result);
    updateAmount();
    }
}

function getNumber(val) {
    
        val = (val = val == null ? 0 : val)
        if (isNaN(val)) {
            return 0;
        }
        return parseFloat(val);
    }


function onClose(value) {
    emit('resolve', value);
}
function onOk() {
    if (cash.value.amount <= 0) {
        toaster.warning($t('msg.Amount cannot smaller than or equal zero'), {
            position: "top",
        });
        return
    }
    else if (!cash.value.note) {
        toaster.warning($t('msg.Please note your reason'), {
            position: "top",
        });
        return
    }
    if (!props.params.name) {
        createResource({
            url: "epos_restaurant_2023.api.api.get_current_shift_information",
            params: {
                business_branch: setting?.business_branch,
                pos_profile: localStorage.getItem("pos_profile")
            },
            auto: true,
            onSuccess(data) { 
                if (data.cashier_shift == null) {
                    toaster.warning($t('msg.Please start shift first'));
                    router.push({name:"OpenShift"});
                } else if(data.working_day==null){
                    toaster.warning($t('msg.Please start working day first'));
                    router.push({name:"StartWorkingDay"});
                }else{
                    onAddNew()
                }
            }
        })
    }
 
}
function onAddNew() {
    cash.value.doctype = 'Cash Transaction'
    cash.value.input_amount = parseFloat(cash.value.input_amount)
    cash.value.amount = parseFloat(cash.value.amount)
    cashResource.value.submit({ doc: cash.value }).then((res) => {
        toaster.success($t(`msg.Add is Successfully`, [$t(props.params.data.cash_type)]));
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
               
            },
            onSuccess(doc) {
                cash.value = doc;
            }
        })
    cash.value = cashEditResource.value.doc
}



</script>