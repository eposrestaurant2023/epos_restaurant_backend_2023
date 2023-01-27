<template>
    <div>{{sale}}

        <v-btn @click="add">Add Me</v-btn>
    </div>
    </template>

    <script setup>
import { inject } from "vue";
const sale = inject("$sale")

function add(){
    sale.addSaleProduct("Product 1")
}
</script>