<template>
    <v-app>
        <v-app-bar :elevation="2" color="error">
            
            <template #prepend>
                <v-app-bar-nav-icon variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
                <v-app-bar-title>
                    POS - 
                    <span v-if="$sale.sale.tbl_number">{{ $sale.sale.tbl_number }}</span>
                    <span v-if="$sale.sale.name">  - $sale.sale.name</span>
                    <span v-else>  - New</span>
                     
            </v-app-bar-title>
            </template>
           
            <template #title>
                <ComProductSearch />
            </template>
            <template #append>
              
                <ComTimeUpdate/>
               
                <v-btn class="text-none" stacked>
                    <v-badge content="9" color="success">
                        <v-icon>mdi-cart</v-icon>
                    </v-badge>
                </v-btn>
                
                
                <v-menu :location="location">
                    <template v-slot:activator="{ props }">
                        <v-avatar :image="currentUser.photo"  v-bind="props"></v-avatar>
                    </template>
                    <v-card min-width="300">
                        <ComCurrentUserAvatar/>

                        <v-divider></v-divider>
                        
                        <v-list density="compact">
                            <v-list-item  @click="onReload()">
                                <template v-slot:prepend class="w-12">
                                    <v-icon icon="mdi-reload"></v-icon>
                                </template>
                                <v-list-item-title>Reload</v-list-item-title>
                            </v-list-item>
                            <v-divider></v-divider>
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
import ComTimeUpdate from './components/ComTimeUpdate.vue';
import ComCurrentUserAvatar from './components/ComCurrentUserAvatar.vue';
import {useStore} from "@/plugin"

 
export default {
    inject: ["$auth","$sale"],
    
    name: "MainLayout",
    components: {
    ComProductSearch,
    SaleLayoutDrawer,
    MainLayoutDrawer,
    ComTimeUpdate,
    ComCurrentUserAvatar
},
computed: {
        currentUser(){
            return JSON.parse(localStorage.getItem('current_user'))
        }
    },
    data() {
        return {
            drawer: false
        }
    },
    methods: {
        onDrawer(){
            this.drawer = !this.drawer;
        },
        onReload(){
            location.reload()
        }
    },
}
</script>
<style lang="">
    
</style>