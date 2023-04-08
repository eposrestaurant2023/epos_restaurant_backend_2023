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
import {inject} from 'vue'
import {useDisplay} from 'vuetify'
const {mobile} = useDisplay()
const frappe = inject('$frappe')
const call = frappe.call()
call.get('epos_restaurant_2023.api.product.get_product_by_menu').then(r=> console.log(r)).catch(er=> console.log(er))
</script>
<style lang="">
    
</style>