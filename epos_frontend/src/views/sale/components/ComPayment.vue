<template>
    <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" :hide-close-button="true" :hide-ok-button="true"
        :fill="true" contentClass="h-full">
        <template #title>
            {{ $t('Payment') }}
        </template>
        <template #bar_custom>
            <ComSelectPaymentPrinter @onClick="onSelectedReceipt" :selected="selectedReceipt.name" v-if="mobile" />
        </template>
        <template #content>
            <div v-if="mobile" class="!p-1 overflow-auto">
                <ComSmallSalePayment />
            </div>
            <div v-else class="!px-0 !pt-1 !pb-0 overflow-hidden h-full">
                <v-row class="!m-0 h-full">
                    <v-col class="!p-1 h-full" md="4">
                        <ComPaymentInputNumber />
                    </v-col>
                    <v-col class="!p-1 h-full" md="4">
                        <div class="overflow-auto h-full">
                            <ComSalePaymentMethodList />
                        </div>
                    </v-col>
                    <v-col class="!p-0 h-full" md="4">
                        <div class="bg-gray-100 p-1 h-full">
                            <div class="grid h-full" style="grid-template-rows: max-content;">
                                <ComSalePaymentGrandTotalInformation />
                                <div class="overflow-auto h-full">
                                    <ComSalePaymentList />
                                </div>
                                <ComPaymentSummaryInformation />
                            </div>
                        </div>
                    </v-col>
                </v-row>
            </div>
        </template>
        <template #action>
            <v-row class="!m-0">
                <v-col class="!p-0" cols="12" md="4">
                    <div class="h-full flex items-center" v-if="!mobile">
                        <ComSelectPaymentPrinter @onClick="onSelectedReceipt" :selected="selectedReceipt.name" />
                    </div>
                    <div v-else>
                        <ComPaymentSummaryInformation />
                    </div>
                </v-col>
                <v-col class="!p-0" cols="12" md="4">
                    <div  v-if="gv.setting.show_button_tip==1" class="border rounded-sm px-2 py-4 text-center cursor-pointer bg-orange-100 hover:bg-orange-300 flex justify-center items-center m-1" @click="onTipPressed">
                        <span v-if="((sale.sale.tip_amount||0)<=0)">{{ $t('TIP') }}</span>
                        <span v-else>{{ $t('Remove TIP') }}</span>
                    </div>
                </v-col>
                <v-col class="!p-0" cols="12" md="4">
                    <v-row class="!m-0">
                        <v-col class="!p-0" cols="6">
                            <div class="p-1">

                                <v-btn size="small" class="w-full" color="primary" @click="onPayment" stacked
                                    prepend-icon="mdi-printer">
                                    <span>{{ $t('Payment with Print') }}</span>
                                </v-btn>
                            </div>
                        </v-col>
                        <v-col class="!p-0" cols="6">
                            <div class="p-1">
                                <v-btn size="small" class="w-full" color="primary" @click="onPaymentWithoutPrint" stacked
                                    prepend-icon="mdi-currency-usd">{{ $t('Payment') }}</v-btn>

                            </div>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
        </template>
    </ComModal>
</template>
<script setup>

import { inject, ref, onUnmounted,onMounted,i18n } from '@/plugin';
import ComPaymentInputNumber from "./ComPaymentInputNumber.vue"
import ComSmallSalePayment from "./mobile_screen/ComSmallSalePayment.vue"
import { useDisplay } from 'vuetify'
import ComSalePaymentMethodList from './ComSalePaymentMethodList.vue';
import ComSalePaymentGrandTotalInformation from './ComSalePaymentGrandTotalInformation.vue';
import ComSalePaymentList from './ComSalePaymentList.vue';
import ComSelectPaymentPrinter from './ComSelectPaymentPrinter.vue';
import ComPaymentSummaryInformation from './ComPaymentSummaryInformation.vue';
import { createToaster } from '@meforma/vue-toaster';

const { t: $t } = i18n.global;  

const { mobile } = useDisplay()

const sale = inject("$sale")
const gv = inject("$gv")

const emit = defineEmits(['resolve'])
const toaster = createToaster({ position: "top" })
const props = defineProps({
    params: Object
})


let backup = ref({})
const selectedReceipt = ref({})
selectedReceipt.value = gv.setting.default_pos_receipt;

sale.paymentInputNumber = sale.sale?.grand_total.toFixed(sale.setting.pos_setting.main_currency_precision);

function onSelectedReceipt(r) {
    selectedReceipt.value = r;
}



onMounted(() => { 
    sale.is_payment_first_load = true;
    backup.value = JSON.parse(JSON.stringify(sale.sale));
})

function onClose() {    
    sale.sale.total_paid = backup.value.total_paid;
    sale.sale.balance = backup.value.balance;
    sale.sale.changed_amount = backup.value.changed_amount;
    sale.sale.tip_amount = 0;
    emit("resolve", false);
}

function onTipPressed(){ 
    if((sale.sale.tip_amount||0)>0){
        sale.sale.tip_amount = 0;
    }
    else{
        if(parseFloat(sale.paymentInputNumber)>0){
            sale.sale.tip_amount = parseFloat(sale.paymentInputNumber);
        }   
        else{
            toaster.warning($t('msg.Please input amount for TIP'));
        }  
    }
}

async function onPayment() {
    if (sale.sale.payment.filter(r => r.required_customer == 1).length > 0) {
        if (sale.sale.customer == sale.setting.customer) {
            toaster.warning($t("msg.Please select customer for payment type")+" " + sale.sale.payment.filter(r => r.required_customer == 1)[0].payment_type);
            return;
        }
    }
    sale.pos_receipt = selectedReceipt.value;
    sale.message = $t("msg.Payment successfully");
    sale.onSubmitPayment(true).then((v) => {
        if (v) {            
            emit("resolve", true);
        }
    })
}
async function onPaymentWithoutPrint() {
    if (sale.sale.payment.filter(r => r.required_customer == 1).length > 0) {
        if (sale.sale.customer == sale.setting.customer) {

            toaster.warning($t('msg.Please select customer for payment type')+" " + sale.sale.payment.filter(r => r.required_customer == 1)[0].payment_type);
            return;
        }
    }
    sale.pos_receipt = undefined;
    sale.message = $t("msg.Payment successfully");


    sale.onSubmitPayment(false).then((v) => {
        if (v) {
            emit("resolve", true);
        }
    })
}

onUnmounted(() => {
    sale.sale.payment = [];
    sale.is_payment_first_load = false;
})

</script>