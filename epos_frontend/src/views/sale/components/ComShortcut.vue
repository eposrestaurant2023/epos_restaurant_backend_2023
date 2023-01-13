<template lang="">
    <div>
        <div class="flex -mx-2" v-if="shortcut.data">
            <div v-for="(m, index) in shortcut.data" :key="m.name" class="px-2">
                <v-btn variant="tonal" @click="onClick(m.name)">
                    {{m.pos_menu_name_en}}
                </v-btn>
            </div>
        </div>
    </div>
</template>
<script setup>
    import { createResource, useStore } from '@/plugin'
    const store = useStore()
    let shortcut = createResource({
        url: 'frappe.client.get_list',
        params:{
        doctype:"POS Menu",
        fields:["name","parent_pos_menu","pos_menu_name_en","pos_menu_name_kh","text_color","background_color"],
        filters: {
                "shortcut_menu": 1,
            }
        },
        auto:true
    })
    function onClick(name) {
        store.dispatch('sale/filterMenu',name)
    }
</script>
