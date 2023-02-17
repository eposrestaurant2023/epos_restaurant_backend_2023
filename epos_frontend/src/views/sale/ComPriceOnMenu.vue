<template>
    <span>
        <span v-if="prices.length > 1">
            <span><CurrencyFormat :value="minPrice"/></span> <v-icon icon="mdi-arrow-right" size="x-small"/> <span><CurrencyFormat :value="maxPrice"/></span>
        </span>
        <CurrencyFormat v-else :value="showPrice"/>
    </span>
</template>
<script setup>
    import Enumerable from 'linq'
    import { defineProps, computed, inject } from 'vue'
    const props = defineProps({
        prices: String,
        price: Number
    })
    const sale = inject('$sale')

    const prices = JSON.parse(props.prices).filter(r=>r.branch == sale.sale.business_branch && r.price_rule == sale.sale.price_rule)
    const showPrice = computed(()=>{
        if(prices.length == 1){
            return prices[0].price
        }
        else if(prices.length == 0){
            return props.price
        }
        return 0
    })
    const maxPrice = computed(()=>{ 
        if(prices.length > 1){
            return Enumerable.from(prices).max("$.price") 
        } 
        return 0
    })
    const minPrice = computed(()=>{ 
        if(prices.length > 1){
            return Enumerable.from(prices).min("$.price")
        } 
        return 0
    })
</script>