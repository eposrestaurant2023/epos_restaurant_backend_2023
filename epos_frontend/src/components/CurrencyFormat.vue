<template>
        {{ vueNumberFormat(value,{prefix:prefix,suffix:suffix, precision:precision}) }} 
</template>
<script setup>

import { useStore, defineProps,ref } from '@/plugin';

const store = useStore();
const props =defineProps({
    value:Number,
    currency:{
        type:String,
        default:""
    }
})

const prefix = ref("")
const suffix = ref("")
const precision = ref(0)

let currency_name = props.currency

if (currency_name==""){
    currency_name = store.state.setting?.default_currency
}
const currency = store.state.setting?.currencies.find(r=>r.name==currency_name)
if (currency){
    if (currency.symbol_on_right){
        suffix.value =" " + currency.symbol;
    }else {
        prefix.value = currency.symbol + " ";
    }
    precision.value = currency.currency_precision
}


</script>