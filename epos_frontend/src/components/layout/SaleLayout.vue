<template>
    <v-app>
        <v-app-bar :elevation="2" color="error">
            <template #prepend>
                <v-app-bar-nav-icon size="small" variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
                <v-app-bar-title>
                    <div :class="mobile ? 'text-xs' : ''">
                        POS
                        <span v-if="$sale.sale.tbl_number">- {{ $sale.sale.tbl_number }}</span>

                        <span v-if="$sale.sale.sale_status == 'New'"> - New</span>
                        <span v-else> - {{ $sale.sale.name }}</span>

                        <v-chip class="ml-2" variant="elevated" v-if="$sale.sale.name" :color="$sale.sale.sale_status_color"
                            :size="mobile ? 'x-small' : 'default'">
                            {{ $sale.sale.sale_status }}
                        </v-chip>
                    </div>
                </v-app-bar-title>
            </template>

            <template #title v-if="!mobile">
                <ComProductSearch />
            </template>
            <template #append>
                <ComTimeUpdate />
                <v-btn icon="mdi-fullscreen" @click="onFullScreen()" v-if="!$gv.isFullscreen && isWindow"></v-btn>
                <v-btn icon="mdi-fullscreen-exit" @click="onFullScreen()" v-if="$gv.isFullscreen && isWindow"></v-btn>

                <ComSaleNotivication />


                <v-menu :location="location">
                    <template v-slot:activator="{ props }">
                        <v-avatar :image="currentUser.photo" v-bind="props"></v-avatar>
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

import ComSaleNotivication from './ComSaleNotification.vue';
import { useDisplay } from 'vuetify'
import { useRouter } from '@/plugin';

export default {
    inject: ["$auth", "$sale","$gv"],

    name: "MainLayout",
    components: {
        ComProductSearch,
        SaleLayoutDrawer,
        MainLayoutDrawer,
        ComTimeUpdate,
        ComCurrentUserAvatar,
        ComSaleNotivication
    },
    setup() {
        const { mobile } = useDisplay()

        return {
            mobile,
    
        }
    },
    computed: {
        currentUser() {
            return JSON.parse(localStorage.getItem('current_user'))
        },
        isWindow(){
            return localStorage.getItem('is_window')=='1';
        }
    },
    data() {
        return {
            drawer: false
        }
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
        },

        onFullScreen(){
            window.chrome.webview.postMessage(JSON.stringify({action:"toggle_fullscreen", "is_full": this.$gv.isFullscreen?"0":"1"}));
            this.$gv.isFullscreen = this.$gv.isFullscreen?false:true;
        }
    },
}

</script>