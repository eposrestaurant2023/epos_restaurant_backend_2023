
<script setup>
import {inject,createResource} from '@/plugin'
import { onMounted } from 'vue';
    const gv = inject('$gv')
    const sale = inject('$sale')
    const emit = defineEmits(['onHandle'])
    const props = defineProps({
        saleProduct: Object
    }) 
    async function onCheckPromotion(){
        return  new Promise(async (resolve) => {
            const check_promotion =  createResource({
                url: 'epos_restaurant_2023.api.promotion.check_promotion',
                //cache: "check_promotion_today",
                auto: true,
                params: { 
                    check_time: 1,
                    business_branch: gv.setting.business_branch || ''
                }
            });

            await check_promotion.fetch().then(async (doc) => {
                if(doc){
                    resolve(doc);
                }
                else{
                    resolve(false);
                }
            });
           
        });
    }


    
    async function checkProductPromotion(){
        return  new Promise(async (resolve) => {
            const params = {
                            product_name: props.saleProduct.product_code,
                            promotions: gv.getPromotionByCustomerGroup(sale.sale.customer_group) || []
                        };

            const check_promotion_product =  createResource({
                    url: 'epos_restaurant_2023.api.promotion.check_promotion_product',
                    cache: "check_promotion_product",
                    params: params
                });
            check_promotion_product.params = params; 

            await check_promotion_product.fetch().then(async (doc) => {
                if(doc){
                    resolve(doc);
                }
                else{
                    resolve(false);
                }
            });            
        });
    }


    onMounted(async()=>{        
        const check_promotion = await onCheckPromotion(); 
        if(check_promotion){
            gv.promotion = check_promotion;
            sale.promotion = check_promotion;
          
            const check_product_promotion = await checkProductPromotion();       
            if(check_product_promotion){ 
                onUpdateSaleProduct(check_product_promotion)                
            }          
            props.saleProduct.is_render = false;     
        }
        else{
            gv.promotion = null,
            sale.promotion = null
        }
        props.saleProduct.is_render = false;
    })

    function onUpdateSaleProduct(promotion){
        const sp = props.saleProduct;
        if(promotion && sp.allow_discount){
            sp.discount_type = 'Percent'
            sp.discount = (promotion.percentage_discount || 0)
            sp.happy_hour_promotion = promotion.name
            sp.happy_hours_promotion_title = promotion.promotion_name
            sale.updateSaleProduct(sp);
            sale.updateSaleSummary();            
        }
    }
</script>