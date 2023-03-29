<template>
    <ComModal :fullscreen="true" @onClose="onClose()" :hideCloseButton="true" :hideOkButton="true" :fill="true" :isShowBarMoreButton="false">
        <template #title>
            Sale# {{ params.title }}
        </template>
        <template #bar_custom>
            <v-btn v-if="params.data?.from_table" icon @click="onAddNewProduct()" v-bind="props">
                <v-icon>mdi-plus</v-icon>
            </v-btn>
            <ComPrintBillButton v-if="sale.sale.sale_status != 'Bill Requested'" doctype="Sale" title="Print Bill" :mobile="true" />
        </template>
        <template #content>
            <ComGroupSaleProductList />
        </template>
        <template #action>
            <ComSmallSaleSummary @onClose="onGoHome()" />
        </template>
    </ComModal>
</template>
<script setup>
import { defineProps, defineEmits, inject,useRouter } from '@/plugin'
import ComGroupSaleProductList from '../ComGroupSaleProductList.vue';
import ComPrintBillButton from '../ComPrintBillButton.vue';
import ComSmallSaleSummary from './ComSmallSaleSummary.vue';

const props = defineProps({
    params: Object
})
const sale = inject('$sale')
const gv = inject('$gv')
const emit = defineEmits(['resolve'])
const router = useRouter();

function onGoHome(){
    if (gv.setting.table_groups.length > 0) {
        sale.sale = {};
        router.push({ name: 'TableLayout' })
    }
    else {
        sale.newSale()
        router.push({ name: "AddSale" });
    }
    emit('resolve', false)
}

function onClose() {  
    emit('resolve', false)
}
function onAddNewProduct(){
    sale.no_loading = true
    onClose()
    router.push({
        name: "AddSale", params: {
            name: sale.sale.name
        }
    });
}
</script>