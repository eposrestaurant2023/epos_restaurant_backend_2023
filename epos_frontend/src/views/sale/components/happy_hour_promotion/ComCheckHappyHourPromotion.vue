<template>
    <ComChip :tooltip="product.happy_hour_promotion_title" v-if="product.happy_hour_promotion && product.discount > 0" size="x-small" variant="outlined" color="orange" text-color="white" prepend-icon="mdi-tag-multiple">
        <span>{{ product.discount }}%</span>
    </ComChip>
</template>
<script setup>
import {inject,createResource} from '@/plugin'
const gv = inject('$gv')
const sale = inject('$sale')
const emit = defineEmits(['onHandle'])
const props = defineProps({
    product: Object
})

if(gv.promotion){ 
    createResource({
        url: 'epos_restaurant_2023.api.promotion.check_promotion',
        cache: "check_promotion_today",
        auto: true,
        params: { 
            check_time: 1,
            business_branch: gv.setting.business_branch || ''
        },
        onSuccess(doc) { 
            if(doc){
                productDiscountResource.params = productDiscountResourceParams()
                productDiscountResource.fetch()
            }else{
                gv.promotion = null,
                sale.promotion = null
            }
        }
    });
}

let productDiscountResource = createResource({
    url: 'epos_restaurant_2023.api.promotion.check_promotion_product',
    cache: "check_promotion_product",
    params: productDiscountResourceParams(),
    onSuccess(doc) { 
        if(doc){ 
            emit('onHandle', doc)
        }else{
            emit('onHandle', false)
        }
    }
});
function productDiscountResourceParams(){
    return {
            product_name: props.product.product_code,
            promotions: gv.getPromotionByCustomerGroup(sale.sale.customer_group) || []
        }
}
</script>
<style lang="">
    
</style>