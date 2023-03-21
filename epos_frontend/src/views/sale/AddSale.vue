<template>
    <ComLoadingDialog
        v-if="sale.newSaleResource?.loading || (sale.saleResource != null && sale.saleResource?.get.loading) || (sale.saleResource != null && sale.saleResource?.setValue.loading)" />
    <div style="height: calc(100vh - 64px)" id="tst">
        <ComSmallAddSale v-if="mobile" />
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
                        <ComGroupSaleProductList />
                    </div>
                    <div class="mt-auto">
                        <div class="-mx-1 bg-blue-100 rounded-tl-md rounded-tr-md text-xs">
                            <ComSaleSummaryList />
                            <ComSaleButtonPaymentSubmit />
                        </div>
                    </div>
                </div>
            </v-col>
        </v-row>
    </div>
</template>
<script setup>
import { inject, useRoute, useRouter, ref, onMounted, onUnmounted, onBeforeRouteLeave,createResource} from '@/plugin';
import Enumerable from 'linq';
import ComMenu from './components/ComMenu.vue';
import ComSelectCustomer from './components/ComSelectCustomer.vue';
import ComSaleInformation from '@/views/sale/components/ComSaleInformation.vue';
import ComLoadingDialog from '../../components/ComLoadingDialog.vue';
import { useDisplay } from 'vuetify'
import ComSmallAddSale from './components/mobile_screen/ComSmallAddSale.vue';
import ComGroupSaleProductList from './components/ComGroupSaleProductList.vue';
import ComSaleSummaryList from './components/ComSaleSummaryList.vue';
import ComSaleButtonPaymentSubmit from './components/ComSaleButtonPaymentSubmit.vue';
import { createToaster } from '@meforma/vue-toaster';
const { mobile, name, platform } = useDisplay()

const sale = inject("$sale")
const gv = inject("$gv")
const product = inject("$product")
let openSearch = ref(false)
const route = useRoute()
const router = useRouter()

const toaster = createToaster({ position: "top" })

sale.orderTime = null;
sale.deletedSaleProducts = []
if (sale.orderBy == null) {
    sale.orderBy = JSON.parse(localStorage.getItem("current_user")).full_name;
}

sale.orderTime = "";
if (product.posMenuResource.data?.length == 0) {
    product.loadPOSMenu();
}



document.onkeydown = function (e) {
    if (e.keyCode === 116) {
        return false;
    }
};
// small device
function onSearchProduct(open) {
    openSearch.value = open
}

onMounted(() => { 
    if (sale.getString(route.params.name) == "") {
        if (sale.sale.sale_status == undefined) {
            if (sale.setting.table_groups.length > 0) {
                router.push({ name: 'TableLayout' })
            }
            else {
                sale.newSale();
            }
        }
    }
 
    //check working day and cashier shift
    createResource({
        url: "epos_restaurant_2023.api.api.get_current_shift_information",
        params: {
            business_branch: sale.setting?.business_branch,
            pos_profile: localStorage.getItem("pos_profile")
        },
        auto: true,
        onSuccess(data) { 
            if (data.cashier_shift == null) {
                toaster.warning("Please start cashier shift first");
                router.push({name:"OpenShift"});
            } else if(data.working_day==null){
                toaster.warning("Please start working day first");
                router.push({name:"StartWorkingDay"});
            }else {
                sale.sale.working_day = data.working_day.name;
                sale.sale.cashier_shift = data.cashier_shift.name;
                sale.working_day = data.working_day.name;
                sale.cashier_shift = data.cashier_shift.name;
            }
        }
    })


    //load sale data 

    if (!sale.getString(route.params.name) == "") {
        sale.LoadSaleData(route.params.name).then((v) => {
            if (v) {
                if (v.docstatus == 1 || v.docstatus == 2) {
                    
                    if(v.docstatus==1){
                        toaster.warning("This sale order is already closed");
                        
                    }else{
                        toaster.warning("This sale order is already cancel");
                    }
                    if (gv.setting.table_groups.length > 0) {
                        router.push({ name: 'TableLayout' });
                    }
                    else {
                        router.push({ name: 'Home' });
                    }
                }

                sale.getTableSaleList();
            }
        });
    }else{
        sale.getTableSaleList()
    }
})

onBeforeRouteLeave(() => {

    const sp = Enumerable.from(sale.sale.sale_products);

    if (sp.where("$.name==undefined").toArray().length > 0) {
        toaster.warning("Please save or submit your current order.");
        return false;
    } else {
        return true;
    }
});

onUnmounted(() => {

    sale.sale = {}
    sale.working_day_resource = null;
    sale.cashier_shift_resource = null;
    sale.newSaleResource = null;
    sale.saleResource = null;
    sale.tableSaleListResource = null;


})

</script>
