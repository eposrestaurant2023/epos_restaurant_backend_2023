<template>
    <div>

        <v-btn
            v-if="saleProduct.sale_product_status == 'New' || sale.setting.pos_setting.allow_change_quantity_after_submit == 1"
            color="error" size="x-small" variant="tonal" icon="mdi-arrow-down"
            @click="sale.updateQuantity(saleProduct, saleProduct.quantity - 1)"
            :disabled="saleProduct.quantity == 1"></v-btn>
        <v-btn class="mx-1" size="small" variant="tonal" @click="sale.onChangeQuantity(saleProduct, gv)">{{
            saleProduct.quantity }}</v-btn>
        <v-btn
            v-if="saleProduct.sale_product_status == 'New' || sale.setting.pos_setting.allow_change_quantity_after_submit == 1"
            color="success" size="x-small" variant="tonal" icon="mdi-arrow-up"
            @click="sale.updateQuantity(saleProduct, saleProduct.quantity + 1)"></v-btn>
    </div>
</template>
<script setup>
import { inject } from '@/plugin'
const props = defineProps({
    saleProduct: Object
})
const sale = inject("$sale");
const gv = inject("$gv");

if (props.saleProduct.selected) {
    sale.vue.$onKeyStroke('+', (e) => {
        e.preventDefault()
        if (props.saleProduct.selected) {
            sale.updateQuantity(props.saleProduct, props.saleProduct.quantity + 1)
        }

    })
    sale.vue.$onKeyStroke('-', (e) => {
        e.preventDefault()
        if (props.saleProduct.selected && props.saleProduct.quantity > 1) {
            sale.updateQuantity(props.saleProduct, props.saleProduct.quantity - 1)
        }
    })
    sale.vue.$onKeyStroke('F3', (e) => {
        e.preventDefault()
        if (props.saleProduct.selected) {
            sale.onChangeQuantity(props.saleProduct, props.saleProduct.quantity - 1)
        }
    })
    sale.vue.$onKeyStroke('F4', (e) => {
        e.preventDefault()
        if (props.saleProduct.selected) {
            sale.onChangePrice(props.saleProduct)
        }
    })
}


</script>
<style lang="">
    
</style>