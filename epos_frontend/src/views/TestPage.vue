<template>
  <div class="pa-8">
    <v-text-field v-model="n1" /> + <v-text-field v-model="n2"/> <v-btn @click="AddNumber">=</v-btn> <v-text-field v-model="result"/> 
  </div>

</template>
<script setup>
import { ref } from '@/plugin';
const n1 = ref(0)
const n2 = ref(0)
const result = ref(0)

function AddNumber(){
  result.value = parseFloat(n1.value) + parseFloat( n2.value);
}




</script>