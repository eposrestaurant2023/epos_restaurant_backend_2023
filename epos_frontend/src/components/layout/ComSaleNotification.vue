<template>
    <v-btn class="text-none" stacked :size="mobile ? 'x-small' : 'small'" @click="onSearchSale">
        <v-badge :content="sale.tableSaleListResource?.data?.length" color="success">
            <v-icon>mdi-cart</v-icon>
        </v-badge>
    </v-btn>
</template>
<script setup>
import { useDisplay } from 'vuetify'
import {searchSaleDialog, inject,useRouter} from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import Enumerable from 'linq'
const { mobile } = useDisplay()
const sale = inject('$sale')
const router = useRouter();
const socket = inject('$socket');
const toaster = createToaster({position:"top"});
async function onSearchSale(){
    const sp = Enumerable.from(sale.sale.sale_products);

if (sp.where("$.name==undefined").toArray().length > 0) {
    toaster.warning("Please submit your current order first");
    
}else {
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