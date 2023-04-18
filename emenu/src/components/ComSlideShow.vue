<template>
    <div>
        <v-carousel v-if="gv.setting?.slideshow" show-arrows="hover" hide-delimiters cycle :height="mobile ? 300 : 400">
            <v-carousel-item
                v-for="(item, index) in gv.setting?.slideshow" :key="index"
                :src="item.photo"
                cover
            ></v-carousel-item>
        </v-carousel>
 
    </div>
</template>
<script setup>
import {inject} from 'vue'
import {useDisplay} from 'vuetify'
const {mobile} = useDisplay()
const gv = inject('$gv')
</script>
<style lang="">
    
</style>