<template>
    <v-app>
        <v-app-bar :elevation="2" height="50">
            <v-app-bar-title class="text-center">{{ appTitle }}</v-app-bar-title>
            <template #prepend>
                <v-app-bar-nav-icon variant="text" @click.stop="onDrawer()"></v-app-bar-nav-icon>
            </template>
            <template #append>
                <v-btn class="text-none" stacked :disabled="total_items<=0" @click.stop="onSaleOrderClick()">
                    <v-badge v-if="total_items>0" :content="total_items" color="error">
                        <v-icon icon="mdi-cart-outline"></v-icon>
                    </v-badge>
                    <v-icon v-else icon="mdi-cart-outline"></v-icon>
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
        </v-navigation-drawer>
        <v-main class="bg-gray-100"> 
            <v-sheet rounded max-width="750px" class="m-auto">
                <router-view />
            </v-sheet> 
        </v-main>
    </v-app>
</template>
<script setup>
    import {ref,inject,computed} from 'vue';
    import {useRouter} from 'vue-router';
    import ComCurrentUserAvatar from '../views/components/ComCurrentUserAvatar.vue';

    const router = useRouter()
    const drawer = ref( false)
    const isFullscreen = ref( true)
    const appTitle = ref('ePOS Menu QR')
    const sale = inject("$sale")
    function onRoute(page) {
        router.push({name:page})
    }

    function onDrawer() {
        drawer.value = !drawer.value;
    }


    const total_items = computed(()=>{
        return sale.sale.sale_products.length;
    })


    function onSaleOrderClick(){ 
        sale.onSaleOrderClick()
    }
    
</script>
<style lang="">
    
</style>