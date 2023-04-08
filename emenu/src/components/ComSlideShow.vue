<template>
    <div>
        <v-carousel show-arrows="hover" hide-delimiters cycle :height="mobile ? 300 : 500">
            <v-carousel-item
                src="https://cdn.vuetifyjs.com/images/cards/docks.jpg"
                cover
            ></v-carousel-item>
            <v-carousel-item
                src="https://cdn.vuetifyjs.com/images/cards/hotel.jpg"
                cover
            ></v-carousel-item>
            <v-carousel-item src="https://cdn.vuetifyjs.com/images/cards/sunshine.jpg" cover></v-carousel-item>
        </v-carousel>
    </div>
</template>
<script setup>
import {useDisplay} from 'vuetify'
const {mobile} = useDisplay()



</script>
<style lang="">
    
</style>