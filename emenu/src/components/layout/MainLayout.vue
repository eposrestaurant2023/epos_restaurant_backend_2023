<template>
    <v-app>
        <v-app-bar :elevation="2" height="50">
            <v-app-bar-title class="text-center">{{ appTitle }}</v-app-bar-title>
            <template #prepend>
                <v-app-bar-nav-icon variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
            </template>
            <template #append>
                <v-btn class="text-none" stacked>
                    <v-badge content="9+" color="error">
                        <v-icon icon="mdi-cart-outline"></v-icon>
                    </v-badge>
                </v-btn>
            </template>
        </v-app-bar>
        <v-navigation-drawer v-model="drawer" temporary>
            <!-- <MainLayoutDrawer /> -->
            <MainLayoutDrawer/>
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
<script setup>
    import {ref} from 'vue'
    // import MainLayoutDrawer from './MainLayoutDrawer.vue';
    import MainLayoutDrawer from '@/components/Layout/MainLayoutDrawer.vue'
    const drawer = ref( false)
    const isFullscreen = ref( true)
    const appTitle = ref('ePOS')

</script>
<style lang="">
    
</style>