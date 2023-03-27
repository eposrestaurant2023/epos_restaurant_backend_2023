<template>
{{ null ?? 'hello' }}
</template>
<script setup>
    import { inject } from '@/plugin';
    import { ref } from 'vue';
    const gv = inject("$gv")
    const open = ref(true)

 
</script>