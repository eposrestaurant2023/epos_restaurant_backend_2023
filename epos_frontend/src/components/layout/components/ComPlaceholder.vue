<template>
    <template v-if="isNotEmpty">
        <slot name="default"></slot>
    </template>
    <div v-else>
        <div v-if="isPlaceholder">
            <slot name="empty"></slot>
        </div>
        <div else>
            <div class="p-8 text-center opacity-70 text-gray-400">
                <div><v-icon :icon="icon"></v-icon></div>
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
        text: {
            type: String,
            default: 'Empty Data'
        }
    })
</script>
<style lang="">
    
</style>