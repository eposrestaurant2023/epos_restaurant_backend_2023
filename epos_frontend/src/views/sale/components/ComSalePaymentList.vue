<template>
    <div>
        <ComPlaceholder :is-not-empty="sale.sale.payment.length > 0" icon="mdi-currency-usd" icon-size="30px" text="No Payment">
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
        </ComPlaceholder>
    </div>
</template>
<script setup>
import { inject } from 'vue'
const sale = inject('$sale')
function onRemovePayment(p) {
    sale.sale.payment.splice(sale.sale.payment.indexOf(p), 1);
    sale.updatePaymentAmount();
    sale.paymentInputNumber = sale.sale.balance;
}
</script> 