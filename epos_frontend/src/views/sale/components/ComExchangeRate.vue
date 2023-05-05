<template>
    <v-tooltip text="Exchange Rate" location="top">
        <template v-slot:activator="{ props }"> 
            <v-chip v-bind="props" color="white" class="m-1" rounded="pill" variant="elevated" size="x-small" ><CurrencyFormat :value="1" :currency="gv.setting.pos_setting.exchange_rate_main_currency"  />  = <CurrencyFormat :value="exchange_rate" :currency="to_currency" /></v-chip>
        </template>
    </v-tooltip>
</template>
<script setup>
import {inject,createResource,ref,computed} from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";

const gv = inject("$gv")
const sale = inject("$sale")
let exchange_rate =ref(1);
const toaster = createToaster({ position:"top" });

const to_currency = computed(()=>{
    if(gv.setting.pos_setting.exchange_rate_main_currency!=gv.setting.pos_setting.main_currency_name){
        return gv.setting.pos_setting.main_currency_name;
    }else {
        return gv.setting.pos_setting.second_currency_name;
    }
})


const exchangeRateResource = createResource({
    url:"frappe.client.get_list",
    params:{
        doctype: "Currency Exchange",
        fields: ["exchange_rate","exchange_rate_input"],
        filters: {
            "from_currency":gv.setting.pos_setting.exchange_rate_main_currency,
            "to_currency":to_currency.value,
            "docstatus":1},
        order_by: "creation desc",
        limit_page_length: 1
        
    },
    auto:true,
    //cache:"exchange_rate",
    onSuccess(data){
        let rate = 1;
        if(data.length>0){
            
            exchange_rate.value=data[0].exchange_rate_input;
            sale.sale.exchange_rate = data[0].exchange_rate;
            rate =  data[0].exchange_rate;
        }else{
            exchange_rate.value=1;
            rate = 1;
            
        }
        sale.exchange_rate = rate;
        sale.sale.exchange_rate = rate;
        
        
    }

})

</script>