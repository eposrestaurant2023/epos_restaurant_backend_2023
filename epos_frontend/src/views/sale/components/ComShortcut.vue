<template lang="">
    <div>
        <div class="flex -mx-2" v-if="shortcut">
            <div v-for="(m, index) in shortcut" :key="m.name_en" class="px-2">
                <v-btn rounded="pill" variant="tonal" :color="m.name_en == active ? 'primary' : ''" @click="onClick(m.name_en)">
                    {{m.name_en}}
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
