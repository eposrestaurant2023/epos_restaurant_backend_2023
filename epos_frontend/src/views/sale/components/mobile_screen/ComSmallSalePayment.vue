<template>
    <div>
        <ComSalePaymentGrandTotalInformation/>
        <div>
            <v-text-field 
            label="Enter number"
            variant="solo"
            v-model="sale.paymentInputNumber"
            clearable 
            maxlength="10"
            append-inner-icon="mdi-selection"
            @click:append-inner="onShowCurrencyPrefine()"
            ></v-text-field>
        </div>
        <ComSalePaymentMethodList/>
        <div>
            <div class="font-bold text-center py-2">Payments</div>
            <ComSalePaymentList/>
        </div>
    </div>
</template>
<script setup>
import { inject,smallCurrencyPrefineModel } from '@/plugin'
import ComSalePaymentGrandTotalInformation from '../ComSalePaymentGrandTotalInformation.vue';
import ComSalePaymentList from '../ComSalePaymentList.vue';
import ComSalePaymentMethodList from '../ComSalePaymentMethodList.vue';
const sale = inject('$sale')
async function onShowCurrencyPrefine(){
    const result = await smallCurrencyPrefineModel({})
}
</script> 