<template>
    <v-app>
        <v-app-bar :elevation="2" color="error">
            <v-app-bar-title>{{ appTitle }}</v-app-bar-title>
            <template #prepend>
                <v-app-bar-nav-icon variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
            </template>
            <template #append>
                <ComTimeUpdate />
                <v-menu :location="location">
                    <template v-slot:activator="{ props }">
                        <v-avatar :image="currentUser.photo" v-bind="props" v-if="currentUser.photo"
                            class="cursor-pointer"></v-avatar>
                        <avatar v-else :name="currentUser.full_name" v-bind="props" class="cursor-pointer" size="40">
                        </avatar>
                    </template>
                    <v-card min-width="300">
                        <ComCurrentUserAvatar />

                        <v-divider></v-divider>

                        <v-list density="compact">
                            <v-list-item @click="onReload()">
                                <template v-slot:prepend class="w-12">
                                    <v-icon icon="mdi-reload"></v-icon>
                                </template>
                                <v-list-item-title>Reload</v-list-item-title>
                            </v-list-item>
                            <v-divider></v-divider>
                            <v-list-item @click="onLogout">
                                <template v-slot:prepend class="w-12">
                                    <v-icon icon="mdi-logout"></v-icon>
                                </template>
                                <v-list-item-title>Logout</v-list-item-title>
                            </v-list-item>
                        </v-list>
                    </v-card>

                </v-menu>
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
import ComProductSearch from '../../views/sale/components/ComProductSearch.vue'
import MainLayoutDrawer from './MainLayoutDrawer.vue';
import ComCurrentUserAvatar from './components/ComCurrentUserAvatar.vue';
import ComToolbar from '../ComToolbar.vue';
import ComTimeUpdate from './components/ComTimeUpdate.vue';
export default {
    inject: ["$auth"],
    name: "MainLayout",
    computed: {
        appTitle() {
            return JSON.parse(localStorage.getItem('setting')).app_name
        },
        currentUser() {
            return JSON.parse(localStorage.getItem('current_user'))
        }
    },
    data() {
        return {
            drawer: false
        }
    },
    components: {
        ComProductSearch,
        MainLayoutDrawer,
        ComCurrentUserAvatar,
        ComToolbar,
        ComTimeUpdate
    },
    methods: {
        onDrawer() {
            this.drawer = !this.drawer;
        },
        onReload() {
            location.reload()
        },
        onLogout(){
            this.$auth.logout().then((r)=>{
                this.$router.push({name: 'Login'})
            })
        }
    },
}
</script>
<style lang="">
    
</style>