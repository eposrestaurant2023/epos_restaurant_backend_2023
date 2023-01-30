<template> 
    <v-menu>
        <template v-slot:activator="{ props }">
            <div class="cursor-pointer p-2 grow bg-blue-600 hover:bg-blue-700 text-white" v-bind="props">
                Discount
            </div>
        </template>
        <v-list>
            <v-list-item prepend-icon="mdi-percent" @click="onDiscountByPercentage(r)">
                <v-list-item-title>Percentage</v-list-item-title>
            </v-list-item>
            <v-list-item  @click="onDiscountByAmount(r)" prepend-icon="mdi-currency-usd">
                <v-list-item-title>Amount</v-list-item-title>
            </v-list-item>
        </v-list>
    </v-menu>       
</template>
<script setup>
    import {defineProps,defineEmits, createResource} from "@/plugin"
</script>