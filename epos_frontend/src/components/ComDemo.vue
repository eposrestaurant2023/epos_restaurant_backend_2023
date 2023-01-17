<template>
    <div class="mb-4">
        <h1>{{ msg }}</h1>
        <div>this is com demo</div>
        <buttton @click="onClick(7)">callback</buttton>
        <slot name="default"></slot> 
        
        <input type="text" style="background-color: red;"  @input="$emit('update:modelValue', $event.target.value)"/>
    </div>
</template>
<script setup>
    import { defineProps,defineEmits } from '@/plugin'
    const emit = defineEmits(['onCallback'])
    const props = defineProps({
        msg: {
            type:String,
            default: 'def'
        }
    })
    function onClick(value){
        let total = 1 + value
        emit('onCallback',total,'vue')
    }
</script>
<style scoped>
    div {
        background-color: aquamarine;
        font-size: 20px;
            }
</style>