<template>
    {{ numberFormat(format,value) }}
</template>
<script setup>

import { inject, defineProps,ref } from '@/plugin';
const gv = inject("$gv")
const numberFormat = inject("$numberFormat")
const props =defineProps({
    value:Number,
    currency:{
        type:String,
        default:""
    }
})

const format = ref("#,###,##0.00##")

let currency_name = props.currency

if (currency_name==""){
    currency_name = gv.setting?.default_currency
}
const currency = gv.setting?.currencies.find(r=>r.name==currency_name)
if (currency){
   format.value = currency.pos_currency_format
}


</script>