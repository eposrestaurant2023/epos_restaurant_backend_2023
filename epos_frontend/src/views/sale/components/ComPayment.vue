<template>
    <v-dialog v-model="open" fullscreen persistent @update:modelValue="onClose">
        <v-card>
            <ComToolbar @onClose="onClose">
                <template #title>
                    Payment
                </template>
            </ComToolbar>
            <v-card-text class="!px-0 !pt-1 !pb-0 overflow-hidden">
                <v-row class="!m-0 h-full">
                    <v-col class="!p-1 h-full" md="4">
                        <ComPaymentInputNumber />
                    </v-col>
                    <v-col class="!p-1 h-full" md="4">
                        <div class="overflow-auto h-full">
                            <div class="grid grid-cols-2 gap-1">
                                <div class="border rounded-sm px-2 py-4 text-center cursor-pointer bg-orange-100 hover:bg-orange-300 flex justify-center items-center"
                                    v-for="(pt, index) in gv.setting?.payment_types" :key="index"
                                    @click="onPaymentTypeClick(pt)">
                                    <div>
                                        {{ pt.payment_method }}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </v-col>
                    <v-col class="!p-0 h-full" md="4">
                        <div class="bg-gray-100 p-1 h-full">
                            <div class="grid h-full" style="grid-template-rows: max-content;">
                                <div class="bg-red-200 p-4 mb-2 rounded-sm text-lg">
                                    <div class="text-center">Total Amount</div>
                                    <div class="flex justify-around">
                                        <div>
                                            <CurrencyFormat :value="sale.sale.grand_total" />
                                        </div>
                                        <div>
                                            <CurrencyFormat :value="sale.sale.grand_total * sale.sale.exchange_rate"
                                                :currency="gv.setting?.pos_setting?.second_currency_name" />
                                        </div>
                                    </div>
                                </div>
                                <div class="overflow-auto h-full">
                                    <div v-for="(p, index) in sale.sale.payment" :key="index">
                                        <div
                                            class="flex items-center p-1 bg-white rounded-sm mb-1 border border-gray-600">
                                            <div class="flex-grow">
                                                <div class="font-bold">{{ p.payment_type }}</div>
                                            </div>
                                            <div class="flex-none text-right">
                                                <CurrencyFormat :value="p.input_amount" :currency="p.currency" />
                                            </div>
                                            <div class="flex-none">
                                                <v-btn size="small" variant="text" color="error" icon="mdi-delete"
                                                    @click="onRemovePayment(p)"></v-btn>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div
                                    class="mt-auto bg-green-600 border border-gray-500 px-1 pt-1 text-white rounded-sm">
                                    <div class="mb-1 flex justify-between" v-if="sale.sale.total_paid > 0">
                                        <div>Total Payment:</div>
                                        <div>
                                            <CurrencyFormat :value="sale.sale.total_paid" />
                                        </div>
                                    </div>
                                    <div class="mb-1 flex justify-between" v-if="sale.sale.balance > 0">
                                        <div>Balance:</div>
                                        <div>
                                            <CurrencyFormat :value="sale.sale.balance" />
                                        </div>
                                    </div>
                                    <div class="mb-1 flex justify-between" v-if="sale.sale.changed_amount > 0">
                                        <div>Change Amount:</div>
                                        <div>
                                            <CurrencyFormat :value="sale.sale.changed_amount" />
                                        </div>
                                    </div>
                                    <div class="mb-1 flex justify-between" v-if="sale.sale.changed_amount > 0">
                                        <div>Change Amount {{ gv.setting.pos_setting.second_currency_name }}:</div>
                                        <div>
                                            <CurrencyFormat :value="sale.sale.changed_amount * sale.sale.exchange_rate"
                                                :currency="gv.setting.pos_setting.second_currency_name" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </v-col>
                </v-row>
            </v-card-text>
            <div>
                <v-row class="!m-0">
                    <v-col class="!p-0" md="8">
                        <div class="h-full flex items-center">
                            <div class="mx-2" v-if="printFormatResource.data?.length > 1">
                                <v-chip :color="item.name == selectedReceipt.name?'warning':''" class="m-1" @click="onSelectedReceipt(item)"
                                    v-for="(item, index) in printFormatResource.data" :key="index">{{ item.name }}
                                </v-chip>
                            </div>
                        </div>
                    </v-col>
                    <v-col class="!p-0" md="4">
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
            </div>
        </v-card>
    </v-dialog>


</template>
<script setup>

import { inject, createResource, ref } from '@/plugin';
import CurrencyFormat from '../../../components/CurrencyFormat.vue';
import ComPaymentInputNumber from "./ComPaymentInputNumber.vue"
import ComToolbar from '@/components/ComToolbar.vue';

const open = ref(true)
const sale = inject("$sale")
const gv = inject("$gv")

const emit = defineEmits(['resolve'])

const props = defineProps({
    params: Object
})

const selectedReceipt = ref({})
selectedReceipt.value = gv.setting.default_pos_receipt;

const printFormatResource = createResource({
    url: "epos_restaurant_2023.api.api.get_pos_print_format",
    params: {
        doctype: "Sale"
    },
    cache: ["print_format", "Sale"],
    auto: true,


})

sale.paymentInputNumber = sale.sale?.grand_total;

function onSelectedReceipt(r) {
    selectedReceipt.value = r;

}

function onRemovePayment(p) {
    sale.sale.payment.splice(sale.sale.payment.indexOf(p), 1);
    sale.updatePaymentAmount();
    sale.paymentInputNumber = sale.sale.balance;
}

function onPaymentTypeClick(pt) {
    sale.onAddPayment(pt, sale.paymentInputNumber);

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
</script>