<template>
    <v-app>
        <v-app-bar :elevation="2" height="50">
            <v-app-bar-title class="text-center">{{ appTitle }}</v-app-bar-title> 
            <template #append v-if="gv.setting.allow_make_order==1">
                <v-btn class="text-none" stacked :disabled="total_items<=0" @click.stop="onSaleOrderClick()">
                    <v-badge v-if="total_items>0" :content="total_items" color="error">
                        <v-icon icon="mdi-cart-outline"></v-icon>
                    </v-badge>
                    <v-icon v-else icon="mdi-cart-outline"></v-icon>
                </v-btn>
            </template>
        </v-app-bar>
        
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

    const router = useRouter()
    const drawer = ref( false)
    const isFullscreen = ref( true)
    const appTitle = ref('ePOS Menu QR')
    const sale = inject("$sale")
    const gv = inject("$gv")
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