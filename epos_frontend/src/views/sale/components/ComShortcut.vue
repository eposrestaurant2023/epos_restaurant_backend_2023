<template>
    <div class="bg-white" :class="mobile ? 'px-2' : 'p-2'" id="shortcut_menu" v-if="shortcut?.length > 0"> 
        <div :class="mobile ? 'flex flex-nowrap overflow-x-auto pb-1 wrap-sm' : 'flex-wrap flex -my-1 justify-center'" v-if="shortcut">
            <v-btn 
                class="flex-shrink-0 m-1"
                v-for="(m, index) in shortcut" :key="index"
                rounded="pill"
                variant="tonal"
                size="small"
                v-bind:style="{'background-color':m.background_color}"
                @click="onClick(m.name_en)">
                <span v-bind:style="{color:m.text_color}">{{m.name_en}}</span>
            </v-btn> 
        </div>
    </div>
</template>
<script setup>
    import { computed, inject } from '@/plugin'
    import {useDisplay}  from 'vuetify'
    const {mobile} = useDisplay()
    const product = inject("$product")
    const shortcut = computed(()=>{
        return product.posMenuResource.data?.filter(r=>r.shortcut_menu == 1) 
    })
  
    function onClick(name) {
        product.searchProductKeyword="";
        product.parentMenu = name;
    
    }
</script>
<style scoped>
.wrap-sm {
    width: calc( 100vw - 22px);
}
</style>