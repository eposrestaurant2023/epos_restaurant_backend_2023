<template>
    <div 
        class="hover:bg-gray-100 cursor-pointer h-28 w-28 rounded-md border-2"
        :class="portion.selected ? 'border-orange-400 bg-orange-100' : 'border-gray-400'"
        @click="onClick()"
        >
        <div class="flex items-center justify-center p-3 h-full">
            <div class="text-center">
                <div class="font-bold mb-1">
                    {{ portion.portion }}
                </div>
                <div>
                    <CurrencyFormat :value="portion.price"></CurrencyFormat>
                </div>
            </div>
        </div>
    </div>
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