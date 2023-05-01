<template>
    <div>
        
    </div>
</template>
<script setup>
const props = defineProps({
    comboMenu: []
})
const data = JSON.parse(JSON.stringify(props.comboMenu))
const r = arr.map(obj => Object.values(obj)).flat();
alert(JSON.stringify(r))
</script>
 