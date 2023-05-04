<template>
    <ComModal
        :mobileFullscreen="true"
        @onClose="onClose"
        @onOk="onConfirm"
        titleButtonOk="OK"
        >
        <template #title>
            Choose Combo Products
        </template>
        <template #content>
            <div class="mb-4">
                        <ComInput prepend-inner-icon="mdi-magnify" keyboard :value="keyword" v-debounce="onSearch" @onInput="onSearch" placeholder="Search Combo Item"/>
                    </div>
                    <div>
                        <v-expansion-panels v-model="panelPortion" multiple variant="accordion">
                            <template v-for="(item, index) in product.combo_group_temp" :key="index">
                                <v-expansion-panel v-if="item.menus.length>0" class="mt-2" variant="accordion" :class="mobile ? 'panel-small' : ''">
                                   <template #title>
                                    <span>{{  item.pos_title  }}</span> 
                                    <span v-if="item.item_selection > 0" class="text-red-500 mx-2 text-xs">* Selection {{ item.item_selection }}</span>
                                   </template>
                                    <v-expansion-panel-text>
                                        <ComPlaceholder :is-not-empty="product.getComboMenu(item).length > 0">
                                            <div class="p-2">
                                                <div class="grid gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-4 2xl:grid-cols-4'">
                                                    <div v-for="(m, index) in product.getComboMenu(item)" :key="index" class="h-36">
                                                        <ComSaleProductComboMenuGroupItem :product="m" :group="item"/>
                                                    </div>
                                                </div>
                                            </div>
                                        </ComPlaceholder>
                                    </v-expansion-panel-text>
                                </v-expansion-panel>
                            </template>
                        </v-expansion-panels>
                    </div>
        </template>
    </ComModal>
</template>
  
<script setup>
import { ref,defineEmits,inject, createToaster } from '@/plugin'
import ComInput from '@/components/form/ComInput.vue';
import { useDisplay } from 'vuetify'
import ComSaleProductComboMenuGroupItem from './ComSaleProductComboMenuGroupItem.vue';
const { mobile } = useDisplay()
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
const toaster = createToaster({position: 'top'})
const product = inject("$product")
product.keyword = "";
let keyword = ref()
const panelPortion = ref([0,1,2,3,4,5,6,7,8,9])
const emit = defineEmits(["resolve","reject"])

function onConfirm(){
    product.validateComboGroup().then((value)=>{
      if(value){
        
        emit("resolve",{
            combo_groups:product.getSelectedComboGroup()
        }) 
      }
    })
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