<template>
    <v-app>
        <v-app-bar :elevation="2" height="50">
            <v-app-bar-title class="text-center">{{ appTitle }}</v-app-bar-title> 
        </v-app-bar>  
    </v-app>
</template>
<script setup>
    import {ref,inject} from 'vue';
    import {useRouter} from 'vue-router';
    import ComCurrentUserAvatar from '../views/components/ComCurrentUserAvatar.vue';

    const router = useRouter()
    const drawer = ref( false)
    const isFullscreen = ref( true)
    const appTitle = ref('Invalid QR Menu')
    const sale = inject("$sale")
    function onRoute(page) {
        router.push({name:page})
    }

    function onDrawer() {
        drawer.value = !drawer.value;
    }
    
</script>
<style lang="">
    
</style>