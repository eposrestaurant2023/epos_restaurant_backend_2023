<template>
    
    <v-dialog v-model="open" v-bind:style="{'width':'100%','max-width': fullscreen ? 'auto' : width}" :fullscreen="mobileFullscreen ? mobile : fullscreen" :scrollable="scrollable" :persistent="persistent" @update:modelValue="onAction()">
        <v-card>
            <v-card-text :class="fill ? '!p-0' : '!p-2'" class="!relative !overflow-x-hidden">
                <v-btn class="!fixed top-1 right-2" size="small" icon="mdi-close" @click="onClose()"></v-btn>
                <slot name="content"></slot>
            </v-card-text>
            <v-card-actions v-if="$slots.action || !hideCloseButton || !hideOkButton" :class="actionClass">
                <slot name="action"></slot>
                <v-btn variant="flat" @click="onClose()" color="error" :disabled="loading" v-if="!hideCloseButton">
                    Close
                </v-btn>
                <v-btn variant="flat" type="button" color="primary" :disabled="loading" v-if="!hideOkButton" @click="onOK()">
                    {{ titleOKButton }}
                </v-btn>
            </v-card-actions>
        </v-card>
     </v-dialog>
 
</template>
<script setup>
import { ref} from 'vue'
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
    actionClass: {
        type: String,
        default: "justify-end flex-wrap"
    }
})
function onClose(value) {
    emit('onClose');
}
function onOK() {
    emit('onOk')
}
function onAction($event){
    // close
    if(props.persistent == false && $event == undefined){
        emit('onClose')
    }
 
} 

</script>