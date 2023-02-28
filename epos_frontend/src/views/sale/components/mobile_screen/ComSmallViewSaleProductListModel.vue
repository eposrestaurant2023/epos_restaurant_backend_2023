<template>
    <ComModal :fullscreen="true" @onClose="onClose()" :hideCloseButton="true" :hideOkButton="true" :fill="true">
        <template #title>
            Sale# {{ params.title }}
        </template>
        <template #bar_custom>
            <ComPrintBillButton v-if="sale.sale.sale_status != 'Bill Requested'" doctype="Sale" title="Print Bill" :mobile="true"/>
        </template>
        <template #bar_more_button>
            <v-card>
                <v-list density="compact">
                    <ComButtonToTableLayout :is-mobile="true" @closeModel="onClose()" />
                    <v-list-item @click="onQuickPay()">
                        <template v-slot:prepend class="w-12">
                            <v-icon icon="mdi-currency-usd"></v-icon>
                        </template>
                        <v-list-item-title>Quick Pay</v-list-item-title>
                    </v-list-item>
                    <ComDiscountButtonList />
                    <ComSaleButtonMoreList />
                </v-list>
            </v-card>
        </template>
        <template #content>
            <ComGroupSaleProductList />
        </template>
        <template #action>
            <ComSmallSaleSummary />
        </template>
    </ComModal>
</template>
<script setup>
import { defineProps, defineEmits, inject } from '@/plugin'
import { useDisplay } from 'vuetify'
import ComToolbar from '../../../../components/ComToolbar.vue';
import ComButtonToTableLayout from '../ComButtonToTableLayout.vue';
import ComDiscountButtonList from '../ComDiscountButtonList.vue';
import ComGroupSaleProductList from '../ComGroupSaleProductList.vue';
import ComPrintBillButton from '../ComPrintBillButton.vue';
import ComSaleButtonMoreList from '../ComSaleButtonMoreList.vue';
import ComSmallSaleSummary from './ComSmallSaleSummary.vue';
const { mobile } = useDisplay()
const props = defineProps({
    params: Object
})
const sale = inject('$sale')
const emit = defineEmits(['resolve'])
async function onQuickPay() {

    await sale.onSubmitQuickPay().then((value) => {
        if (value) {
            router.push({ name: "TableLayout" });
            onClose()
        }
    });
}
function onClose() {
    sale.mobile_view_sale_product = false
    emit('resolve', false)
}
</script>