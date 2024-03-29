<template>
    <div> 
        <div class="h-60 bg-no-repeat bg-cover"
            v-bind:style="{ 'background-image': 'url(' + gv.setting.home_background + ')' }">
            <div class="wrap-overlay w-full h-full flex items-end justify-center">
                <div>
                    <div class="text-center text-white mb-3">
                        <img :src="gv.setting.logo" class="w-24 inline-block mb-2" />
                        <p class="text-xl">{{ gv.setting.business_branch }}</p>
                        <p>
                            <span class="font-bold">{{ $t('POS Profile') }}</span> : {{ gv.setting.pos_profile }} /
                            <span class="font-bold">{{ $t('Outlet') }}</span> : {{ gv.setting.outlet }} /
                            <span class="font-bold">{{ $t('Device Name') }}</span> :{{ device_name }}
                        </p>                        
                    </div>
                </div>
            </div>
        </div>
        <v-container>
            <div class="pb-16">
                <div class="mx-auto mt-4 mb-0 md:w-[600px]">
                    <ComMessagePromotion />
                    <div class="grid xs:grid-cols-2 md:grid-cols-4 grid-cols-2" style="grid-gap: 20px;">
                        <WorkingDayButton  v-if="device_setting?.show_start_close_working_day==1 && device_setting?.is_order_station==0"/>
                        <OpenShiftButton  v-if="device_setting?.show_start_close_cashier_shift==1 && device_setting?.is_order_station==0"/>
                        
                        <ComButton @click="onPOS()" :title="$t('POS')" icon="mdi-cart-outline" class="bg-green-600 text-white" icon-color="#fff" />
                        <ComButton @click="onViewPendingOrder()" :title="$t('Pending Order')" icon="mdi-arrange-send-backward"  icon-color="#e99417" />


                        <ComButton @click="onRoute('ClosedSaleList')" :title="$t('Closed Receipt')" v-if="device_setting?.is_order_station==0 && (gv.workingDay || gv.cashierShift)" icon="mdi-file-document"  icon-color="#e99417" />
                        
                        
                        <ComButton @click="onRoute('ReceiptList')" :title="$t('Receipt List')" v-if="device_setting?.is_order_station==0" icon="mdi-file-chart"  icon-color="#e99417" />

                        <ComButton @click="onRoute('Customer')" :title="$t('Customer')" v-if="device_setting?.is_order_station==0" icon-color="#e99417"  icon="mdi-account-multiple-outline" />
                        <ComButton @click="onCashInCashOut" :title="$t('Cash Drawer')" v-if="device_setting?.is_order_station==0" icon-color="#e99417" icon="mdi-currency-usd" />
                        <ComButton v-if="isWindow() && device_setting?.is_order_station==0"  @click="onOpenCashDrawer" :title="$t('Open Cash Drawer')" icon="mdi-cash-multiple" icon-color="#e99417" />
                        
                        <ComButton @click="onRoute('Report')" :title="$t('Report')" v-if="device_setting?.is_order_station==0" icon="mdi-chart-bar" icon-color="#e99417" />
                
                        <ComButton v-if="isWindow() && device_setting?.show_button_customer_display==1"  @click="onOpenCustomerDisplay"  :title="$t('Customer Display')" icon="mdi-monitor" icon-color="#e99417" />

                        <ComButton v-if="isWindow()"  @click="onPrintWifiPassword" :title="$t('Wifi Password')" icon="mdi-wifi" icon-color="#e99417" /> 
                        
                        <ComButton @click="onLogout()" text-color="#fff" icon-color="#fff" :title="$t('Logout')" icon="mdi-logout" background-color="#b00020" />
                    </div>
                </div>
            </div>
        </v-container>
    </div>
    
</template>
<script setup>
import { useRouter, createResource, computed, createToaster,pendingSaleListDialog,inject,onMounted,printWifiPasswordModal,i18n } from '@/plugin'
import ComButton from '../components/ComButton.vue';
import WorkingDayButton from './shift/components/WorkingDayButton.vue';
import OpenShiftButton from './shift/components/OpenShiftButton.vue';
import ComMessagePromotion from '../components/ComMessagePromotion.vue';

const { t: $t } = i18n.global; 
const toaster = createToaster({ position: "top" });
const auth = inject('$auth')
const gv = inject('$gv');
const sale = inject('$sale');
const router = useRouter();
const device_setting = JSON.parse(localStorage.getItem("device_setting"));
let already_load_confirm_close_working_day = false;

function isWindow(){
    return localStorage.getItem('is_window') == 1;
}

const device_name = computed(() => {
    return localStorage.getItem('device_name')
})

const cashierShiftResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_shift_information",
    params: {
        business_branch: gv.setting?.business_branch,
        pos_profile: localStorage.getItem("pos_profile")
    },
});


const workingDayResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
      business_branch: gv.setting?.business_branch
    },
    onSuccess(doc){
        if(!already_load_confirm_close_working_day){
                if(doc){
                already_load_confirm_close_working_day = true;
                gv.confirm_close_working_day(doc.posting_date);
            }
        } 
    }
});


//on init
onMounted(async () => {
    localStorage.removeItem('make_order_auth');    
    await workingDayResource.fetch();
})


function onRoute(page) {   
    router.push({ name: page })
}

async function onCashInCashOut(){
    await gv.authorize("cash_in_check_out_required_password", "cash_in_check_out").then(async (v) => {
        if (v) {
            router.push({ name: "CashDrawer" });
        }
    });
}

async function onPOS() {
    await cashierShiftResource.fetch().then(async (v) => {
        if (v) {
            if (v.working_day == null) {
                toaster.warning($t("msg.Please start working day first"))
            } else if (v.cashier_shift == null) {
                toaster.warning($t("msg.Please start shift first"))
            } else {
                if (gv.setting.table_groups.length > 0) {
                    router.push({ name: 'TableLayout' })
                }
                else {
                    gv.authorize("open_order_required_password","make_order").then((v)=>
                    {
                        if(v){  
                            const make_order_auth = {"username":v.username,"name":v.user,discount_codes:v.discount_codes }; 
                            localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                            router.push({ name: 'AddSale' })                            
                        }
                    })                    
                }
            }
        }
    })
}

function onOpenCustomerDisplay(){
    window.chrome.webview.postMessage(JSON.stringify({ action: "open_customer_display" }));
}

async function onViewPendingOrder() {
    let working_day = '';
    let cashier_shift = '';
    await workingDayResource.fetch().then(async (wk)=>{
        if(wk.name){
            working_day = wk.name
            await cashierShiftResource.fetch().then(async (cs)=>{
                if(cs.cashier_shift?.name)
                    cashier_shift = cs.cashier_shift.name
                const result = await pendingSaleListDialog({data:{working_day:working_day, cashier_shift: cashier_shift}})
            })
        }else{
            toaster.warning($t("msg.Please start working day first"))
        }
        
    })
}

function onLogout() {
    auth.logout().then((r) => {
        router.push({ name: 'Login' })
    })
}
    



async function onPrintWifiPassword(){
    await printWifiPasswordModal({})
}



//open cashdrawer
function onOpenCashDrawer(){
    window.chrome.webview.postMessage(JSON.stringify({action:"open_cashdrawer"}));
} 

</script>
<style scoped>
.wrap-overlay {
    background: rgb(0, 0, 0);
    background: linear-gradient(7deg, rgba(0, 0, 0, 1) 0%, rgba(0, 212, 255, 0) 100%);
}
</style>