<template>
    <div>
        <v-toolbar color="transparent" elevation="0" >
            <v-toolbar-title class="!text-sm">
                <v-icon :size="mobile ?'small' : 'default'">{{ icon }}</v-icon>
                {{ title }}
                <slot name="title"></slot>
            </v-toolbar-title>
            <v-toolbar-items>
               <slot name="centerCotent"></slot>
            </v-toolbar-items>
            <v-toolbar-items>
               <slot name="action"></slot>
            </v-toolbar-items>
        </v-toolbar>
        <v-divider></v-divider>
        <div :class="full ? 'page-full' : 'page-container'" class="mx-auto mt-4 mb-0">
            <slot name="default"></slot>
        </div>
    </div>
</template>
<script setup>
const props = defineProps({
    title: {
        type: String,
        default: 'Page Title'
    },
    icon: {
        type: String,
        default: 'mdi-note-outline'
    },
    full: {
        type: Boolean,
        default: false
    },
})
</script>
<style scoped>
    .page-container {
        max-width: 960px;
        width: 100%;
    }
    .page-full {
        max-width: calc(100% - 32px);
        width: 100%;
    }
</style>