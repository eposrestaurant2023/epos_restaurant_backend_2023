<template>
    <ComModal mobileFullscreen persistent @onClose="onClose()" hideOkButton hideCloseButton
        actionClass="flex justify-between">
        <template #content>
            <div class="-mt-2 -mx-2 h-60 bg-cover bg-no-repeat bg-center"
                v-bind:style="{ 'background-image': 'url(' + (product.photo || '/assets/frappe/images/emenu_placeholder.jpg') + ')' }">
            </div>
            <div class="flex justify-between items-center">
                <div>
                    <h2 class="pt-3 font-bold">{{product.name}} - {{ product.name_en }}</h2>
                    <h3 v-if="product.name_en!=product.name_kh">{{ product.name_kh }}</h3>
                </div>
                <div class="pt-3">     
                    <span class="text-lg font-bold text-right" :style="{ color: gv.setting?.template_style?.title_color }"
                    :class="gv.setting?.template_style?.title_class">
                    <CurrencyFormat :value="getPortionPriceSelected()" />
                </span>              
                </div>
            </div>
            <div class="py-3">
                <hr />
            </div>
            <div v-html="product.description"></div>
            <ComCheckPortion v-if="product_prices.length > 0" :portions="product_prices" :title_color="gv.setting?.template_style?.title_color"/>
            <ComCheckModifier v-if="product_modifiers.length > 0" :modifiers="product_modifiers" :title_color="gv.setting?.template_style?.title_color" :business_branch="gv.pos_profile.business_branch"/>
        </template>
        <template #action v-if="gv.setting.allow_make_order==1">
            <div class="flex justify-between grow border-t pt-2">
                <div class="grow flex items-center">
                    <button class="bg-black w-8 h-8 rounded-sm" type="button" @click.stop="onQtyChanged(-1)" :disabled="qty<=1">
                        <v-icon>mdi-minus</v-icon>
                    </button>
                    <div class="h-8 px-2 flex items-center justify-center text-lg" style="min-width: 40px;">{{ qty }}</div>
                    <button type="button" @click.stop="onQtyChanged(1)" class="bg-black w-8 h-8 rounded-sm">
                        <v-icon>mdi-plus</v-icon>
                    </button>
                </div>
                <button type="button" class="h-8 px-4 rounded-sm text-white" @click.stop="onAddtoCart(product)"
                    :style="{ 'background-color': gv.setting?.template_style?.title_color }"><v-icon>mdi-cart-outline</v-icon>
                    Add to card</button>
            </div>
        </template>
    </ComModal>
</template>
<script setup>
    import { inject, ref,onMounted } from 'vue'
    import ComModal from '../../components/ComModal.vue';
    import ComCheckPortion from './components/ComCheckPortion.vue';
    import ComCheckModifier from './components/ComCheckModifier.vue';
    import CurrencyFormat from '../components/CurrencyFormat.vue';
    import { createToaster } from '@meforma/vue-toaster';
    import Enumerable from 'linq';
	
	const toaster = createToaster({position:'top'});
 
    const props = defineProps({
        product: Object
    })
    let qty = ref(1)

    const product_prices = ref([]);
    const product_modifiers = ref([]);
    const gv = inject("$gv");
    const sale = inject("$sale");

    const emit = defineEmits(['onCloseModal'])
    function onClose() {
        emit('onCloseModal')
    }

    onMounted(()=>{
        getProductPrices(props.product)
        getProductModifiers(props.product)
    })

    function getProductPrices(p){
        const prices = JSON.parse(p.prices)?.filter(r => (r.branch ==gv.pos_profile.business_branch || r.branch == '') && r.price_rule == gv.pos_profile.price_rule);
        prices.forEach(_p => {
           _p.selected = false; 
           product_prices.value.push(_p)
        });

        if(product_prices.value.length>0)
        { 
            product_prices.value[0].selected = true;
        } 
    }

    function getProductModifiers(p){
        const modifiers = JSON.parse(p.modifiers);
        modifiers.forEach((_m)=>{ 
            product_modifiers.value.push(_m)
        }) 
    }

    function getPortionPriceSelected(){
        const prices =  product_prices.value.filter((r)=>(r.selected||false)); 
        let _modifier_amount = 0;
        product_modifiers.value.forEach((c)=>{
            c.items.filter((r)=>(r.selected??false)).forEach(e=>{ 
                _modifier_amount += e.price;
            })
        })
   
        if(prices.length>0){
            return prices[0].price + _modifier_amount;
        }

        if(product_prices.value.length<=0){
            return (props.product?.price??0) + _modifier_amount;
        }
        return _modifier_amount;
    }

    function onModifierValidate(p){

        let is_required = false;
        product_modifiers.value.forEach((c) => {
            const countItem = c.items.filter(r => r.branch == gv.pos_profile.business_branch || r.branch == '').length
            if (countItem > 0) {
                if (c.is_required == 1) { 
                    if (c.items.filter(r => (r.selected??false)).length == 0) {
                        toaster.warning("Please select a modifier of " + c.category); 
                        is_required =  true
                    }
                }
            } 
        });


        return !is_required;
    }

    function onAddtoCart(p){ 
      if( onModifierValidate(p)){
        let _portion = {}
        if(product_prices.value.length>0){
            const pp = product_prices.value.filter((r)=>(r.selected??false));
            if(pp.length>0){
                _portion = pp[0]
            }
        }

        let _modifiers = {}
        if(product_modifiers.value.length>0){

            const selected = (Enumerable.from(product_modifiers.value).selectMany("$.items").where("$.selected==true").orderBy("$.modifier"));
            let modifiers = selected.select("r=>(r.prefix || '') + ' ' + r.modifier").toJoinedString(", ");
            if (modifiers == "[]" || modifiers == undefined) {
                modifiers = "";
            }

            _modifiers = {
                modifiers_data: selected.select("x => {name:x['name'], modifier: x['modifier'], price: x['price'] }").toJSONString(),
                modifiers: modifiers,
                price: selected.sum("$.price")
            }
        }
        
        sale.onAddtoCart(p,qty.value,_portion,_modifiers)
      }
    }

    function onQtyChanged(symbol){
        qty.value = qty.value + symbol
    }

</script> 
<style>
.v-list-item--variant-text .v-list-item__overlay {
    background: transparent !important;
}
</style>