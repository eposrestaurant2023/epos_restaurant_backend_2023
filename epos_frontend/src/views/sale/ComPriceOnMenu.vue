<template>
    <span>
        <span v-if="result.length > 1">
            <span><CurrencyFormat :value="minPrice"/></span> <v-icon icon="mdi-arrow-right" size="x-small"/> <span><CurrencyFormat :value="maxPrice"/></span>
        </span>
        <CurrencyFormat v-else :value="showPrice"/>
    </span>
</template>
<script setup>
    import Enumerable from 'linq'
    import { defineProps, computed, inject,ref } from 'vue'
    const props = defineProps({
        prices: String,
        price: Number
    })
    const sale = inject('$sale')

    const result = ref(JSON.parse(props.prices).filter(r=>(r.branch == sale.sale.business_branch || r.branch == '') && r.price_rule == sale.sale.price_rule))
 
    const showPrice = computed(()=>{
        if(result.value.length == 1){
            return result.value[0].price
        }
        else if(result.value.length == 0){
            return props.price
        }
        return 0
    })
    const maxPrice = computed(()=>{ 
        if(result.value.length > 1){
            return Enumerable.from(result.value).max("$.price") 
        } 
        return 0
    })
    const minPrice = computed(()=>{ 
        if(result.value.length > 1){
            return Enumerable.from(result.value).min("$.price")
        } 
        return 0
    })
</script>