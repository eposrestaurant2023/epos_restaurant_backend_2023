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
            <div>
        <ComCurrentUserAvatar/>
        <v-divider></v-divider>

        <v-list :lines="false" density="compact" nav>
            <v-list-item active-color="primary" @click="onRoute('Home')">
                <template v-slot:prepend>
                <v-icon>mdi-home</v-icon>
                </template>
                <v-list-item-title>Home</v-list-item-title>
            </v-list-item>
            </v-list>
        </div>
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
    import {useRouter} from 'vue-router'
    import ComCurrentUserAvatar from './components/ComCurrentUserAvatar.vue'
    const router = useRouter()
    const drawer = ref( false)
    const isFullscreen = ref( true)
    const appTitle = ref('ePOS')
    function onRoute(page) {
        router.push({name:page})
    }
    function onDrawer() {
        drawer.value = !drawer.value;
    }
</script>
<style lang="">
    
</style>