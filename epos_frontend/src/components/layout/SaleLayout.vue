<template>
    <v-app>
        <v-app-bar :elevation="2">
            
            <template #prepend>
                <v-app-bar-nav-icon variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
                <v-app-bar-title>
                    POS - T001
                     
            </v-app-bar-title>
            </template>
           
            <template #title>
                <ComProductSearch />
            </template>
            <template #append>
                <v-menu :location="location">
                    <template v-slot:activator="{ props }">
                        <v-avatar image="https://pyxis.nymag.com/v1/imgs/51b/28a/622789406b8850203e2637d657d5a0e0c3-avatar-rerelease.rsquare.w700.jpg"  v-bind="props"></v-avatar>
                    </template>
                    <v-card min-width="300">
                        <v-list>
                            <v-list-item prepend-avatar="https://cdn.vuetifyjs.com/images/john.jpg"
                                title="John Leider" subtitle="Founder of Vuetify">
                            </v-list-item>
                        </v-list>

                        <v-divider></v-divider>

                        <v-list density="compact">
                            <v-list-item  @click="$auth.logout()">
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
            <MainLayoutDrawer/>
            <template v-slot:append>
                <v-btn variant="tonal" prepend-icon="mdi-arrow-left" class="w-full" @click="onDrawer">
                    Close
                </v-btn>
            </template>
        </v-navigation-drawer>
        <v-main>
            <router-view />
        </v-main>
    </v-app>
</template>
<script>
import ComProductSearch from '../../views/sale/components/ComProductSearch.vue'
import MainLayoutDrawer from './MainLayoutDrawer.vue';
import SaleLayoutDrawer from './SaleLayoutDrawer.vue';
import {useStore} from "@/plugin"
 
export default {
    inject: ["$auth"],
    name: "MainLayout",
    components: {
    ComProductSearch,
    SaleLayoutDrawer,
    MainLayoutDrawer
},
    data() {
        return {
            drawer: false
        }
    },
    methods: {
        onDrawer(){
            this.drawer = !this.drawer;
        }
    },
}
</script>
<style lang="">
    
</style>