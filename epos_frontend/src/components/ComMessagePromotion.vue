<template>
    <div class="mb-2"> 
        <v-alert v-for="(promotion, index) in gv.promotion" :key="index" icon="mdi-tag-multiple" close-icon="#fba322" color="transparent" elevation="1">
 
            <div class="text-xs">{{$t('Today Promotion')}}</div>
            <div class="font-bold uppercase">{{promotion.promotion_name}}</div>
            <div class="flex items-center">
                <v-icon size="x-small" icon="mdi-clock"></v-icon>
                <div class="text-sm ml-1">{{promotion.start_time}} - {{promotion.end_time}}</div>
            </div>
            <div class="pt-1" v-if="gv.promotion.customer_groups && gv.promotion.customer_groups.length > 0">
                <div class="font-bold text-sm underline">{{$t('On Customer')}}:</div>
                <div class="-mx-1">
                    <v-chip class="m-1" size="small" v-for="(c, index) in gv.promotion.customer_groups" :key="index">{{c.customer_group_name_en}}</v-chip>
                </div>
            </div>
            <div>{{gv.promotion.note}}</div>
        </v-alert>
    </div>
</template>
<script setup>
    import {inject} from 'vue'
    const gv = inject('$gv')
    const moment = inject('$moment')
</script>
<style lang="">
    
</style>