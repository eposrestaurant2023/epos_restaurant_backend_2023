<template>
    <div>
        <v-btn color="error" size="x-small" variant="tonal" icon="mdi-arrow-down"
            @click="sale.updateQuantity(saleProduct, saleProduct.quantity - 1)"
            :disabled="saleProduct.quantity == 1"></v-btn>
        <v-btn class="mx-1" size="small" variant="tonal" @click="onChangeQuantity">{{ saleProduct.quantity }}</v-btn>
        <v-btn color="success" size="x-small" variant="tonal" icon="mdi-arrow-up"
            @click="sale.updateQuantity(saleProduct, saleProduct.quantity + 1)"></v-btn>
    </div>
</template>
<script setup>
import { inject, keyboardDialog } from '@/plugin'
const props = defineProps({
    saleProduct: Object
})
const sale = inject("$sale");

async function onChangeQuantity() {
    const result = await keyboardDialog({ title: "Change Quantity", type: 'number', value: props.saleProduct.quantity });
    if (result) {
        let quantity = sale.getNumber(result);

        if (quantity == 0) {
            quantity = 1
        }
        sale.updateQuantity(props.saleProduct, quantity);
    }
}
</script>
<style lang="">
    
</style>