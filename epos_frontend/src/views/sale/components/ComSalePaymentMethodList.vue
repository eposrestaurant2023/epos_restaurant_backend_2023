<template>
    <div class="grid gap-1" :class="mobile ? 'grid-cols-3 text-sm' : 'grid-cols-2'">
        <div class="border rounded-sm px-2 py-4 text-center cursor-pointer bg-orange-100 hover:bg-orange-300 flex justify-center items-center"
            v-for="(pt, index) in gv.setting?.payment_types" :key="index"
            @click="onPaymentTypeClick(pt)">
            <div>
                {{ pt.payment_method }} 
            </div>
        </div>
    </div>
</template>
<script setup>
import { inject,onMounted } from '@/plugin';
import { useDisplay } from 'vuetify'
const {mobile} = useDisplay()
const gv = inject("$gv")
const sale = inject("$sale")

let is_first_input_amount = true;
let payment_input_number = 0;


onMounted(()=>{
    payment_input_number = sale.paymentInputNumber;
    is_first_input_amount = true;
});

function onPaymentTypeClick(pt) { 
    if( payment_input_number == sale.paymentInputNumber){
        is_first_input_amount = true;
    }
    
    if(is_first_input_amount){
        sale.paymentInputNumber = sale.paymentInputNumber * pt.exchange_rate;
    }
    sale.onAddPayment(pt, sale.paymentInputNumber);
    is_first_input_amount = false;
}
</script>