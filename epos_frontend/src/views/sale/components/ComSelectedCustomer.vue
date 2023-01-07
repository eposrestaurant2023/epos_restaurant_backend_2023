<template lang="">
    <div>
        <input v-model="$store.state.sale.customer"/>
        {{$store.state.sale.customer}}
    </div>
</template>
<script>
export default {
    name: "ComSelectedCustomer"
}
</script>
<style lang="">
    
</style>