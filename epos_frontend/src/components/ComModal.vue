<template>
    
    <v-dialog v-model="open" v-bind:style="{'width':'100%','max-width': fullscreen ? 'auto' : width}" :fullscreen="mobileFullscreen ? mobile : fullscreen" :scrollable="scrollable" :persistent="persistent" @update:modelValue="onAction()">
        <v-card>
    
            <ComToolbar @onPrint="onPrint()" :isPrint="isPrint" :isMoreMenu="isShowBarMoreButton" @onClose="onClose()" :disabled="loading">
                <template #title>
                    <slot name="title"></slot>
                </template>
                <template #action>
                    <slot name="bar_custom"></slot>
                </template>
                <template #more_menu> 
                    <slot name="bar_more_button"></slot>
                </template>
            </ComToolbar>
            <v-card-text :class="fill ? '!p-0' : '!p-2'" class="!overflow-x-hidden"> 
                <slot name="content"></slot>
            </v-card-text>
            <v-card-actions v-if="$slots.action || !hideCloseButton || !hideOkButton" class="justify-end flex-wrap" :class="{'!p-0' : fill}">            
                <slot name="action"></slot>
                <template v-if="!customActions">
                    <v-btn variant="flat" @click="onClose()" color="error" :disabled="loading" v-if="!hideCloseButton">
                        Close
                    </v-btn>
                    <v-btn variant="flat" type="button" color="primary" :disabled="loading" v-if="!hideOkButton" @click="onOK()">
                        {{ titleOKButton }}
                    </v-btn>
                </template>
            </v-card-actions>
 
        </v-card>
     </v-dialog>
 
</template>
<script setup>
import { defineEmits, ref, defineProps} from '@/plugin'
import ComToolbar from './ComToolbar.vue'
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const open = ref(true)
const emit = defineEmits(["onClose","onOK"])
const props =defineProps({
    loading: {
        type: Boolean,
        default: false
    },
    hideOkButton:{
        type: Boolean,
        default: false
    },
    hideCloseButton: {
        type: Boolean,
        default: false
    },
    width: {
        type: String,
        default: "800px"
    },
    titleOKButton: {
        type: String,
        default: "Save"
    },
    fullscreen: {
        type: Boolean,
        default: false
    },
    mobileFullscreen: {
        type: Boolean,
        default: false
    },
    height: {
        type: [String, Number],
        default: undefined
    },
    scrollable: {
        type: Boolean,
        default: true
    },
    persistent: {
        type: Boolean,
        default: false
    },
    isShowBarMoreButton:{
        type: Boolean,
        default: false
    },
    isPrint: {
        type: Boolean,
        default: false
    },
    fill: {
        type: Boolean,
        default: false
    },
    customActions: {
        type: Boolean,
        default: false
    }
})
function onClose(value) {
    emit('onClose');
}
function onOK() {
    emit('onOk')
}
function onPrint() {
    emit('onPrint')
}
function onAction($event){
    // close
    if(props.persistent == false && $event == undefined){
        emit('onClose')
    }
 
} 

</script>