<template>
    <v-tooltip text="Exchange Rate" location="top">
        <template v-slot:activator="{ props }">
            <v-chip v-bind="props" color="white" class="m-1" rounded="pill" variant="elevated" size="small" ><CurrencyFormat :value="1" />  = <CurrencyFormat :value="exchange_rate" :currency="gv.setting.pos_setting.second_currency_name" /></v-chip>
        </template>
    </v-tooltip>
</template>
<script setup>
import {inject,createResource,ref} from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";

const gv = inject("$gv")
const sale = inject("$sale")
let exchange_rate =ref(1);
const toaster = createToaster({ position:"top" });
const exchangeRateResource = createResource({
    url:"frappe.client.get_list",
    params:{
        doctype: "Currency Exchange",
        fields: ["exchange_rate"],
        filters: {"from_currency":gv.setting.pos_setting.main_currency_name,"to_currency":gv.setting.pos_setting.second_currency_name,"docstatus":1},
        order_by: "posting_date desc",
        limit_page_length: 1
        
    },
    auto:true,
    //cache:"exchange_rate",
    onSuccess(data){
        if(data.length>0){
            exchange_rate.value=data[0].exchange_rate;
            sale.sale.exchange_rate = exchange_rate.value;
        }else{
            exchange_rate.value=1;
            
        }
        sale.sale.exchange_rate = exchange_rate.value;
       
        if(exchange_rate.value<=1){
            toaster.warning("Invalid Exchange Rate")
        }
    }

})

</script>