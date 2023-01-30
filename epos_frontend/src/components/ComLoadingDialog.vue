<template>
     
    <v-overlay
      :model-value="open"
      class="align-center justify-center"
    >
      <v-progress-circular
        color="primary"
        indeterminate
        size="64"
      ></v-progress-circular>
    </v-overlay>
    </template>
    <script setup>
import { ref } from '@/plugin';
const props = defineProps({
    isLoading:Boolean
})
const open = ref(true)
//open.value=props.isLoading;

</script>