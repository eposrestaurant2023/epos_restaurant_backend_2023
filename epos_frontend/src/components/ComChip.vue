<template>
    <v-tooltip :text="tooltip" location="top">
        <template v-slot:activator="{props}">
            <v-chip 
                v-bind="props" 
                style="margin: 1px;" 
                :size="screen.chipSize"
                :rounded="rounded" 
                :variant="variant"
                :prepend-icon="prependIcon"
                :append-icon="appendIcon"
                @click="emit('onClick')"
                >
                <slot></slot>
            </v-chip>
        </template>
    </v-tooltip>
</template>
<script setup>
    import { defineProps, inject } from 'vue'
    const screen = inject('$screen')
    const props = defineProps({
        tooltip: String,
        rounded: {
            type: String,
            default: 'pill'
        },
        variant: {
            type: String,
            default: 'tonal'
        },
        prependIcon: String,
        appendIcon: String,
        rounded: {
            type: String,
            default: 'pill'
        }
    })
</script>
<style lang="">
    
</style>