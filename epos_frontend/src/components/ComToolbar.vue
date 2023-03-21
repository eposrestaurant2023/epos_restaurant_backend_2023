<template>
    <v-toolbar :color="color" height="60">
        <v-toolbar-title :class="mobile ? '!text-sm' : ''"> 
            <slot name="title"></slot> 
        </v-toolbar-title>
        <v-toolbar-items>
            <v-btn icon @click="$emit('onPrint')" v-if="isPrint" :disabled="disabled">
                <v-icon>mdi-printer</v-icon>
            </v-btn> 
            <slot name="action"></slot>
            <v-menu v-if="isMoreMenu">
                <template v-slot:activator="{ props }">
                    <v-btn 
                    v-bind="props"
                    icon
                    >
                    <v-icon color="white">mdi-dots-vertical</v-icon>
                    </v-btn>
                </template> 
                <slot name="more_menu"></slot>
            </v-menu>
            <v-btn icon @click="$emit('onClose')" v-if="isClose" :disabled="disabled">
                <v-icon>mdi-close</v-icon>
            </v-btn>
        </v-toolbar-items>
    </v-toolbar>
</template>
<script setup>
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const props = defineProps({
    color: {
        type: String,
        default: "red"
    },
    disabled: {
        type: Boolean,
        default: false
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