<template>
    <v-dialog v-model="openKeyboard" persistent :max-width="type == 'number' ? '400' : '900'">
        <v-card>
            <v-toolbar color="default" :title="title">
                <v-toolbar-items>
                    <v-btn icon @click="onClose('{enter}')">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </v-toolbar-items>
            </v-toolbar>
            <v-card-text>
                <div class="mb-2">
                    <v-text-field type="text" density="compact" variant="solo" single-line hide-details :value="input"
                        @input="onInputChange">
                    </v-text-field>
                </div>
                <ComKeypad v-if="type == 'number'" @onChange="onKey($event)" :input="input" />
                <div v-else>
                    <ComKeyboard @onChange="onKey($event)" @onKeyPress="onKeyPress($event)" :input="input" lang="en" v-if="lang == 'en'"/>
                    <ComKeyboard @onChange="onKey($event)" @onKeyPress="onKeyPress($event)" :input="input" lang="kh" v-if="lang == 'kh'"/>
                </div>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn variant="flat" @click="onClose(false)" color="error">
                    Close
                </v-btn>
                <v-btn variant="flat" @click="onKeyPress('{enter}')" color="primary">
                    OK
                </v-btn>
                <v-spacer></v-spacer>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, closeDialog, computed } from '@/plugin'
import ComKeyboard from './ComKeyboard.vue';
import ComKeypad from './ComKeypad.vue';

const props = defineProps({
    value: String,
    type: {
        type: String,
        default: 'text'
    },
    title: {
        type: String,
        default: 'Keyboard'
    }
})
let openKeyboard = ref(true)
let lang = ref('en')
let input = ref(props.value)

function onKey($event) {
    input.value = $event;
}
function onKeyPress($event) { 
    if ($event == '{enter}') {
        if (props.type == "number") {
            let number = parseFloat(input.value);
            
            if (isNaN(number)){
            
                number = 0
            }
            closeDialog(number)
        }
        else {
            closeDialog(input.value === undefined ? '' : input.value) 
        }
    }
    
    else if($event == '{bksp}'){ 
        if(input.value != undefined && input.value.length > 0)
            input.value = String(input.value).substring(0,String(input.value).length-1);
    }
    else if($event == '{English}' || $event == '{ខ្មែរ}'){
        
        lang.value = $event == '{English}' ? 'en': 'kh' 
       
     
    }
}
function onClose() {
    closeDialog(false)
}
function onInputChange($event) {
    input.value = $event.target.value;
}
</script>