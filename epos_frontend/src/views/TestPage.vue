<template>
     <div>
    <p>Screen width is {{ $screen.width }}</p>
    <p>Screen height is {{ $screen.height }}</p>
    <p>Current breakpoint is {{ $grid.breakpoint }}</p>
  </div>
    </template>

    <script setup>
    import {useScreen} from 'vue-screen'
import { inject } from "vue";
const sale = inject("$sale")
const $screen = useScreen()
function add(){
    sale.addSaleProduct("Product 1")
}
console.log($screen.height)
</script>