<template>
    <div>
        xxx
    </div>
</template>
<script setup>
    import {inject} from 'vue'
    const fdb = inject('$fdb')
 
    fdb.getDoc('Customer', '000001')
    .then((doc) => console.log(doc))
    .catch((error) => console.error(error));
</script>
<style lang="">
    
</style>