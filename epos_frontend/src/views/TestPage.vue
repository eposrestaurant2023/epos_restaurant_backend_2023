<template>
    
    <v-btn @click="changeX">Change Color</v-btn>
    
</template>

<script setup>
    import {ref} from "@/plugin"
    function changeX(){
        alert("xxxxxxxxxxx");
        window.parent.postMessage('Hello from JS');
    }

  
</script>

