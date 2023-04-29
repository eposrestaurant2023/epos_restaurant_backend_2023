<template>
    <v-tooltip :text="tooltip" location="top">
        <template v-slot:activator="{props}">
            <v-chip 
                v-bind="props" 
                style="margin: 1px;" 
                :size="size"
                :rounded="rounded" 
                :variant="variant"
                :prepend-icon="prependIcon"
                :append-icon="appendIcon"
                :color="color"
                @click="onClick"
                >
                <slot></slot>
            </v-chip>
        </template>
    </v-tooltip>
</template>
<script setup>
    import { defineProps, inject } from 'vue'
    const emit = defineEmits(["onClick"]) 
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
        size: {
            type: String,
            default: 'small'
        },
        color: String,
        prependIcon: String,
        appendIcon: String,
        rounded: {
            type: String,
            default: 'pill'
        }
    })
    function onClick(){
        emit('onClick')
    }
</script>
<style lang="">
    
</style>