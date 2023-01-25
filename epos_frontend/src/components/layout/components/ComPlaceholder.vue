<template>
    <template v-if="isNotEmpty">
        <slot name="default"></slot>
    </template>
    <div v-else>
        <div v-if="isPlaceholder">
            <slot name="empty"></slot>
        </div>
        <div else>
            <div class="p-6 text-center" :class="classColor">
                <div><v-icon :icon="icon" :style="{'font-size':iconSize}"></v-icon></div>
                <div class="text-sm italic">{{ text }}</div>
            </div>
        </div>
    </div>
    
</template>
<script setup>
    const props = defineProps({
        isNotEmpty: Boolean,
        isPlaceholder: Boolean,
        icon: {
            type: String,
            default: 'mdi-package-variant'
        },
        iconSize: {
            type: String,
            default: '60px'
        },
        text: {
            type: String,
            default: 'Empty Data'
        },
        classColor:{
            type: String,
            default: 'text-gray-400'
        }
    })
</script>
<style lang="">
    
</style>