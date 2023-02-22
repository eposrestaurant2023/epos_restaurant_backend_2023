<template>
    <v-dialog v-model="open" @update:modelValue="onClose" >
        <v-card
            class="mx-auto my-2 py-2"
            title="Change Price Rule">
            <v-card-text> 
                    <ComPlaceholder :is-not-empty="price_rules.length > 0">
                        <v-list class="!p-0">
                            <v-list-item v-for="(item, index) in price_rules" :key="index" class="!p-0">
                                <v-btn
                                    class="w-full"
                                    color="primary"
                                    :disabled="selectedPriceRule == item.price_rule"
                                    @click="onSelect(item.price_rule)">
                                    {{ item.price_rule }}
                                </v-btn> 
                            </v-list-item>
                        </v-list>
                    </ComPlaceholder> 
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn variant="flat" @click="onClose(false)" color="error">
                    Close
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, defineEmits, inject, confirm } from '@/plugin'
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';
const emit = defineEmits(['resolve'])
const props = defineProps({
    params:Object
}) 

const sale  = inject('$sale')
const product  = inject('$product')
const price_rules  = JSON.parse(localStorage.getItem('setting')).price_rules;
let open = ref(true)
let selectedPriceRule = ref(sale.setting.price_rule)
 
function onClose() {
    emit('resolve',false)
}
async function onSelect(result){
    if(sale.sale.sale_products.length > 0){
        const yesNo = await confirm({title: 'Are you sure to change price rule?',text: 'Notice : The products will delete all.'})
        if(yesNo == true){
            sale.sale.sale_products = []
            await onConfrim(result)
        }
    }else{
        await onConfrim(result)
    }
}
async function onConfrim(result){
    sale.sale.price_rule = result;
    sale.setting.price_rule = result;
    product.parentMenu = '';
    product.searchProductKeyword= '';
    product.searchProductKeywordStore= '';
    product.selectedProduct = {};
    await emit('resolve', true)
}
</script>