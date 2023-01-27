<template>
    <v-dialog v-model="open" width="100%" max-width="800px">
        <div class="bg-white rounded-md overflow-hidden">
            <ComToolbar @onClose="onClose">
                <template #title>
                    Choose Product Portion & Modifier
                </template>
            </ComToolbar>
            <div>
                <div style="max-height: calc(100vh - 254px);" class="overflow-auto p-4">
                    <div class="mb-4">
                        <ComInput prepend-inner-icon="mdi-magnify" keyboard :value="keyword" v-debounce="onSearch" @onInput="onSearch" placeholder="Search Portion & Modifier"/>
                    </div>
                    <div>
                        <div>
                        <v-chip  closable  @click:close="onRemoveModifier(item)" class="ml-2 mb-2 mt-0"  v-for="(item, index) in product.getSelectedModierList()" :key="index">
                           {{item.prefix}} {{item.modifier}} - <CurrencyFormat :value="item.price"/>
                        </v-chip>
 
                        </div>
                        <v-expansion-panels v-model="panelPortion" multiple variant="accordion">
                            <v-expansion-panel title="Portion" v-if="product.prices.length>1">
                                <v-expansion-panel-text>
                                    <div class="flex flex-wrap">
                                        <div class="m-1" v-for="(item, i) in product.prices" :key="i">
                                            <ComPortionItem :portion="item" @click="product.onSelectPortion(item)" />
                                        </div>
                                    </div>
                                </v-expansion-panel-text>
                            </v-expansion-panel>
                            <template v-for="(item, index) in product.modifiers" :key="index">
                                <v-expansion-panel v-if="product.getModifierItem(item).length>0" class="mt-2" variant="accordion">
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
                </div>
            </div>
            <v-divider></v-divider>
            <div class="text-right p-2">
                <v-btn color="error" @click="onClose" class="mr-2">Close</v-btn>
                <v-btn color="primary" @click="onConfirm">OK</v-btn>
            </div>
        </div>
    </v-dialog>
</template>
  
<script setup>
import { ref,defineEmits,inject } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComModifierItem from './ComModifierItem.vue';
import ComInput from '../../../components/form/ComInput.vue';
import ComPortionItem from './ComPortionItem.vue';
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
const product = inject("$product")
product.keyword = "";
let keyword = ref()
const panelPortion = ref([0,1,2,3,4,5,6,7,8,9])
const emit = defineEmits(["resolve","reject"])
 
const open = ref(true);


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
</style>