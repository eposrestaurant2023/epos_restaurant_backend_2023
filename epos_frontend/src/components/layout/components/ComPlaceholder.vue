<template>
    <div v-if="loading">
        <div class="p-6 text-center" :class="classColor">
            <div><v-icon icon="mdi-spin mdi-loading" style="font-size:60px"></v-icon></div>
            <div class="text-sm italic">{{ $t('Loading') }}...</div>
        </div>
    </div>
    <template v-else>
        <template v-if="isNotEmpty">
            <slot name="default"></slot>
        </template>
        <template v-else>
            <template v-if="isPlaceholder">
                <slot name="empty"></slot>
            </template>
            <div v-else>
                <div class="p-6 text-center" :class="classColor">
                    <div><v-icon :icon="icon" :style="{ 'font-size': iconSize }"></v-icon></div>
                    <div class="text-sm italic">{{ text }}</div>
                </div>
            </div>
        </template>
    </template>
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
    loading: {
        type: Boolean,
        default: false
    },
    classColor: {
        type: String,
        default: 'text-gray-400'
    }
})
</script>
<style lang="">
    
</style>