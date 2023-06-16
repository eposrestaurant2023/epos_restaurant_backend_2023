<template>
    <div class="mr-2 d-none d-md-block" style="font-size: 14px;">
        {{ moment(time).format('DD-MM-YYYY HH:mm:ss A') }}
    </div>
</template>
<script setup>
import { ref, inject } from '@/plugin'
const moment = inject('$moment')
var time = ref()
setInterval(() => {
    time.value = new Date().toString('hh:mm:ss tt')
}, 1000)


</script>
