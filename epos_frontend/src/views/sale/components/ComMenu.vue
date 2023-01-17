<template lang="">
    <div>
        <div>
            <ComShortcut/>
        </div>
        <div class="mt-4">
            <ComPlaceholder :is-not-empty="posMenu && posMenu.length > 0">
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
</template>
<script setup>
import ComShortcut from './ComShortcut.vue';
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';
import ComMenuItem from './ComMenuItem.vue';
import { useStore, computed } from '@/plugin';
const store = useStore()

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
        return store.state.sale.posMenu.filter(r => r.parent == store.state.sale.parentMenu)
    }
})
</script>