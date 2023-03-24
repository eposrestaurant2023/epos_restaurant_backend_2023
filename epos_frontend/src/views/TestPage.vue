

<template>
  <button @click="test()">Test</button>
</template>
<script setup>

import io from 'socket.io-client';
const socket = io('http://192.168.10.127:9004/socket.io/?EIO=4&transport=polling');

socket.on('connect', () => {
  console.log('Connected to server!');
  socket.on("hello", (arg) => {
    alert(123);
    console.log(arg); // world
  });
});




socket.on('disconnect', () => {
  console.log('Disconnected from server!');
});

function test(){
  alert("Test click")
  socket.emit("hello", "world");
  console.log(socket);

}
</script> 
 