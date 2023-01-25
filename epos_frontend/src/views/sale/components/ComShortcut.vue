<template>
    <div class="py-2 bg-white">
        <div class="flex -mx-2 justify-center" v-if="shortcut">
            <div v-for="(m, index) in shortcut" :key="m.name_en" class="px-2">
                <v-btn rounded="pill" variant="tonal" :color="m.background_color" size="small" @click="onClick(m.name_en)">
                    <span v-bind:style="{color:m.text_color}">{{m.name_en}}</span>
                </v-btn>
            </div>
        </div>
    </div>
</template>
<script setup>
    import { computed, useStore, ref } from '@/plugin'
    const store = useStore()
    const active = ref('')
    const shortcut = computed(()=>{
        return store.state.sale.posMenu ? store.state.sale.posMenu.filter(r=>r.shortcut_menu == 1) : null
    })
    function onClick(name) {
        active.value = name;
        store.state.sale.parentMenu = name;

    }
</script>
