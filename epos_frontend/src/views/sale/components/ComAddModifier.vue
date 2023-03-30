<template>
    <ComModal
        :mobileFullscreen="true"
        @onClose="onClose"
        @onOk="onConfirm"
        titleButtonOk="OK"
        >
        <template #title>
            Choose Product Portion & Modifier
        </template>
        <template #content>
            <div class="mb-4">
                        <ComInput prepend-inner-icon="mdi-magnify" keyboard :value="keyword" v-debounce="onSearch" @onInput="onSearch" placeholder="Search Portion & Modifier"/>
                    </div>
                    <div>
                        
                        <div>
                            <v-chip :size="mobile ? 'large' : 'x-large'" closable  
                            @click:close="onRemoveModifier(item)" 
                            class="m-1" 
                             v-for="(item, index) in product.getSelectedModierList()" 
                             :key="index">
                            {{item.prefix}} {{item.modifier}} - <CurrencyFormat :value="item.price"/>
                            </v-chip>
                        </div> 
                        <v-expansion-panels v-model="panelPortion" multiple variant="accordion">
                            <v-expansion-panel title="Portion" v-if="product?.prices?.filter(r=>r.price_rule == sale.sale.price_rule).length>1" :class="mobile ? 'panel-small' : ''">
                                <v-expansion-panel-text>
                                    <div class="flex flex-wrap">
                                        <div class="m-1" v-for="(item, i) in product.prices.filter(r=>r.price_rule == sale.sale.price_rule)" :key="i">
                                            <ComPortionItem :portion="item" @click="product.onSelectPortion(item)" />
                                        </div>
                                    </div>
                                </v-expansion-panel-text>
                            </v-expansion-panel>
                            <template v-for="(item, index) in product.modifiers" :key="index">
                                <v-expansion-panel v-if="product.getModifierItem(item).length>0" class="mt-2" variant="accordion" :class="mobile ? 'panel-small' : ''">
                                   <template #title>
                                    <span>{{  item.category  }}</span> 
                                    <span v-if="item.is_required" class="text-red-500 mx-2 text-xs">* Required</span>
                                   </template>
                                    <v-expansion-panel-text>
                                        <div class="flex flex-wrap">
                                            <template v-for="(m, i) in product.getModifierItem(item)" :key="i">
                                                <ComModifierItem :modifier="m" @click="product.onSelectModifier(item,m)" />
                                            </template>
                                        </div>
                                    </v-expansion-panel-text>
                                </v-expansion-panel>
                            </template>
                        </v-expansion-panels>
                    </div>
        </template>
    </ComModal>
</template>
  
<script setup>
import { ref,defineEmits,inject } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComModifierItem from './ComModifierItem.vue';
import ComInput from '../../../components/form/ComInput.vue';
import ComPortionItem from './ComPortionItem.vue';
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
const product = inject("$product")
const sale = inject("$sale")
product.keyword = "";
let keyword = ref()
const panelPortion = ref([0,1,2,3,4,5,6,7,8,9])
const emit = defineEmits(["resolve","reject"])

function onConfirm(){
    product.validateModifier().then((value)=>{
      if(value){
        emit("resolve",{
            portion:product.getSelectedPortion(),
            modifiers:product.getSelectedModifier()
        })
      } 

    })
    
}

function onRemoveModifier(d){
    d.selected = false;
}

function onClose() {
    emit('reject',false);
}
function onSearch(keyword){
    product.keyword =keyword;
}
</script>
<style>
.v-expansion-panel--active > .v-expansion-panel-title {
    min-height: auto !important;
}
.v-expansion-panel-text__wrapper {
    padding: 8px 8px 12px !important;
}
.v-expansion-panel-title {
    padding: 12px !important;
}
</style>