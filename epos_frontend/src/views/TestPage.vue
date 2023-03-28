<template>
  <div>
    <p>xxx</p>
    <v-btn @click="Hello">Test </v-btn>
  </div>
</template>

<script setup>
 
 function Hello(){
  Toaster.postMessage("Test")
 }

</script>

