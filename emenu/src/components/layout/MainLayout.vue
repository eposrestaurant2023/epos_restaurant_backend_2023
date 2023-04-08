<template>
    <v-app>
        <v-app-bar :elevation="2" color="error">
            <v-app-bar-title>{{ appTitle }}</v-app-bar-title>
            <template #prepend>
                <v-app-bar-nav-icon variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
            </template>
            <template #append>
                <v-btn icon="mdi-fullscreen"></v-btn>
                <v-btn icon="mdi-fullscreen-exit"></v-btn>
            </template>
        </v-app-bar>
        <v-navigation-drawer v-model="drawer" temporary>
            <MainLayoutDrawer />
            <template v-slot:append>
                <v-btn variant="tonal" prepend-icon="mdi-arrow-left" class="w-full" @click="onDrawer">
                    Close
                </v-btn>
            </template>
        </v-navigation-drawer>
        <v-main class="overflow-auto h-screen">
            <router-view />
        </v-main>
    </v-app>
</template>
<script>
import MainLayoutDrawer from './MainLayoutDrawer.vue';
export default {
    name: "MainLayout",
    data() {
        return {
            drawer: false,
            isFullscreen: true
        }
    }
}
</script>
<style lang="">
    
</style>