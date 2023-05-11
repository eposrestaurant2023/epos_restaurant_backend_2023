<template>
    <template v-if="promotions.length > 0">
        <ComChip v-for="(item, index) in promotions" :key="index" color="orange" tooltip="Happy Hour Promotion" prepend-icon="mdi-tag-multiple">{{ item.promotion_name }}</ComChip>
    </template>
    <v-chip v-else-if="customer.default_discount > 0" color="error">{{ customer.default_discount }} % OFF</v-chip>
</template>
<script setup>
    import {inject,ref} from 'vue'
    const gv = inject('$gv')
    const props = defineProps({
        customer: Object
    }) 
    let promotions = ref([])
    promotions.value = gv.getPromotionByCustomerGroup(props.customer.customer_group)
</script>
<style lang="">
    
</style>