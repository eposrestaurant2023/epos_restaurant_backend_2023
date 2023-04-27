<template lang=""> 
    <div class="h-full flex-col flex">
        <ComCustomerDisplayCustomerProfile :data="data" />
        <div class="product-list overflow-auto h-full">  
            <ComPlaceholder :isNotEmpty="getSaleProducts.length > 0" text="Please sale order" icon="mdi-cart-outline" iconSize="80px">
                <template v-if="getSaleProducts.length > 0">
                    <div v-for="(g, index) in getSaleProductGroupByKey" :key="index">
                        <div class="bg-red-700 text-white flex items-center justify-between" style="font-size: 10px; padding: 2px;">
                            <div><v-icon icon="mdi-clock" size="small" class="mr-1"></v-icon>{{
                                moment(g.order_time).format('HH:mm:ss')
                            }}</div>
                            <div><v-icon icon="mdi-account-outline" size="small" class="mr-1"></v-icon>{{ g.order_by }}</div>
                        </div>
                        <ComSaleProductList :saleCustomerDisplay="data" :group-key="g" :readonly="true"/>
                    </div>
                </template>
            </ComPlaceholder>
        </div>
        <div class="mt-auto">
            <div class="-mx-1 bg-blue-100 rounded-tl-md rounded-tr-md text-xs">
                <ComCustomerDisplaySummary :data="data"/>
                <ComCustomerDisplaySummarPayment :data="(data)"/>
            </div>
        </div>
    </div> 

</template>
<script setup>
import { inject, computed } from '@/plugin';
import Enumerable from 'linq';
import ComCustomerDisplaySummary from './components/ComCustomerDisplaySummary.vue';
import ComCustomerDisplaySummarPayment from './components/ComCustomerDisplaySummarPayment.vue';
import moment from '@/utils/moment.js';
import ComSaleProductList from '../sale/components/ComSaleProductList.vue';
import ComCustomerDisplayCustomerProfile from './components/ComCustomerDisplayCustomerProfile.vue';
const gv = inject("$gv")
const props = defineProps({
    data: Object,
})
const getSaleProducts = computed(() => {

    return Enumerable.from(props.data.sale_products).orderByDescending("$.modified").toArray()

})
const getSaleProductGroupByKey = computed(() => {
    if (!props.data.sale_products) {
        return []
    } else {
        if (props.data?.sale_products) {
            const group = Enumerable.from(props.data.sale_products).groupBy("{order_by:$.order_by,order_time:$.order_time}", "", "{order_by:$.order_by,order_time:$.order_time}", "$.order_by+','+$.order_time");
            return group.orderByDescending("$.order_time").toArray();
        }
        return []
    }
})
</script> 