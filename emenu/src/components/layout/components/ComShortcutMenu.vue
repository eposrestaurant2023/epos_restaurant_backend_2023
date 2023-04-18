<template>
    <div class="py-4">
        <div v-style="{
                color: (active == item.name ? gv.setting.template_style.shortcut_active_color : gv.setting.template_style.shortcut_color),
                backgroundColor: (active == item.name ? gv.setting.template_style.shortcut_active_background : gv.setting.template_style.shortcut_background)
                }">xxx</div>
        <div class="overflow-x-auto -m-1" v-if="gv.setting.pos_menu">
            
            <v-chip class="m-1" v-for="(item, index) in gv.setting.pos_menu" :key="index" @click="onSelect(item.name)" v-style="{
                color: (active == item.name ? gv.setting.template_style.shortcut_active_color : gv.setting.template_style.shortcut_color),
                backgroundColor: (active == item.name ? gv.setting.template_style.shortcut_active_background : gv.setting.template_style.shortcut_background)
                }"> 
                {{ active }}
                {{ item.pos_menu_name_en }} - {{ active == item.name ? gv.setting.template_style.shortcut_active_color : gv.setting.template_style.shortcut_color }}
            </v-chip>
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
 