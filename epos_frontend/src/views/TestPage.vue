<template>
  <div class="p-4 bg-yellow-200">
    <button @click="callChildMethod">Call Child</button>

    <ChildComponent ref="childComponentRef" />
  </div>
</template>

<script setup>
import { ref } from 'vue';
import ChildComponent from './com_test/ComChild.vue';
const childComponentRef = ref(null);
const callChildMethod = () => {
  childComponentRef.value.childMethod();
};
</script>