<template>
    <div>
      
  
        <div v-if="type=='textarea'">
            <v-textarea
                v-if="keyboard"
                :required="required"
                :type="type"
                :density="density"
                :variant="variant"
                :label="label"
                :single-line = "!label"
                :disabled="disabled"
                :no-resize = "noResize"
                :max-rows = "maxRows"
                :rows = "row"
                hide-details
                :placeholder="placeholder"
                append-inner-icon="mdi-keyboard"
                v-model="value"
                @click:append-inner="onDialog()"
                :prepend-inner-icon="prependInnerIcon"
                @input="updateValue">
            </v-textarea>
            <v-textarea
                v-else
                :required="required"
                :type="type"
                :density="density"
                :variant="variant"
                :label="label"
                :single-line = "!label"
                :disabled="disabled"
                :no-resize = "noResize"
                :max-rows = "maxRows"
                :rows = "row"
                hide-details
                :append-inner-icon="appendInnerIcon"
                v-model="value"
                @click:append-inner="emit('onClickAppendInner')"
                :prepend-inner-icon="prependInnerIcon"
                @click:prepend-inner="emit('onClickPrependInner')"
                @input="updateValue">
            </v-textarea>
        </div>
        <div v-else>
            <v-text-field
                :autofocus="autofocus && !mobile"
                clearable
                :required="required"
                v-if="keyboard && !mobile"
                :type="type"
                :density="density"
                :variant="variant"
                :label="label"
                :single-line = "!label"
                :disabled="disabled"
                hide-details
                :placeholder="placeholder"
                append-inner-icon="mdi-keyboard"
                v-model="value"
                @click:append-inner="onDialog()"
                :prepend-inner-icon="prependInnerIcon"
                @input="updateValue"
                @click:clear="onClear"
                >
            </v-text-field>
            <v-text-field
                v-else
                :autofocus="autofocus && !mobile"
                :required="required"
                :type="type"
                :density="density"
                :variant="variant"
                :label="label"
                :single-line = "!label"
                :disabled="disabled"
                hide-details
                :append-inner-icon="appendInnerIcon"
                v-model="value"
                :placeholder="placeholder"
                @click:append-inner="emit('onClickAppendInner')"
                :prepend-inner-icon="prependInnerIcon"
                @click:prepend-inner="emit('onClickPrependInner')"
                @input="updateValue">
            </v-text-field>
        </div>
    </div>
</template>
<script setup>
import {computed,keyboardDialog} from '@/plugin'
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()

const props = defineProps({
    modelValue: [String, Number],
    type: {
        type: String,
        default: 'text'
    },
    appendInnerIcon: {
        type: String,
        default: ''
    },
    prependInnerIcon: {
        type: String,
        default: ''
    },
    variant: {
        type: String,
        default: 'solo'
    },
    density: {
        type: String,
        default: 'compact'
    },
    placeholder: {
        type: String,
        default: ''
    },
    label: {
        type: String,
        default: ''
    },
    title: {
        type: String
    },
    noResize: {
        type: Boolean,
        default: true
    },
    maxRows: {
        type: Number,
        default: 5
    },
    row: {
        type: Number,
        default: 3
    },
    disabled:{
        type: Boolean,
        default: false
    },
    required: {
        type: Boolean,
        default: false
    },
    keyboard: Boolean,
    autofocus:Boolean
})

let value = computed({
    get(){
        return props.modelValue
    },
    set(newValue){
        return newValue
    }
})
// let data = ref(props.modelValue)
const emit = defineEmits(['update:modelValue'])

const updateValue = (event) => {
    // props.modelValue = event.target.value;
    emit('update:modelValue', event.target.value)
}

async function onDialog() {
 
    const keys = await keyboardDialog({title: props.title,type: props.type, value:props.modelValue});
 
    if(typeof keys == 'boolean' && keys == false){
        return
    }
    else {
         
        emit('onInput',keys)
        emit('update:modelValue', keys)
    }
        
}
function onClear(){ 
    emit('onInput',"")
    emit('update:modelValue', "")
}
</script>
 