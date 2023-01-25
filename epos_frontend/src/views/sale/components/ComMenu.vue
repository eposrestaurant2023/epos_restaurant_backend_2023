<template>
      
    <div>
        <div>
            <ComShortcut/>
        </div>
        <div>
            <div class="pa-2">
                <ComPlaceholder :is-not-empty="posMenu && posMenu.length > 0" class-color="text-white">
                    <template #default>
                        <div class="flex -ml-1 -mr-1" v-if="posMenu && posMenu.length > 0">
                          
                            <div v-for="(m, index) in posMenu" :key="index" class="p-1">
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
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';
import ComMenuItem from './ComMenuItem.vue';
import { useStore, computed } from '@/plugin';
const store = useStore()
const setting = computed(()=>{
    return JSON.parse(localStorage.getItem('setting'));
})
const posMenu = computed(() => {
    if (!store.state.sale.keyword) {
        if (store.state.sale.parentMenu) {

            return store.state.sale.posMenu.filter(r => r.parent == store.state.sale.parentMenu)
        }
        else {
            const setting = JSON.parse(localStorage.getItem('setting'))
            let defaultMenu = setting.default_pos_menu;

            if (localStorage.getItem('default_menu')) {
                defaultMenu = localStorage.getItem('default_menu')
            }

            return store.state.sale.posMenu ? store.state.sale.posMenu.filter(r => r.parent == defaultMenu) : null;
        }
    } else {
        return store.state.sale.posMenu.filter((r) => {
            return String( r.name_en + ' ' + r.name_kh + ' ' + r.name ).toLocaleLowerCase().includes(store.state.sale.keyword.toLocaleLowerCase())  && r.type =="product"
        })

 
    }
})
</script>