<template >
    <v-btn v-if="(sale.tableSaleListResource?.data?.length||0)>0" class="text-none" stacked :size="mobile ? 'x-small' : 'small'" @click="onSearchSale">
        <v-badge :content="sale.tableSaleListResource?.data?.length" color="success">
            <v-icon>mdi-cart</v-icon>
        </v-badge>
    </v-btn>
</template>
<script setup>
import { useDisplay } from 'vuetify'
import {searchSaleDialog, inject,useRouter} from '@/plugin'
// import { createToaster } from '@meforma/vue-toaster'; 
const { mobile } = useDisplay()
const sale = inject('$sale')
const router = useRouter();
// const socket = inject('$socket');
// const toaster = createToaster({position:"top"});
const setting = JSON.parse(localStorage.getItem("setting"))
async function onSearchSale(){
    const isOrdered = sale.isOrdered(`Please ${setting.table_groups && setting.table_groups.length > 0 ? 'submit' : 'save'} your current order first`)
    if(isOrdered == false) {
        const result = await searchSaleDialog({ })
        if(result != false){ 
            router.push({
                name: "AddSale", params: {
                    name: result.name
                }
            });

            sale.LoadSaleData(result.name)
        }
    }
}
</script>