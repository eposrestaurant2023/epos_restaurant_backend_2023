<template>
  <div>
    <p>xxx</p>
    <v-btn @click="Hello">Hello </v-btn>
  </div>
</template>

<script setup>
import { ref, onMounted,inject } from 'vue'
import { createToaster } from '@meforma/vue-toaster';

const toaster = createToaster({position:'top'});
 
  const socket = inject("$socket")

 
socket.on("UpdateData",(arg)=>{
    toaster.warning(arg)
  })

  


  function Hello(){
console.log(socket);
    socket.emit('UpdateTable',"Hello Ho")

  }

</script>

