<template>
    <div class="-mx-2" v-if="sale_status">
        <v-chip
            v-for="(s, index) in sale_status" :key="index"
            class="ma-2"
            :color="s.background_color"
            text-color="white"
            >
            {{s.name}}
        </v-chip>
    </div>
</template>
<script setup>
    const   sale_status  = JSON.parse(localStorage.getItem("setting")).sale_status
</script>