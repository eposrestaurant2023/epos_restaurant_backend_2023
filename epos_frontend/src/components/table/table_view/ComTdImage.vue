<template>
    <td class="text-center">
        <v-avatar v-if="photo">
            <v-img :src="photo"></v-img>
        </v-avatar>
        <avatar v-else :name="title" size="40"></avatar>
    </td>
</template>
<script setup>
    import {defineProps} from 'vue'
    const props = defineProps({
        photo: String,
        title: String
    })
</script>
<style lang="">
    
</style>