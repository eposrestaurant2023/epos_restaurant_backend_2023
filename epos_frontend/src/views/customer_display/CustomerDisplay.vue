<template>
<ul class="bg-violet-950">
    <li v-for="(p, index) in sale.sale_products" :key="index">
        {{ p.product_code }} - {{ p.product_name }}
    </li>
</ul>
{{ sale }}
</template>
<script setup>
    import { inject } from '@/plugin';
    import { ref } from 'vue';

    const data = ref()

    const socket = inject("$socket")
    const gv = inject("$gv")
    const sale = ref({})
    const open = ref(true)

    socket.on("ShowOrderInCustomerDisplay", (arg) => {
        sale.value = arg;
    })
    


 
</script>