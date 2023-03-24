

<template>
  <button @click="test">Test</button>
</template>
<script setup>
  function test(){
    Toaster.postMessage('Hello World being called from Javascript code sadfasf');
  }
</script> 
 