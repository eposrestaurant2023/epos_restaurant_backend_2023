<template>
    <ComLoadingDialog
        v-if="sale.newSaleResource?.loading || (sale.saleResource != null && sale.saleResource?.get.loading) || (sale.saleResource != null && sale.saleResource?.setValue.loading)" />
    <div style="height: calc(100vh - 64px)" id="tst">
        <ComSmallAddSale v-if="mobile"/>
        <v-row class="h-full ma-0" v-else>
            <v-col cols="12" sm="7" md="7" lg="8" class="pa-0 h-full d-none d-sm-block">
                <ComMenu :background-image="gv.setting.pos_sale_order_background_image" />
                
            </v-col>
            <v-col cols="12" sm="5" md="5" lg="4" class="h-full pa-0">
                <div class="h-full grid px-1" style="grid-template-rows: max-content;">
                    <div class="mb-1">
                        <div class="flex justify-between items-center">
                            <div class="flex-grow">
                                <ComSaleInformation />
                            </div>
                            <div class="flex-none d-block d-sm-none">
                                <v-btn color="primary" type="button" @click="onSearchProduct(true)">
                                    <v-icon icon="mdi-filter-outline"></v-icon>
                                </v-btn>
                            </div>
                        </div>
                        <ComSelectCustomer />
                    </div>
                    <div class="overflow-auto">
                        <ComGroupSaleProductList/>
                    </div>
                    <div class="mt-auto">
                        <div class="-mx-1 bg-blue-100 rounded-tl-md rounded-tr-md text-xs">
                            <ComSaleSummaryList/>
                            <ComSaleButtonActionRight/>
                            <ComSaleButtonPaymentSubmit/>
                        </div>
                    </div>
                </div>
            </v-col>
        </v-row>
    </div>
</template>
<script setup>
import { useStore, inject, useRoute, ref, onUnmounted } from '@/plugin';
import ComMenu from './components/ComMenu.vue';
import ComSelectCustomer from './components/ComSelectCustomer.vue';
import ComSaleInformation from '@/views/sale/components/ComSaleInformation.vue';
import ComLoadingDialog from '../../components/ComLoadingDialog.vue';
import { useDisplay } from 'vuetify'
import ComSmallAddSale from './components/mobile_screen/ComSmallAddSale.vue';
import ComGroupSaleProductList from './components/ComGroupSaleProductList.vue';
import ComSaleButtonActionRight from './components/ComSaleButtonActionRight.vue';
import ComSaleSummaryList from './components/ComSaleSummaryList.vue';
import ComSaleButtonPaymentSubmit from './components/ComSaleButtonPaymentSubmit.vue';
const { mobile, name, platform } = useDisplay()
const store = useStore()
const sale = inject("$sale")
const gv = inject("$gv")
const product = inject("$product")
let openSearch = ref(false)
const route = useRoute()
sale.orderTime = null;
sale.deletedSaleProducts = []
if (sale.orderBy == null) {
    sale.orderBy = JSON.parse(localStorage.getItem("current_user")).full_name;
}


sale.orderTime = "";


if (product.posMenuResource.data?.length == 0) {

    product.loadPOSMenu();
}

//check if new sale
if (route.params.name == "") {
    sale.newSale()
} else {
 
    sale.LoadSaleData(route.params.name);
    
}
sale.getTableSaleList();

document.onkeydown = function (e) {
    if (e.keyCode === 116) {
        return false;
    }
}; 
// small device
function onSearchProduct(open){
    openSearch.value = open
}

onUnmounted(() => {
    sale.sale = {}
})

</script>
