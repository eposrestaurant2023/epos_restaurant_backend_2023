
<script setup>
import {inject,createResource} from '@/plugin'
const gv = inject('$gv')
const sale = inject('$sale')
const emit = defineEmits(['onHandle'])
const props = defineProps({
    productName: ''
})

if(gv.promotion){
    console.log(gv.promotion)
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
    //cache: "check_promotion_product",
    params: productDiscountResourceParams(),
    onSuccess(doc) {
        console.log(doc)
        if(doc){ 
            emit('onHandle', doc)
        }else{
            emit('onHandle', false)
        }
    }
});
function productDiscountResourceParams(){
    return {
            product_name: props.productName,
            promotions: gv.promotion
        }
}
</script>
<style lang="">
    
</style>