<template> 
    <ComChip v-if="isPromotion" color="orange" tooltip="Happy Hour Promotion" prepend-icon="mdi-cake-variant">{{ gv.promotion.info.promotion_name }}</ComChip>
    <v-chip v-else-if="customer.default_discount > 0" color="error">{{ customer.default_discount }} % OFF</v-chip>
</template>
<script setup>
    import {inject,computed} from 'vue'
    const gv = inject('$gv')
    const props = defineProps({
        customer: Object
    })
    const isPromotion = computed(()=>{
        if(gv.promotion && gv.promotion.customer_groups.length > 0){
            return gv.promotion.customer_groups.filter(r=>r.customer_group_name_en == props.customer.customer_group).length > 0;
        }
    })
</script>
<style lang="">
    
</style>