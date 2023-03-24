<template>
    <ComPlaceholder :is-not-empty="sale.getSaleProducts().length > 0" icon="mdi-cart-outline" text="Empty Product">
        <div v-for="(g, index) in sale.getSaleProductGroupByKey()" :key="index">
       
                <div class="bg-red-700 text-white flex items-center justify-between" style="font-size: 10px; padding: 2px;">
                    <div><v-icon icon="mdi-clock" size="small" class="mr-1"></v-icon>{{
                        moment(g.order_time).format('HH:mm:ss')
                    }}</div>
                    <div><v-icon icon="mdi-account-outline" size="small" class="mr-1"></v-icon>{{ g.order_by }}</div>
                </div>
                <ComSaleProductList :group-key="g" />

        </div>
    </ComPlaceholder>
</template>
<script setup>
import { inject } from 'vue'
import moment from '@/utils/moment.js';
import ComSaleProductList from './ComSaleProductList.vue';
const sale = inject('$sale')

</script>