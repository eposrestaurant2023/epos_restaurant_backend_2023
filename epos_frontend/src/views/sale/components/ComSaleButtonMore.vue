<template> 
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-btn  v-if="!mobile" stacked  size="small" :variant="mobile ? 'tonal':'elevated'" class="m-0-1 grow" prepend-icon="mdi-dots-horizontal" v-bind="props">
                <span>{{ $t('More') }}</span>
            </v-btn>
            <v-btn v-else  size="small" variant="tonal"  :height="'35px'" class="m-0-1 grow" v-bind="props">
                <v-icon>mdi-dots-horizontal</v-icon>
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