<template>
    <!-- on small sreen -->
    <div class="block md:hidden lg:hidden xl:hidden 2xl:hidden">
        <ComPrintButton v-if="isDesktop" doctype="Sale" title="Print All Bill" @onPrint="$emit('onPrintAllBill')" />
        <v-btn class="grow" variant="flat" color="success" @click="$emit('onNewOrder')">New Sale Order</v-btn>
        <v-menu>
        <template v-slot:activator="{ props }">
            <v-btn variant="flat" v-bind="props" color="info">
                More
            </v-btn>
        </template>
        <v-list>
            <v-list-item v-if="isBillRequested" color="error" @click="$emit('onCancelPrintBill')">
                <v-list-item-title>Cancel Print Bill</v-list-item-title>
            </v-list-item>
            <v-list-item @click="$emit('onQuickPay',true)">
                <v-list-item-title>Quick Pay</v-list-item-title>
            </v-list-item>
            <v-list-item @click="$emit('onQuickPay',false)">
                <v-list-item-title>Quick Pay without Print</v-list-item-title>
            </v-list-item>
        </v-list>
        </v-menu>
        
        <v-btn class="grow" variant="flat" color="error" @click="$emit('onClose')">Cancel</v-btn>
    </div>
    <div class="hidden md:block lg:block xl:block 2xl:block">
        <ComPrintButton v-if="isDesktop" doctype="Sale" title="Print All Bill" @onPrint="$emit('onPrintAllBill')" />
        <v-btn v-if="isBillRequested" class="grow" variant="flat" color="warning" @click="$emit('onCancelPrintBill')">Cancel Print Bill</v-btn>
        <v-btn class="grow" variant="flat" color="primary" @click="$emit('onQuickPay',true)">Quick Pay</v-btn>
        <v-btn class="grow" variant="flat" color="primary" @click="$emit('onQuickPay',false)">Quick Pay without Print</v-btn>
        <v-btn class="grow" variant="flat" color="success" @click="$emit('onNewOrder')">New Sale Order</v-btn>
        <v-btn class="grow" variant="flat" color="error" @click="$emit('onClose')">Cancel</v-btn>
    </div>
</template>
<script setup>
import ComPrintButton from '@/components/ComPrintButton.vue';
    import {defineProps} from 'vue'
    const props = defineProps({
        isBillRequested: {
            type: Boolean,
            default: false
        },
        isDesktop: {
            type: Boolean,
            default: false
        }
    })
</script>
<style lang="">
    
</style>