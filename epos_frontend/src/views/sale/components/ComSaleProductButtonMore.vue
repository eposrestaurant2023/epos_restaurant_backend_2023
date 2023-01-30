<template >
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-chip v-bind="props" color="primary" class="mx-1 grow text-center justify-center" size="small">More</v-chip>
        </template>
        <v-list>
            <v-list-item prepend-icon="mdi-currency-usd-off" title="Free" v-if="!saleProduct.is_free" @click="sale.onSaleProductFree(saleProduct)"></v-list-item>
            <v-list-item prepend-icon="mdi-currency-usd-off" title="Cancel Free" v-else @click="sale.onSaleProductCancelFree(saleProduct)"></v-list-item>
            <v-list-item prepend-icon="mdi-chair-school" title="Seat #" @click="sale.onSaleProductSetSeatNumber(saleProduct)"></v-list-item>
            <v-list-item prepend-icon="mdi-note-outline" title="Note" @click="sale.onSaleProductNote(saleProduct)"></v-list-item>
            <v-list-item prepend-icon="mdi-delete" title="Delete" @click="sale.onRemoveSaleProduct(saleProduct, saleProduct.quantity)"></v-list-item>
        </v-list>
    </v-menu>
</template>
<script setup>
    import { defineProps, inject } from 'vue'
    const sale = inject('$sale')
    const props=defineProps({
        saleProduct:Object
    })
</script>