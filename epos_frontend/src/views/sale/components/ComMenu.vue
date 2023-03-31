<template>
    <div class="h-full relative bg-cover bg-no-repeat bg-center" v-bind:style="{'background-image': 'url(' + backgroundImage + ')' }">
        <div class="flex h-full flex-col">
            <ComShortcut/>
            <div class="pa-2 h-full overflow-y-auto" id="wrap_menu">
                <ComPlaceholder :loading="product.posMenuResource.loading" :is-not-empty="product.posMenuResource.data?.length > 0" class-color="text-white" :is-placeholder="true">
                    <template #default>
                        <div class="grid gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 2xl:grid-cols-6'" v-if="product.posMenuResource.data?.length > 0">
                            <div v-for="(m, index) in product.getPOSMenu()" :key="index" class="h-36">
                                <ComMenuItem :data="m"/>
                            </div>
                        </div>
                    </template>
                    <template #empty>
                        <div class="h-full flex items-center justify-center">
                            <div class="p-6 text-center bg-white rounded-sm">
                                <div class="text-sm italic mb-2">Please click refresh button to get menu</div>
                                <div>
                                    <v-btn color="primary" prepend-icon="mdi-refresh" @click="onMenuRefresh()">
                                        Refresh
                                    </v-btn>
                                </div>
                            </div>
                        </div>
                    </template>
                </ComPlaceholder>
            </div>
            <ComSaleButtonActions v-if="!mobile"/>
        </div>
    </div>
</template>
<script setup>
import ComShortcut from './ComShortcut.vue';
import ComPlaceholder from '@/components/layout/components/ComPlaceholder.vue';
import ComMenuItem from './ComMenuItem.vue';
import {  inject, defineProps} from '@/plugin';
import { useDisplay } from 'vuetify'
import ComSaleButtonActions from './ComSaleButtonActions.vue';
const { mobile } = useDisplay()
const product = inject("$product")
const props = defineProps({
    backgroundImage: String
});
 
</script>