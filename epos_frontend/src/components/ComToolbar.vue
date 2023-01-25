<template>
    <v-toolbar :color="color">
        <v-toolbar-title>
            <slot name="title"></slot>
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <v-toolbar-items>
            <v-btn icon @click="$emit('onPrint')" v-if="isPrint">
                <v-icon>mdi-printer</v-icon>
            </v-btn>
            <slot name="action"></slot>
            <v-menu v-if="isMoreMenu">
                <template v-slot:activator="{ props }">
                    <v-btn
                    color="primary"
                    v-bind="props"
                    icon
                    >
                    <v-icon color="white">mdi-dots-vertical</v-icon>
                    </v-btn>
                </template>
                <slot name="more_menu"></slot>
            </v-menu>
            <v-btn icon @click="$emit('onClose')" v-if="isClose">
                <v-icon>mdi-close</v-icon>
            </v-btn>
        </v-toolbar-items>
    </v-toolbar>
</template>
<script setup>
const props = defineProps({
    color: {
        type: String,
        default: "red"
    },
    isMoreMenu: {
        type: Boolean,
        default: false
    },
    isClose: {
        type: Boolean,
        default: true
    },
    isPrint:{
        type: Boolean,
        default: false
    }
})
</script>
<style lang="">
    
</style>