
<template>
    <div  class="grid h-full" style="grid-template-rows: max-content auto">
        <!-- 140px -->
        <div class="overflow-auto" style="height: calc(100vh);">
            <div class="pt-2">
                <div class="px-2 pb-2">
                    <ComSelectCustomer  />
                </div>
                <ComProductSearch :small="true" />
                <ComMenu /> 
            </div>
        </div> 
        <div class="bg-red-500 text-white text-sm px-2 py-1 fixed bottom-0 left-0 right-0" v-ripple @click="onViewDetail">
            <div>
                <div class="text-xs" v-if="lastProduct">{{ lastProduct.product_code }} - {{ lastProduct.product_name }} ({{ lastProduct.quantity }})</div>
                <div class="flex items-center justify-between">
                    <div>total Qty</div>
                    <div>{{ sale.sale.total_quantity }}</div>
                </div>
                <div class="flex items-center justify-between">
                    <div>Total Amount {{ sale.mobile_view_sale_product }}</div>
                    <div style="font-size: 24px;"><CurrencyFormat :value="sale.sale.grand_total" /></div>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { inject,computed, smallViewSaleProductListModel  } from '@/plugin'
import {useDisplay} from 'vuetify'
import ComMenu from '../ComMenu.vue';
import ComSelectCustomer from '../ComSelectCustomer.vue';
import ComProductSearch from '../ComProductSearch.vue';
const sale = inject('$sale')
const {mobile} = useDisplay()
const lastProduct = computed(()=>{
    return sale.sale?.sale_products?.find(r=>r.selected == true)
})
async function onViewDetail(){
 const result = await smallViewSaleProductListModel ({title: sale.sale.name ? sale.sale.name : 'New Sale', value:  ''});
}
</script> 