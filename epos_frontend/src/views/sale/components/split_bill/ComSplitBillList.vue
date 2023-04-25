<template lang=""> 
     <div class="grid gap-2 sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-3">
        <v-card v-for="(g, index) in data" :key="index">
            <v-card-title class="!p-0">
                <v-toolbar height="55">
                    <v-toolbar-title class="text">
                        <span>#: {{ g.no }}</span> - <span>{{g.sale.name}}</span>
                    </v-toolbar-title>
                    <template v-slot:append>
                       
                    </template>
                </v-toolbar>
            </v-card-title>
            <v-card-text class="!pt-0 !pr-0 !pb-14 !pl-0"> 
                <v-list>
                    <v-list-item v-for="(sp, _index) in g.sale.sale_products" :key="_index">
                        <v-card-title>
                            <span>{{sp.product_name}}</span>
                        </v-card-title>
                    </v-list-item> 
                </v-list>
            </v-card-text>
            <v-card-actions class="pt-0 flex items-center justify-between absolute bottom-0 w-full bg-gray-400">
                <ComSplitBillSaleSummary :sale="g.sale"/>
            </v-card-actions>
        </v-card>
    </div>
</template>
<script setup>
import ComSplitBillSaleSummary from './ComSplitBillSaleSummary.vue';
const props = defineProps({
    data: Object
})
</script>
<style lang="">
    
</style>