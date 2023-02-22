<template>
    <v-btn class="text-none" stacked :size="mobile ? 'x-small' : 'small'" @click="onSearchSale">
        <v-badge :content="sale.total_pending_sale" color="success">
            <v-icon>mdi-cart</v-icon>
        </v-badge>
    </v-btn>
</template>
<script setup>
import { useDisplay } from 'vuetify'
import {searchSaleDialog, inject} from '@/plugin'
const { mobile } = useDisplay()
const sale = inject('$sale')

async function onSearchSale(){
    const result = await searchSaleDialog({ })
    if(result != false){ 
        sale.LoadSaleData(result.name)
    }
}
</script>