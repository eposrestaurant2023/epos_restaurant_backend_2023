<template>
    <ComModal :fullscreen="true" @onClose="onClose()" :hideCloseButton="true">
        <template #title>
            {{ $t('Currency Predefine') }}
        </template>
        <template #content>
            <ComPaymentCurrencyPrefine @onSelected="onClose()" :is-mobile="true"/>
        </template> 
    </ComModal>
</template>
<script setup>
import { defineProps, defineEmits,inject } from '@/plugin'
import ComToolbar from '../../../../components/ComToolbar.vue';
import ComPaymentCurrencyPrefine from '../ComPaymentCurrencyPrefine.vue';
let open = true
const props = defineProps({
    params: Object
}) 
const emit = defineEmits(['resolve'])

function onClose() {
    emit('resolve', false)
}
</script>