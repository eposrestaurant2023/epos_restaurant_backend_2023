<template>
    <v-card elevation="0"  rounded="0">
        <div class="flex p-2 border-b">
            <v-avatar 
            size="90px"
            rounded="lg">
                <div class="h-full w-full bg-cover bg-center" v-bind:style="{ 'background-image': 'url(' + (product.photo || '/assets/frappe/images/emenu_placeholder.jpg') + ')' }"></div>
            </v-avatar>
            <div class="grow pl-2 grid content-between">
                <div class="flex justify-between">
                    <div>
                        <h3 class="font-bold">{{ product?.name }} - {{ product.name_en }}</h3>
                        <h4 v-if="product.name_en != product.name_kh">{{ product.name_kh }}</h4>
                    </div>
                    <div class="text-lg font-bold text-right"  :style="{color:gv.setting?.template_style?.title_color}" :class="gv.setting?.template_style?.title_class">
                        <CurrencyFormat :value="getPrice(product)" />
                    </div>
                </div>
                <div>
                    <div class="flex justify-end"> 
                        <template v-if="checkProductHaveOptions(product)">
                            <button type="button" class="btn-item-customize h-7 w-40 rounded-sm" @click.stop="onItemClick()">
                                Customize
                            </button>
                        </template>
                        <template v-else>
                            <ComButtonAddOn/>
                        </template>
                    </div>
                </div>
            </div>
        </div> 
    </v-card>
</template>
<script setup>
    import { inject,ref,onMounted } from 'vue' 
    import ComButtonAddOn from '../../components/form/ComButtonAddOn.vue';
    import CurrencyFormat from './CurrencyFormat.vue';
    const gv = inject('$gv')
    const props = defineProps({
        product: Object
    })
    const emit = defineEmits(['onItemClick'])
    let qty = ref(0) 

    function onItemClick(){ 
        emit('onItemClick', props.product)
    }
 


    function checkProductHaveOptions(p){
        if(p.is_combo_menu==1){
            return true;
        } 
        const portions = JSON.parse(p.prices)?.filter(r => (r.branch ==gv.pos_profile.business_branch || r.branch == '') && r.price_rule == gv.pos_profile.price_rule);        
        const check_modifiers = onCheckModifier(JSON.parse(p.modifiers));

        if (check_modifiers || portions?.length > 1) {
            return true;            
        } else {
            return false
        }
    }

    function getPrice(p){ 
        const portions = JSON.parse(p.prices)?.filter(r => (r.branch ==gv.pos_profile.business_branch || r.branch == '') && r.price_rule == gv.pos_profile.price_rule); 
        if(portions.length>0){
            return portions[0].price;
        }
        if(JSON.parse(p.prices).length>0){
            return 0
        }
        return p.price
    }


   function onCheckModifier(modifiers) {
        if (modifiers) {
            const data = modifiers.filter((r) => {
                return r.items.some(x => x.branch == gv.pos_profile.business_branch || x.branch == '')
            })
            return data.length > 0;
        }
        return false
    }


    
</script>


<style scoped>
.btn-item-customize {
    border: 1px solid rgb(6, 180, 6);
    color: rgb(6, 180, 6);
} 
</style>