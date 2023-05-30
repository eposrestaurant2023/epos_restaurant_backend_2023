<template>
    <v-menu>
        <template v-slot:activator="{ props }">
            <!-- <div class="cursor-pointer m-1 rounded-md p-2 grow bg-blue-600 hover:bg-blue-700 text-white" v-bind="props">
                Discount
            </div> -->
            <v-btn :stacked="!mobile"  :height="mobile ? '35px' : undefined" :variant="mobile ? 'tonal':'elevated'" size="small" class="m-0-1 grow" :prepend-icon="mobile ? '' : 'mdi-tag-outline'"  v-bind="props">
                {{ $t('Discount') }}
            </v-btn>
        </template>
        <v-list>
            <ComDiscountButtonList/>
        </v-list>
    </v-menu>
</template>
<script setup>
import ComDiscountButtonList from './ComDiscountButtonList.vue';
import {useDisplay} from 'vuetify'
const {mobile} = useDisplay()
</script>