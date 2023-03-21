


<!-- <template>
  <button @click="test">Test</button>
</template>
<script setup>
  function test(){
    Toaster.postMessage('Hello World being called from Javascript code sadfasf');
  }
</script> -->


<template>
  <div>
    <h1>Websocket Example</h1>
    <p>{{ message }}</p>
    <button @click="sendMessage">Send Message</button>
  </div>
</template>

<script setup> 
import {ref, inject } from '@/plugin'
const socket = inject("$socket")
const message = ref('No message yet.')


socket.on("connect", () => {
alert("connect")
});

    socket.on('message', data => {
      alert(data)
      message.value = data
    })

    const sendMessage = () => {
      alert(123)
      console.log(socket)
      socket.emit('message', 'Hello World!')
    }

 
</script>