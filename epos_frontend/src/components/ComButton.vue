<template>
    <div class="shadow-md text-center p-4 rounded-md cursor-pointer flex items-center justify-center h-full" :style="{backgroundColor:backgroundColor}">
        <div>
            <v-icon :color="iconColor" class="m-2" v-if="icon" :icon="icon" size="x-large" :class="iconClass"></v-icon>
            <div :style="{color:textColor}">{{ title }}</div>
        </div>
    </div>
</template>
<script setup>
    const props = defineProps({
        iconColor: {
            type:String,
            default: '#000'
        }, 
        icon: String, 
        title: String,
        backgroundColor: {
            type: String,
            default: ''
        },
        textColor: {
            type: String,
            default: ''
        },
        iconClass: String
    })
</script>
<style lang="">
    
</style>