<template>
    <v-chip 
        class="m-1"
        @click="onClick" >
        <v-icon start icon="mdi-checkbox-marked-circle-outline" v-if="portion.selected" color="orange"></v-icon>
        <span>
            {{ portion.portion }} <CurrencyFormat :value="portion.price"></CurrencyFormat>
        </span>
    </v-chip> 
</template>
<script setup>
import { defineProps,defineEmits } from 'vue'
const props = defineProps({
    portion:Object
})
const emit = defineEmits(["click"])
function onClick(){
    emit("click");
}
</script> 