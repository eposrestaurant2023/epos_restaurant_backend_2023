<template>
    <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" :hide-close-button="true" :hide-ok-button="true" :fill="true">
        <template #title>
            Payment
        </template>
        <template #bar_custom>
            <ComSelectPaymentPrinter @onClick="onSelectedReceipt" :selected="selectedReceipt.name" v-if="mobile"/>
        </template>
        <template #content>
            <div v-if="mobile" class="!p-1 overflow-auto">
                <ComSmallSalePayment/>
            </div>
            <div v-else class="!px-0 !pt-1 !pb-0 overflow-hidden">
                <v-row class="!m-0 h-full">
                    <v-col class="!p-1 h-full" md="4">
                        <ComPaymentInputNumber />
                    </v-col>
                    <v-col class="!p-1 h-full" md="4">
                        <div class="overflow-auto h-full">
                            <ComSalePaymentMethodList/>
                        </div>
                    </v-col>
                    <v-col class="!p-0 h-full" md="4">
                        <div class="bg-gray-100 p-1 h-full">
                            <div class="grid h-full" style="grid-template-rows: max-content;">
                                <ComSalePaymentGrandTotalInformation/>
                                <div class="overflow-auto h-full">
                                    <ComSalePaymentList/>
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
                    <v-col class="!p-0" cols="12" md="8">
                        <div class="h-full flex items-center" v-if="!mobile">
                            <ComSelectPaymentPrinter @onClick="onSelectedReceipt" :selected="selectedReceipt.name"/>
                        </div>
                        <div v-else>
                            <ComPaymentSummaryInformation />
                        </div>
                    </v-col>
                    <v-col class="!p-0" cols="12" md="4">
                        <v-row class="!m-0">
                            <v-col class="!p-0" cols="6">
                                <div class="p-1">
                                    <v-btn size="small" class="w-full" color="primary" @click="onPaymentWithoutPrint" stacked
                                        prepend-icon="mdi-currency-usd">Payment</v-btn>
                                </div>
                            </v-col>
                            <v-col class="!p-0" cols="6">
                                <div class="p-1">
                                    <v-btn size="small" class="w-full" color="primary" @click="onPayment"
                                        stacked prepend-icon="mdi-printer">
                                        <span>Payment with Print</span>
                                    </v-btn>
                                </div>
                            </v-col>
                        </v-row>
                    </v-col>
                </v-row>
            </template>
    </ComModal>
</template>
<script setup>

import { inject, createResource, ref ,onUnmounted} from '@/plugin';
import ComPaymentInputNumber from "./ComPaymentInputNumber.vue"
import ComSmallSalePayment from "./mobile_screen/ComSmallSalePayment.vue"
import { useDisplay } from 'vuetify'
import ComToolbar from '../../../components/ComToolbar.vue';
import ComSalePaymentMethodList from './ComSalePaymentMethodList.vue';
import ComSalePaymentGrandTotalInformation from './ComSalePaymentGrandTotalInformation.vue';
import ComSalePaymentList from './ComSalePaymentList.vue';
import ComSelectPaymentPrinter from './ComSelectPaymentPrinter.vue';
import ComPaymentSummaryInformation from './ComPaymentSummaryInformation.vue';
const { mobile } = useDisplay()

let open = ref(true)
const sale = inject("$sale")
const gv = inject("$gv")

const emit = defineEmits(['resolve'])

const props = defineProps({
    params: Object
})

const selectedReceipt = ref({})
selectedReceipt.value = gv.setting.default_pos_receipt;

sale.paymentInputNumber = sale.sale?.grand_total;

function onSelectedReceipt(r) {
    selectedReceipt.value = r;

}

function onClose() {
    emit("resolve", false);
}

async function onPayment() {
    sale.pos_receipt = selectedReceipt.value;
    sale.message = "Payment successfully";
    sale.onSubmitPayment(true).then((v) => {
        if (v) {
            emit("resolve", true);
        }
    })
}
async function onPaymentWithoutPrint() {
    sale.pos_receipt = undefined;
    sale.message = "Payment successfully";
    sale.onSubmitPayment(false).then((v) => {
        if (v) {
            emit("resolve", true);
        }
    })
}
onUnmounted(() => {
    sale.sale.payment=[];
})
</script>