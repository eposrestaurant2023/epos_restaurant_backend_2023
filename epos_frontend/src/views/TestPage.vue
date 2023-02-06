<template>
    <div  v-ripple class="pa-8 bg-yellow-500 ma-4 rounded-tl-lg rounded-br-lg">

        <v-select v-model="color"  label="Select Color" :items="['red', 'green', 'blue', 'yellow', 'black', 'gray']"></v-select>
        <v-select v-model="colorWeight" label="Select Weight" :items="[100, 200, 300, 400, 500, 600, 700, 800]"></v-select>

    <v-btn @click="changeX">Change Color</v-btn>
    </div>
    <div  v-ripple :class="getClass()">
        Hello 
    </div>
</template>

<script setup>
    import {ref} from "@/plugin"
    const x= ref("Hello");
    const color= ref("green");
    const colorWeight= ref(400);


    function changeX(){
        
        x.value = "Hello World";

    }

    function getClass(){
        return "pa-8 bg-" + color.value + "-" + colorWeight.value.toString() + " ma-4 rounded-tl-lg rounded-br-lg";
    }

</script>