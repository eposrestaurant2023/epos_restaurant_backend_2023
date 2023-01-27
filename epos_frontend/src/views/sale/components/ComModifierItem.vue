<template>
    <!-- <div 
        class="hover:bg-gray-100 cursor-pointer h-28 w-28 rounded-md border-2"
        :class="modifier.selected ? 'border-orange-400 bg-orange-100' : 'border-gray-400'"
        @click="onClick()"
        >
        <div class="flex items-center justify-center p-3 h-full">
            <div class="text-center">
                <div class="font-bold mb-1">
                    {{ modifier.modifier }}
                </div>
                <div>
                    <CurrencyFormat :value="modifier.price"></CurrencyFormat>
                </div>
            </div>
        </div>
    </div> -->
    <v-chip 
        class="m-1"
        @click="onClick" >
        <v-icon start icon="mdi-checkbox-marked-circle-outline" v-if="modifier.selected" color="orange"></v-icon>
        <span>
            {{ modifier.modifier }} <CurrencyFormat :value="modifier.price"></CurrencyFormat>
        </span>
    </v-chip> 
</template>
<script setup>
import { defineProps,defineEmits } from 'vue'
const props = defineProps({
    modifier:Object
})
const emit = defineEmits(["click"])
function onClick(){
    emit("click");
}
</script> 