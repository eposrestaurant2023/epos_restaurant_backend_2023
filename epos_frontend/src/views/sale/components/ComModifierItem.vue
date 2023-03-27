<template>
    <v-chip 
        class="m-1"
        @click="onClick"
        :size="mobile ? 'large' : 'x-large'" >
        <v-icon start icon="mdi-checkbox-marked-circle-outline" v-if="modifier.selected" color="orange"></v-icon>
        <span>
            {{ modifier.modifier }} <CurrencyFormat :value="modifier.price"></CurrencyFormat>
        </span>
    </v-chip> 
</template>
<script setup>
import { defineProps,defineEmits } from 'vue'
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const props = defineProps({
    modifier:Object
})
const emit = defineEmits(["click"])
function onClick(){
    emit("click");
}
</script> 