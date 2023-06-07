<template>
    <div class="bg-white" :class="mobile ? 'px-2' : 'p-2'" id="shortcut_menu" v-if="shortcut?.length > 0"> 
        <div :class="mobile ? 'flex flex-nowrap overflow-x-auto pb-1 wrap-sm' : 'flex-wrap flex -my-1 justify-center'" v-if="shortcut">
            <v-btn 
                class="flex-shrink-0 m-1"
                rounded="pill"
                variant="tonal"
                size="small"
                v-bind:style="{'background-color':'red'}"
                @click="onShortCutMenuClick('All Product Categories')">
                <span v-bind:style="{'color':'#fff'}">{{ $t('All Product Categories') }}</span>
            </v-btn> 
            <v-btn 
                class="flex-shrink-0 m-1"
                v-for="(m, index) in shortcut" :key="index"
                rounded="pill"
                variant="tonal"
                size="small"
                v-bind:style="{'background-color':m.background_color}"
                @click="onShortCutMenuClick(m.name)">
                <span v-bind:style="{'color':m.text_color}">{{m.name}}</span>
            </v-btn> 
 
        </div>
    </div>
</template>
<script setup>
    import {  inject,ref } from '@/plugin'
    import {useDisplay}  from 'vuetify'
    const product = inject("$product")

    const frappe = inject("$frappe")
    const db = frappe.db();


    const {mobile} = useDisplay()
 

    const shortcut = ref([])
 
    db.getDocList("Product Category",{
        fields:["name","background_color","background_color"],
        filters:[
        ["show_in_pos_shortcut_menu","=","1"],
        ["allow_sale","=","1"]
    ]
    })
    .then((docs)=>{
        shortcut.value = docs
    })
    .catch((error)=>{
        console.log(error)
    })
  
    function onShortCutMenuClick(name) {
       product.getProductMenuByProductCategory(db,name)
        
    }
</script>
<style scoped>
.wrap-sm {
    width: calc( 100vw - 22px);
}
</style>