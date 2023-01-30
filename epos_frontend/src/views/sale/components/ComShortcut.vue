<template>
    <div class="py-2 bg-white" id="shortcut_menu">
        <div class="flex flex-wrap -mx-2 justify-center" v-if="shortcut">
            <div v-for="(m, index) in shortcut" :key="m.name_en" class="px-2">
                <!-- :size="$screen.width > 1024 ? 'small' : 'x-small'" -->
                <v-btn rounded="pill" variant="tonal" size="small" :color="m.background_color"  @click="onClick(m.name_en)">
                    <span v-bind:style="{color:m.text_color}">{{m.name_en}}</span>
                </v-btn>
            </div>
        </div>
    </div>
</template>
<script setup>
    import { computed, ref,watch, inject } from '@/plugin'
    const product = inject("$product")
    const active = ref('')
 
    const shortcut = computed(()=>{
        return product.posMenu ? product.posMenu.filter(r=>r.shortcut_menu == 1) : null
    })
  
    function onClick(name) {
        active.value = name;
        product.parentMenu = name;
    
    }
</script>
