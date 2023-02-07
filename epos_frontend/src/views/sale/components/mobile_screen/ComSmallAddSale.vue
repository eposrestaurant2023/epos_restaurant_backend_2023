
<template>
    <div class="overflow-auto" style="height: calc(100vh - 128px);">
        <div class="pt-2">
            <ComProductSearch :small="true" />
            <ComMenu />
            <div style="height:80px"></div>
        </div>
        <div class="bg-red-500 text-white fixed bottom-0 right-0 left-0 text-sm p-2" v-ripple>
            <div>
                <div class="text-xs" v-if="lastProduct">{{ lastProduct.product_code }} - {{ lastProduct.product_name }} ({{ lastProduct.quantity }})</div>
                <div class="flex items-center justify-between">
                    <div>total Qty</div>
                    <div>{{ sale.sale.total_quantity }}</div>
                </div>
                <div class="flex items-center justify-between">
                    <div>Total Amount</div>
                    <div><CurrencyFormat :value="sale.sale.total_amount" /></div>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { inject,computed  } from 'vue'
import Enumerable from 'linq'
import ComMenu from '../ComMenu.vue';
import ComProductSearch from '../ComProductSearch.vue';
const sale = inject('$sale')
const lastProduct = computed(()=>{
    return sale.sale.sale_products.find(r=>r.selected == true)
})
</script> 