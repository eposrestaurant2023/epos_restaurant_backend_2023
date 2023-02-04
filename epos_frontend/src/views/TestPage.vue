 
<template>
   {{ data }}
</template>
<script setup>
import {ref} from "@/plugin"
let data = ref([])
data.value.push({menu_text:"sub-menu",repear_text:"['Item 1','Item 2','Item 3']"})
data.value.push({menu_text:"sub-menu2",repear_text:"['Item 4','Item 5','Item 6']"})


</script>
