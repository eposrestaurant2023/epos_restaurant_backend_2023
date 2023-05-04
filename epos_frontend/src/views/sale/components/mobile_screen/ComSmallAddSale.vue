<template>
    <div class="h-full">
        <div  class="h-full flex-col flex">
            <div class="overflow-auto mb-20">
                <div class="pt-2">
                    <div class="px-2 pb-2">
                        <ComSelectCustomer  />
                    </div>
                    <ComProductSearch :small="true" />
                    <ComMenu /> 
                </div>
            </div>
            <div class="bg-red-500 text-white text-sm px-2 py-1 h-20 fixed left-0 right-0 bottom-0" v-ripple @click="onViewDetail">
                <div>
                    <div class="text-xs" v-if="lastProduct">{{ lastProduct.product_code }} - {{ lastProduct.product_name }} ({{ lastProduct.quantity }})</div>
                    
                    <div class="flex items-center justify-between">                        
                        <div>Total Qty</div>
                        <div>{{ sale.sale.total_quantity }}</div>                    
                    </div> 
                    <div class="flex items-center justify-between">
                        <div>Total Amount</div>
                        <div style="font-size: 24px;"><CurrencyFormat :value="sale.sale.grand_total" /></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { inject,computed, smallViewSaleProductListModel  } from '@/plugin'

import ComMenu from '../ComMenu.vue';
import ComSelectCustomer from '../ComSelectCustomer.vue';
import ComProductSearch from '../ComProductSearch.vue';
const sale = inject('$sale')
const lastProduct = computed(()=>{
    return sale.sale?.sale_products?.find(r=>r.selected == true)
})
async function onViewDetail(){
 const result = await smallViewSaleProductListModel ({title: sale.sale.name ? sale.sale.name : 'New Sale', value:  ''});
}
</script> 