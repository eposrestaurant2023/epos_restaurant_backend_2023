<template>
    <div>
        
        <div v-if="type=='textarea'">
            <v-textarea
                v-if="keyboard"
                :type="type"
                density="compact"
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
                :value="data"
                @click:append-inner="onDialog()"
                :prepend-inner-icon="prependInnerIcon"
                @input="updateValue">
            </v-textarea>
            <v-textarea
                v-else
                :type="type"
                density="compact"
                :variant="variant"
                :label="label"
                :single-line = "!label"
                :disabled="disabled"
                :no-resize = "noResize"
                :max-rows = "maxRows"
                :rows = "row"
                hide-details
                :append-inner-icon="appendInnerIcon"
                :value="data"
                @click:append-inner="emit('onClickAppendInner')"
                :prepend-inner-icon="prependInnerIcon"
                @click:prepend-inner="emit('onClickPrependInner')"
                @input="updateValue">
            </v-textarea>
        </div>
        <div v-else>
            <v-text-field
                v-if="keyboard"
                :type="type"
                density="compact"
                :variant="variant"
                :label="label"
                :single-line = "!label"
                :disabled="disabled"
                hide-details
                :placeholder="placeholder"
                append-inner-icon="mdi-keyboard"
                :value="data"
                @click:append-inner="onDialog()"
                :prepend-inner-icon="prependInnerIcon"
                @input="updateValue">
            </v-text-field>
            <v-text-field
                v-else
                :type="type"
                density="compact"
                :variant="variant"
                :label="label"
                :single-line = "!label"
                :disabled="disabled"
                hide-details
                :append-inner-icon="appendInnerIcon"
                :value="data"
                @click:append-inner="emit('onClickAppendInner')"
                :prepend-inner-icon="prependInnerIcon"
                @click:prepend-inner="emit('onClickPrependInner')"
                @input="updateValue">
            </v-text-field>
        </div>
    </div>
</template>
<script setup>
import {ref,openDialog} from '@/plugin'
import ComModalKeyboard from './ComModalKeyboard.vue';

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
    keyboard: Boolean
})

let data = ref(props.modelValue)
const emit = defineEmits(['update:modelValue'])

const updateValue = (event) => {
    data.value = event.target.value;
    emit('update:modelValue', data.value)
}

async function onDialog() {
 
    const keys = await openDialog(ComModalKeyboard,{title: props.title,type: props.type, value:data.value});
 
    if(typeof keys == 'boolean' && keys == false){
        
        return
    }
    else {
        data.value = keys;
        emit('onInput',data.value)
        emit('update:modelValue', data.value)
    }
        
}
</script>
 