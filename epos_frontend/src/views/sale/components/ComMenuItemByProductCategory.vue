<template >
    <div v-for="(m, index) in product.menuProducts" :key="index" class="h-36" >
        <ComMenuItem :data="m" />
    </div>
</template>
<script setup>
    import {inject } from "@/plugin"
    import ComMenuItem from './ComMenuItem.vue';
    const product = inject("$product")
    
</script>