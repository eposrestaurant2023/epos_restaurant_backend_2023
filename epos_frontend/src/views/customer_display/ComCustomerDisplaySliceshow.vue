<template lang="">
    <div>
        <v-carousel height="100vh" cycle :show-arrows="false" hide-delimiters :interval="6000" vertical-delimiters>
            <v-carousel-item :src="item.photo" cover v-for="(item, index) in gv.setting.pos_setting.customer_display_slideshow" :key="index"></v-carousel-item> 
        </v-carousel>
    </div>
</template>
<script setup>
import { inject } from '@/plugin';
const gv = inject("$gv")
</script>
<style lang="">
    
</style>