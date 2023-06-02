<template>
    <ComModal @onClose="onClose()" :hide-ok-button="true" width="350px">
        <template #title>
            <div>{{ $t('Change Price Rule') }}</div>
        </template>
        <template #content>
            <ComPlaceholder :is-not-empty="price_rules.length > 0">
                <v-list class="!p-0">
                    <v-list-item v-for="(item, index) in price_rules" :key="index" class="!p-0">
                        <v-btn class="w-full" color="primary"
                            @click="onSelect(item.price_rule)">
                           {{ item.price_rule }}
                        </v-btn>
                    </v-list-item>
                </v-list>
            </ComPlaceholder>
        </template>
    </ComModal>
</template>
<script setup>
import { defineEmits, inject, confirm,i18n } from '@/plugin'
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';

const { t: $t } = i18n.global;  

const emit = defineEmits(['resolve'])
const props = defineProps({
    params: Object
})

const sale = inject('$sale')
const product = inject('$product')
const price_rules = JSON.parse(localStorage.getItem('setting')).price_rules;

function onClose() {
    emit('resolve', false)
}
async function onSelect(result) {
    if (sale.sale.sale_products.length > 0) {
        const yesNo = await confirm({ title: $t('msg.are you sure to change price rule'), text: $t('msg.All items will be remove from bill') })
        if (yesNo == true) {
            sale.sale.sale_products = []
            await onConfrim(result)
        }
    } else {
        await onConfrim(result)
    }
}
async function onConfrim(result) {
    sale.sale.price_rule = result;
    sale.setting.price_rule = result;
    product.parentMenu = '';
    product.searchProductKeyword = '';
    product.searchProductKeywordStore = '';
    product.selectedProduct = {};
    await emit('resolve', true)
}
</script>