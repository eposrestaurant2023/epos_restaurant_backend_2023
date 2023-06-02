<template>
    <div class="h-full relative bg-cover bg-no-repeat bg-center" v-bind:style="{'background-image': 'url(' + backgroundImage + ')' }">
        <div class="flex h-full flex-col">
            <ComShortcut  v-if="product.setting.pos_menus.length>0"/>
            <ComShortcurMenuFromProductGroup v-else/>
            <div class="pa-2 h-full overflow-y-auto" :class="getCustomerScrollWidth()"  id="wrap_menu">
                <ComPlaceholder :loading="product.posMenuResource.loading" :is-not-empty="product.posMenuResource.data?.length > 0 || product.setting.pos_menus.length==0" class-color="text-white" :is-placeholder="true">
                    <template #default> 
                        <div class="grid gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 2xl:grid-cols-6'" v-if="product.posMenuResource.data?.length > 0">
                            <template v-if="product.setting.pos_menus.length>0">
                            <div v-for="(m, index) in product.getPOSMenu()" :key="index" class="h-36">
                                <ComMenuItem :data="m"/>
                              
                            </div>
                        </template>
                        <template v-else>
                           <ComMenuItemByProductCategory />
                        </template>
                        </div>
                    </template>
                    <template #empty>
                        <div class="h-full flex items-center justify-center">
                            <div class="p-6 text-center bg-white rounded-sm">
                                <div class="text-sm italic mb-2">{{ $t('msg.Please click Refresh to get menu') }}</div>
                                <div>
                                    <v-btn color="primary" prepend-icon="mdi-refresh" @click="onMenuRefresh()">
                                        {{ $t('Refresh') }}
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
import ComShortcurMenuFromProductGroup from './ComShortcurMenuFromProductGroup.vue';
import ComMenuItemByProductCategory from './ComMenuItemByProductCategory.vue';
import ComPlaceholder from '@/components/layout/components/ComPlaceholder.vue';
import ComMenuItem from './ComMenuItem.vue';
import {  inject, defineProps} from '@/plugin';
import { useDisplay } from 'vuetify'
import ComSaleButtonActions from './ComSaleButtonActions.vue';
const { mobile } = useDisplay()
const product = inject("$product")
const frappe = inject("$frappe")
const db = frappe.db();
const props = defineProps({
    backgroundImage: String
});


function getCustomerScrollWidth(){
    const is_window = localStorage.getItem('is_window');
    if(is_window==1){
        return 'scrollbar';
    }
    return '';
}


function onMenuRefresh(){
    if(product.setting.pos_menus.length>0){
        product.loadPOSMenu()
    }else{
        
        product.getProductMenuByProductCategory(db,"All Product Categories")
        product.loadPOSMenu();
    }
    
}
 
</script>
 
<style>

    .scrollbar::-webkit-scrollbar {
        width: 17px;
    }

</style>