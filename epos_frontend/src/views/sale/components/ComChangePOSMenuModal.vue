<template>
    <ComModal @onClose="onClose()" :hide-ok-button="true" width="350px">
        <template #title>
            <div>{{ $t('Change POS Menu') }}</div>
        </template>
        <template #content>
            <ComPlaceholder :is-not-empty="pos_menus.length > 0">
                <v-list class="!p-0">
                    <v-list-item v-for="(item, index) in pos_menus" :key="index" class="!p-0">
                        <v-btn class="w-full" color="primary" :disabled="selected == item.pos_menu"
                            @click="onSelect(item.pos_menu)">
                            {{ item.pos_menu }}
                        </v-btn>
                    </v-list-item>
                </v-list>
            </ComPlaceholder>
        </template>
    </ComModal>
</template>
<script setup>
import { ref, defineEmits, inject, confirm } from '@/plugin'
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';
const emit = defineEmits(['resolve'])
const props = defineProps({
    params: Object
})

const sale = inject('$sale')
const product = inject('$product')
const pos_menus = JSON.parse(localStorage.getItem('setting')).pos_menus;
let selected = ref(sale.setting.pos_menus)

function onClose() {
    emit('resolve', false)
}
function onSelect(result) {
    onConfrim(result)
}
function onConfrim(result) {
    product.currentRootPOSMenu = result;
    product.parentMenu = '';
    product.searchProductKeyword = '';
    product.searchProductKeywordStore = '';
    product.selectedProduct = {};
    emit('resolve', true)
}
</script>