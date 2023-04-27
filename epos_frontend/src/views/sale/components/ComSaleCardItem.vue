<template lang="">
    <v-card v-for="(s, index) in data" :key="index">
        <v-card-title class="!p-0">
            <v-toolbar height="48">
                <v-toolbar-title class="text">
                    <span class="text-sm font-bold">#: {{ s.name }}</span> - <Timeago class="!text-sm" :long="false" :datetime="s.modified" />
                </v-toolbar-title>
                <template v-slot:append>
                    <v-chip size="small" class="ma-2" :color="s.sale_status_color" text-color="white">
                        {{ s.sale_status }}
                    </v-chip>
                </template>
            </v-toolbar>
        </v-card-title>
        <v-card-text class="!pt-0 !pr-0 !pb-14 !pl-0">
            <v-list :lines="false" density="compact" class="pa-0">
                <v-list-item title="Table #" v-if="s.tbl_number">
                    <template v-slot:append>
                        {{ s.tbl_number }}
                    </template>
                </v-list-item>
                <v-list-item v-else title="Sale Type">
                    <template v-slot:append>
                        <v-chip size="x-small" :color="s.sale_type_color">{{s.sale_type}}</v-chip>
                    </template>
                </v-list-item>
                <v-list-item title="Guest Cover" v-if="s.guest_cover">
                    <template v-slot:append>
                        {{ s.guest_cover }}
                    </template>
                </v-list-item>
                <v-list-item title="Customer Code">
                    <template v-slot:append>
                        {{ s.customer }}
                    </template>
                </v-list-item>
                <v-list-item title="Customer Name">
                    <template v-slot:append>
                        {{ s.customer_name }}
                    </template>
                </v-list-item>
                <v-list-item title="Total Qty">
                    <template v-slot:append>
                        {{ s.total_quantity }}
                    </template>
                </v-list-item>
                <v-list-item title="Grand Total">
                    <template v-slot:append>
                        <CurrencyFormat :value="s.grand_total" />
                    </template>
                </v-list-item>
            </v-list>
        </v-card-text>
        <v-card-actions class="pt-0 flex items-center justify-between absolute bottom-0 w-full">
            <v-btn variant="tonal" color="primary" @click="onViewSaleOrder(s.name)">
                Sale Detail
            </v-btn>
            <v-btn variant="tonal" color="success" @click="onOpenOrder(s.name)">
                Open Order
            </v-btn>
        </v-card-actions>
    </v-card>
</template>
<script setup>
import { Timeago } from 'vue2-timeago'
const props = defineProps({
    data: Object
})
const emit = defineEmits(['onViewSaleOrder','onOpenOrder'])
function onViewSaleOrder(name){
    emit('onViewSaleOrder',name)
}
function onOpenOrder(name){
    emit('onOpenOrder',name)
}
</script>
<style lang="">
    
</style>