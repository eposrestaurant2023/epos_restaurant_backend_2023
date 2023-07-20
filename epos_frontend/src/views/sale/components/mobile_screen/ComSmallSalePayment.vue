<template>
    <div>
        <ComSalePaymentGrandTotalInformation @onClick="onShowCurrencyPrefine()"/>
        <div>
            <v-text-field 
            :label="$t('Enter Number')"
            type="number"
            variant="solo"
            v-model="sale.paymentInputNumber"
            clearable 
            maxlength="10"
            ></v-text-field>
        </div>
        <ComSalePaymentMethodList/>
        <div>
            <div class="font-bold text-center py-2">{{ $t('Payment') }}</div>
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
const gv = inject('$gv')
async function onShowCurrencyPrefine(){
    if(gv.setting.pos_setting.main_currency_predefine_payment_amount){
        const result = await smallCurrencyPrefineModel({})
    }        
}
</script> 