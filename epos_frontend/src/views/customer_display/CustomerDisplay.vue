<template>
    
{{ sale }}
</template>
<script setup>
    import { inject } from '@/plugin';
    import { ref } from 'vue';
    const socket = inject("$socket")
    const gv = inject("$gv")
    const sale = ref({})
    const open = ref(true)

    socket.on("ShowOrderInCustomerDisplay", (arg) => {
        sale.value = arg;
    })
    


 
</script>