<template>
    <v-dialog :scrollable="false" v-model="open" fullscreen @update:modelValue="onClose">
        <v-card class="mx-auto my-0" >
            <ComToolbar @onClose="onClose">
                <template #title>
                    Currency Prefine
                </template>
            </ComToolbar>
            <v-card-text class="overflow-y-auto !p-2">
                <ComPaymentCurrencyPrefine @onSelected="onClose()" :is-mobile="true"/>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn class="mr-2" variant="flat" @click="onClose()" color="error">
                    Close
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { defineProps, defineEmits,inject } from '@/plugin'
import ComToolbar from '../../../../components/ComToolbar.vue';
import ComPaymentCurrencyPrefine from '../ComPaymentCurrencyPrefine.vue';
const open = true
const props = defineProps({
    params: Object
}) 
const emit = defineEmits(['resolve'])

function onClose() {
    emit('resolve', false)
}
</script>