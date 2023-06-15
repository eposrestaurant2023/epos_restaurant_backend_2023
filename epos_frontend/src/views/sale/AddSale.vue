<template>
    <ComLoadingDialog
        v-if="sale.newSaleResource?.loading || (sale.saleResource != null && sale.saleResource?.get.loading) || (sale.saleResource != null && sale.saleResource?.setValue.loading)" />
    <ComSmallAddSale v-if="mobile" />
    <div v-else style="height: calc(100vh - 64px)" id="tst">
        <v-row class="h-full ma-0">
            <v-col cols="12" sm="7" md="7" lg="8" class="pa-0 h-full d-none d-sm-block">
                <ComMenu :background-image="gv.setting.pos_sale_order_background_image" />
            </v-col>
            <v-col cols="12" sm="5" md="5" lg="4" class="h-full pa-0">
                <div class="h-full flex-col flex px-1">
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
                    <div class="overflow-auto h-full " :class="getCustomerScrollWidth()">
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
import { inject, useRoute, useRouter, ref, onMounted, onUnmounted, onBeforeRouteLeave, createResource,ShortCutKeyHelpDialog,i18n } from '@/plugin';
import { getCurrentInstance } from 'vue';
import ComMenu from './components/ComMenu.vue';
import ComSelectCustomer from './components/ComSelectCustomer.vue';
import ComSaleInformation from '@/views/sale/components/ComSaleInformation.vue';
import ComLoadingDialog from '../../components/ComLoadingDialog.vue';
import ComSmallAddSale from './components/mobile_screen/ComSmallAddSale.vue';
import ComGroupSaleProductList from './components/ComGroupSaleProductList.vue';
import ComSaleSummaryList from './components/ComSaleSummaryList.vue';
import ComSaleButtonPaymentSubmit from './components/ComSaleButtonPaymentSubmit.vue';
import { createToaster } from '@meforma/vue-toaster';
import { useDisplay } from 'vuetify';

const { t: $t } = i18n.global;

const { mobile } = useDisplay()

const sale = inject("$sale")
const gv = inject("$gv")
const socket = inject("$socket")

const product = inject("$product")
const frappe = inject("$frappe")
const db = frappe.db();
let openSearch = ref(false)
const route = useRoute()
const router = useRouter()

const toaster = createToaster({ position: "top" })

sale.vueInstance = getCurrentInstance();
sale.vue = sale.vueInstance.appContext.config.globalProperties

sale.orderTime = null;
sale.deletedSaleProducts = [];

sale.vue.$onKeyStroke('F1', (e) => {
    e.preventDefault();
    
    if(localStorage.getItem('dialogstate') === null){
        localStorage.setItem('dialogstate',1)
        ShortCutKeyHelpDialog()
    }  
})



sale.orderTime = "";
if (product.posMenuResource.data?.length == 0) {
    if(product.setting.pos_menus.length>0){
        product.loadPOSMenu();
    }else{
        
        product.getProductMenuByProductCategory(db,"All Product Categories")
        product.loadPOSMenu();
    }
    
}

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
                toaster.warning($t("msg.Please start shift first"));
                router.push({ name: "OpenShift" });
            } else if (data.working_day == null) {
                toaster.warning($t('msg.Please start working day first'));
                router.push({ name: "StartWorkingDay" });
            } else {
                sale.sale.working_day = data.working_day.name;
                sale.sale.cashier_shift = data.cashier_shift.name;
                sale.sale.shift_name = data.cashier_shift.shift_name;


                sale.working_day = data.working_day.name;
                sale.cashier_shift = data.cashier_shift.name;
                sale.shift_name = data.cashier_shift.shift_name;
                product.getProductMenuByProductCategory(db,'All Product Categories')
                gv.confirm_close_working_day(data.working_day.posting_date);
                
                onCheckExpireHappyHoursPromotion()
            }
        }
    })


    //load sale data
    if (!sale.getString(route.params.name) == "" && !sale.no_loading) {
        sale.LoadSaleData(route.params.name).then((v) => {
            if (v) {
                if (v.docstatus == 1 || v.docstatus == 2) {

                    if (v.docstatus == 1) {
                        toaster.warning($t('msg.This bill is already closed'));

                    } else {
                        toaster.warning($t('msg.This bill is already cancelled'));
                    }
                    if (gv.setting.table_groups.length > 0) {
                        router.push({ name: 'TableLayout' });
                    }
                    else {
                        router.push({ name: 'Home' });
                    }
                }
                socket.emit("ShowOrderInCustomerDisplay", sale.sale);
                sale.getTableSaleList();
            }
        });
    } else {
        sale.getTableSaleList()
    }
    socket.emit("ShowOrderInCustomerDisplay", sale.sale, "new");

})
function onCheckExpireHappyHoursPromotion(){
        createResource({
            url: 'epos_restaurant_2023.api.promotion.check_promotion',
            auto: true,
            params: { 
                check_time: 1,
                business_branch: gv.setting.business_branch || ''
            },
            onSuccess(doc) {
                gv.promotion = doc
                sale.promotion = doc
            }
        });
  
}
onBeforeRouteLeave(() => {
    return !sale.isOrdered()
});

onUnmounted(() => {

    sale.sale = {}
    sale.working_day_resource = null;
    sale.cashier_shift_resource = null;
    sale.newSaleResource = null;
    sale.saleResource = null;
    sale.tableSaleListResource = null;

    socket.emit("ShowOrderInCustomerDisplay", {}, true);

})


function getCustomerScrollWidth(){
    const is_window = localStorage.getItem('is_window');
    if(is_window==1){
        return 'scrollbar';
    }
    return '';
}

</script>

 
<style>

    .scrollbar::-webkit-scrollbar {
        width: 17px;
    }

</style>