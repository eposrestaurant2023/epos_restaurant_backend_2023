<template>
    <v-app>
        <v-app-bar :elevation="2" color="error" >
            <template #prepend>
                <v-app-bar-nav-icon size="small" variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
                <template v-if="mobile"> 
                    <v-btn icon  @click="onBack('TableLayout')" v-if="gv.setting.table_groups.length > 0" >
                        <v-icon >mdi-arrow-left</v-icon> 
                    </v-btn>
                    <v-btn icon  @click="onBack('Home')" v-else >
                        <v-icon >mdi-home-outline</v-icon> 
                    </v-btn>
                </template>
                <v-app-bar-title>
                    <div :class="mobile ? 'text-xs' : ''">
                        POS
                        <span v-if="sale.sale.tbl_number">- {{ sale.sale.tbl_number }}</span>
                        <span v-if="sale.sale.sale_status == 'New'"> - {{ $t('New') }}</span>
                        <span v-else> - {{ sale.sale.name }}</span>

                        <v-chip class="ml-2" variant="elevated" v-if="sale.sale.name" :color="sale.sale.sale_status_color"
                            :size="mobile ? 'x-small' : 'default'">
                            {{ sale.sale.sale_status }}
                        </v-chip>
                    </div>
                </v-app-bar-title>
            </template>

            <template #title v-if="!mobile">
                <ComProductSearch />
            </template>
            <template #append>
                <ComTimeUpdate />
                <template v-if="isWindow">
                    <v-btn :icon="(!gv.isFullscreen?'mdi-fullscreen':'mdi-fullscreen-exit')" @click="onFullScreen()"></v-btn>
                </template>
                <ComSaleNotivication />


                <v-menu :location="location">
                    <template v-slot:activator="{ props }">
                        <v-avatar v-if="currentUser?.photo" :image="currentUser.photo" v-bind="props"></v-avatar>
                        <avatar v-else :name="currentUser?.full_name || 'No Name'" v-bind="props" class="cursor-pointer" size="40"></avatar>
                    </template>
                    <v-card min-width="300">
                        <ComCurrentUserAvatar /> 
                        <v-divider></v-divider> 
                        <v-list density="compact">
                            <v-list-item @click="onReload()">
                                <template v-slot:prepend class="w-12">
                                    <v-icon icon="mdi-reload"></v-icon>
                                </template>
                                <v-list-item-title>{{ $t('Reload') }}</v-list-item-title>
                            </v-list-item>
                            <v-divider></v-divider>
                            <v-list-item @click="onLogout">
                                <template v-slot:prepend class="w-12">
                                    <v-icon icon="mdi-logout"></v-icon>
                                </template>
                                <v-list-item-title>{{ $t('Logout') }}</v-list-item-title>
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
                    {{ $t('Close') }}
                </v-btn>
            </template>
        </v-navigation-drawer>
        <v-main>
            <router-view />
        </v-main>
    </v-app>
</template>
<script setup>
    import ComProductSearch from '../../views/sale/components/ComProductSearch.vue'
    import MainLayoutDrawer from './MainLayoutDrawer.vue';
    import ComTimeUpdate from './components/ComTimeUpdate.vue';
    import ComCurrentUserAvatar from './components/ComCurrentUserAvatar.vue';
    import ComSaleNotivication from './ComSaleNotification.vue';
    import { useDisplay } from 'vuetify';
    import { useRouter ,ref,inject,confirmBackToTableLayout } from '@/plugin';
    import { computed } from 'vue';
    import Enumerable from 'linq';
    const emit = defineEmits('closeModel')

    const sale = inject("$sale")
    const auth = inject("$auth")
    const gv = inject("$gv")

    const { mobile } = useDisplay();
    const router = useRouter();


    const currentUser = computed(()=>{
        return JSON.parse(localStorage.getItem('current_user'))
    });

    const isWindow = computed(()=>{
        return localStorage.getItem('is_window')=='1';
    })

    let drawer = ref(false);


    function onDrawer() {
        drawer.value = !drawer.value;
    }
    function onReload() {
        location.reload()
    }

    function onLogout(){
        const isOrdered = sale.isOrdered()
        if(isOrdered == false){
            auth.logout().then((r)=>{
                router.push({name: 'Login'})
            })
        }
    }

    function onFullScreen(){
        window.chrome.webview.postMessage(JSON.stringify({action:"toggle_fullscreen", "is_full": gv.isFullscreen?"0":"1"}));
        gv.isFullscreen = gv.isFullscreen?false:true;
    }

 
    
    async function onBack(_router) {   
    
        const sp = Enumerable.from(sale.sale.sale_products);
        if (sp.where("$.name==undefined").toArray().length > 0) {
            let result = await confirmBackToTableLayout({});
            if (result) {
            if (result == "hold" || result == "submit") {
                if (result == "hold") {
                sale.sale.sale_status = "Hold Order";
                sale.action = "hold_order";
                } else {
                sale.sale.sale_status = "Submitted";
                sale.action = "submit_order";
                }
                await sale.onSubmit().then(async (value) => {
                    if (value) {                
                        router.push({name:_router})                
                    }
                });
            } else {
                //continue
                sale.sale = {};
                router.push({name:_router})    
            }}
        } else {
            sale.sale = {};
            router.push({name:_router})    
        }
    }
    

</script>