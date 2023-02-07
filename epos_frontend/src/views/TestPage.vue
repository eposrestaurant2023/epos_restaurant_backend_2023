<template>
    {{ platform }}
    <v-btn @click="changeX">Change Color</v-btn>
    
</template>

<script setup>
    import {ref} from "@/plugin"
    import { useDisplay } from 'vuetify'
 
const { mobile, name, platform } = useDisplay()
    function changeX(){
        alert("xxxxxxxxxxx");
        window.parent.postMessage('Hello from JS');
    }

  
</script>

