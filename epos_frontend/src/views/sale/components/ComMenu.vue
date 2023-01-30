<template>
      
    <div class="h-full">
        <div class="grid h-full" style="grid-template-rows: max-content;">
            <div>
                <ComShortcut/>
            </div>
            <div class="pa-2 h-full bg-cover bg-no-repeat bg-center" v-bind:style="{'background-image': 'url(' + backgroundImage + ')' }">
                <ComPlaceholder :is-not-empty="product.posMenu && product.posMenu.length > 0" class-color="text-white">
                    <template #default>
                        <div class="flex flex-wrap -ml-1 -mr-1" v-if="product.posMenu && product.posMenu.length > 0">
                            <div v-for="(m, index) in product.getPOSMenu()" :key="index" class="p-1">
                                <ComMenuItem :data="m"/>
                            </div>
                        </div>
                    </template>
                </ComPlaceholder>
            </div>
        </div>
    </div>
</template>
<script setup>
import ComShortcut from './ComShortcut.vue';
import ComPlaceholder from '@/components/layout/components/ComPlaceholder.vue';
import ComMenuItem from './ComMenuItem.vue';
import {  inject, defineProps } from '@/plugin';
const screen = inject('$screen')
const product = inject("$product")

const props = defineProps({
    backgroundImage: String
})

</script>