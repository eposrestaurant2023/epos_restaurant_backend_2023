<template> 
    <ComPlaceholder :is-not-empty="sale.getSaleProducts().length > 0 || (gv.device_setting.show_deleted_sale_product_in_sale_screen==1 && sale.deletedSaleProductsDisplay.length>0)" 
        icon="mdi-cart-outline" :text="$t('Empty Data')">
        <div>   
            <template v-if="gv.device_setting.show_deleted_sale_product_in_sale_screen==1">
             
                <span v-for="(g, index) in sale.getSaleProductDeletedGroupByKey()" :key="index">       
                        <div class="bg-red-700 text-white flex items-center justify-between" style="font-size: 10px; padding: 2px;">
                            <div><v-icon icon="mdi-clock" size="small" class="mr-1"></v-icon>{{
                                moment(g.order_time).format('HH:mm:ss')
                            }}</div>
                            <div><v-icon icon="mdi-account-outline" size="small" class="mr-1"></v-icon>{{ g.order_by }}</div>
                        </div>
                        <ComSaleProductDeletedList :group-key="g" />
                </span>
            </template>
            
            <span v-for="(g, index) in sale.getSaleProductGroupByKey()" :key="index">       
                    <div class="bg-red-700 text-white flex items-center justify-between" style="font-size: 10px; padding: 2px;">
                        <div><v-icon icon="mdi-clock" size="small" class="mr-1"></v-icon>{{
                            moment(g.order_time).format('HH:mm:ss')
                        }}</div>
                        <div><v-icon icon="mdi-account-outline" size="small" class="mr-1"></v-icon>{{ g.order_by }}</div>
                    </div>
                    <ComSaleProductList :group-key="g" />
            </span>

          

        </div>
    </ComPlaceholder>
</template>
<script setup>
import { inject } from 'vue'
import moment from '@/utils/moment.js';
import ComSaleProductList from './ComSaleProductList.vue';
import ComSaleProductDeletedList from './ComSaleProductDeletedList.vue';
const sale = inject('$sale')
const gv = inject('$gv')

</script>



