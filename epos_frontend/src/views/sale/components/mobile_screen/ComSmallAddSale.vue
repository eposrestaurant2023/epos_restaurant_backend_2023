<template>
    <div class="h-full">
        <div  class="h-full flex-col flex">
            <div class="overflow-auto mb-20">
                <div class="pt-2">
                    <!-- <div class="px-2 pb-2">
                        <ComSelectCustomer  />
                    </div> -->
                    <ComProductSearch :small="true" />
                    <ComMenu /> 
                </div>
            </div>
            <div  class="text-white text-sm px-2 py-1 fixed left-0 right-0 bottom-0" :class="((lastProduct?'':'h-20')+(checkNewSaleNoSaleProducts?' bg-red-200':' bg-red-500') )"  v-ripple @click="onViewDetail">
                <div>
                    <div class="text-xs" v-if="lastProduct">{{ lastProduct.product_code }} - {{ lastProduct.product_name }} ({{ lastProduct.quantity }})</div>
                    
                    <div class="flex items-center justify-between">                        
                        <div>{{ $t('Total Qty') }}</div>
                        <div>{{ sale.sale.total_quantity||0 }}</div>                    
                    </div> 
                    <div class="flex items-center justify-between">
                        <div>{{ $t('Total Amount') }}</div>
                        <div style="font-size: 24px;">
                            <CurrencyFormat :value="sale.sale.grand_total" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { inject,computed, smallViewSaleProductListModal,i18n  } from '@/plugin'
import { createToaster } from "@meforma/vue-toaster";

import ComMenu from '../ComMenu.vue';
// import ComSelectCustomer from '../ComSelectCustomer.vue';
import ComProductSearch from '../ComProductSearch.vue';

const toaster = createToaster({ position: "top" });
const { t: $t } = i18n.global;

const sale = inject('$sale');
const lastProduct = computed(()=>{
    return sale.sale?.sale_products?.find(r=>r.selected == true)
})

const checkNewSaleNoSaleProducts = computed(()=>{
    if((sale.sale.name||'')=='' && (sale.sale.sale_products||[]).length <=0){
        return true;
    } 
    return false;
    
})

async function onViewDetail(){
    if(checkNewSaleNoSaleProducts.value){
        toaster.warning( $t('msg.Please select a menu item to continue'));
       
        return;
    }
    const result = await smallViewSaleProductListModal ({title: sale.sale.name, value:  ''});
}
</script> 