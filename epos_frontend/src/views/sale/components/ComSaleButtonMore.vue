<template> 
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-btn :stacked="!mobile"  size="small" :variant="mobile ? 'tonal':'elevated'" class="m-1 grow" prepend-icon="mdi-menu" v-bind="props">
                More
            </v-btn>
        </template>
        <v-list>
            <ComSaleButtonMoreList/>
        </v-list>
    </v-menu>       
</template>
<script setup> 
    import ComSaleButtonMoreList from "./ComSaleButtonMoreList.vue";
    import {useDisplay} from  'vuetify'
    const {mobile} = useDisplay()
</script>