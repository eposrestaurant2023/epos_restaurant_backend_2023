<template>
    <v-dialog v-model="openKeyboard" persistent :max-width="params.type == 'number' ? '400' : '900'">
        <v-card>
            <v-toolbar color="default" :title="params.title ? params.title : 'Keyboard'">
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
                <ComKeypad v-if="params.type == 'number'" @onChange="onKey($event)" :input="input" />
                <div v-else>
                    <ComKeyboard @onChange="onKey($event)" @onKeyPress="onKeyPress($event)" :input="input" lang="en" v-if="lang == 'en'"/>
                    <ComKeyboard @onChange="onKey($event)" @onKeyPress="onKeyPress($event)" :input="input" lang="kh" v-if="lang == 'kh'"/>
                </div>
                <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" variant="flat" @click="onClose(false)" color="error" size="large">
                            {{$t('Close')}}
                        </v-btn>
                        <v-btn variant="flat" @click="onKeyPress('{enter}')" color="primary" size="large">
                            {{$t('Ok')}}
                        </v-btn>
                    </div>
                </div>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, defineEmits } from '@/plugin'
import ComKeyboard from './ComKeyboard.vue';
import ComKeypad from './ComKeypad.vue';
const emit = defineEmits(['resolve'])
const props = defineProps({
    params:Object
})
let openKeyboard = ref(true)
let lang = ref('en')
let input = ref(props.params.value)

function onKey($event) {
    input.value = $event;
}
function onKeyPress($event) { 
    if ($event == '{enter}') {
        if (props.params.type == "number") {
            let number = parseFloat(input.value);
            
            if (isNaN(number)){
            
                number = 0
            }
            onOK(number)
        }
        else {
            onOK(input.value === undefined ? '' : input.value) 
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
    emit('resolve',false)
}
function onOK(value){
    emit('resolve', value)
}
function onInputChange($event) {
    input.value = $event.target.value;
}
</script>