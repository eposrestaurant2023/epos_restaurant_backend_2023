<template>
    <div class="p-4 bg-red-400 rounded-sm">
        <p>I am son</p>
        <h1 class="p-4">{{ awnser }}</h1>
    </div>
</template>
<script>
import { ref } from 'vue';
const awnser = ref(200)
export default {
  setup() {
    const childMethod = (data) => {
      awnser.value++
    };

    return {
      childMethod,
      awnser
    };
  },
};
</script>