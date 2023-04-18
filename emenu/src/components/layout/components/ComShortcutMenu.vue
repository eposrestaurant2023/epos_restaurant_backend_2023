<template>
    <div class="py-4">
        <div class="flex flex-nowrap overflow-x-auto pb-1 wrap-sm" v-if="gv.setting.pos_menu">
            <v-btn 
                class="flex-shrink-0 m-1"
                v-for="(item, index) in gv.setting.pos_menu" :key="index" 
                @click="onSelect(item.name)"
                rounded="pill"
                variant="tonal"
                size="small"
                v-bind:style="{'background-color': (active == item.name ? gv.setting.template_style.shortcut_active_background : gv.setting.template_style.shortcut_background)}"
                >
                    <span v-bind:style="{'color': (active == item.name ? gv.setting.template_style.shortcut_active_color : gv.setting.template_style.shortcut_color)}">{{ item.pos_menu_name_en }}</span>
            </v-btn>
        </div>
    </div>
</template>
<script setup>
    import {inject, ref, computed} from 'vue'
    const gv = inject('$gv')
    let active = ref(gv.setting.pos_menu && gv.setting.pos_menu.length > 0 ? gv.setting.pos_menu[0].name : '')
    function onSelect(name){
        active.value = name
        alert(active.value)
    }
</script>
 