<template>
    <v-app>
        <v-app-bar :elevation="2">
            <template #prepend>
                <v-app-bar-nav-icon variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
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
                            <v-list-item>
                                <template v-slot:prepend class="w-12">
                                    <v-icon icon="mdi-logout"></v-icon>
                                </template>
                                <v-list-item-title  @click="$auth.logout()">Logout</v-list-item-title>
                            </v-list-item>
                        </v-list>
                    </v-card>
                    <!-- 
                    <v-list>
                        <v-list-item
                            prepend-avatar="https://cdn.vuetifyjs.com/images/john.png"
                            title="John Leider"
                            subtitle="john@google.com"
                        >
                            <template v-slot:append>
                            <v-btn
                                size="small"
                                variant="text"
                                icon="mdi-menu-down"
                            ></v-btn>
                            </template>
                        </v-list-item>
                    </v-list>
                    <v-divider></v-divider>
                    <v-list
                    :lines="false"
                    density="compact"
                    nav
                    >
                    <v-list-item>
                        <template v-slot:prepend>
                        <v-icon icon="mdi-home"></v-icon>
                        </template>
                        <v-list-item-title v-if="$auth.isLoggedIn" @click="$auth.logout()">Logout</v-list-item-title>
                    </v-list-item>
                    </v-list> -->
                </v-menu>
            </template>
        </v-app-bar>
        <v-navigation-drawer v-model="drawer">...</v-navigation-drawer>
        <v-main>
            <router-view />
        </v-main>
    </v-app>
</template>
<script>
import ComProductSearch from '../../views/sale/components/ComProductSearch.vue'
export default {
    inject: ["$auth"],
    name: "MainLayout",
    computed: {
        drawer(){
            return this.$store.state.drawer;
        }
    },
    components: {
        ComProductSearch
    },
    methods: {
        onDrawer(){
            this.$store.commit('drawer',!this.drawer)
        }
    },
}
</script>
<style lang="">
    
</style>