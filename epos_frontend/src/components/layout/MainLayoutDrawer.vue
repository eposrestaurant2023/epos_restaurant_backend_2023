<template lang="">
    <div>
        <ComCurrentUserAvatar/>
        <v-divider></v-divider>

        <v-list
          :lines="false"
          density="compact"
          nav
        >
          <v-list-item active-color="primary" @click="onRoute('Home')">
            <template v-slot:prepend>
              <v-icon>mdi-home-outline</v-icon>
            </template>
            <v-list-item-title>{{$t('Home')}}</v-list-item-title>
          </v-list-item>
         
          <v-list-item v-if="gv.workingDay!='' && device_setting?.is_order_station==0" active-color="primary" @click="onCloseWorkingDay()">
            <template v-slot:prepend>
              <v-icon>mdi-calendar</v-icon>
            </template>
            <v-list-item-title>{{$t('Close Working Day')}}</v-list-item-title>
          </v-list-item>

          <v-list-item v-if="gv.workingDay=='' && device_setting?.is_order_station==0" active-color="primary" @click="onStartWorkingDay()">
            <template v-slot:prepend>
              <v-icon>mdi-calendar</v-icon>
            </template>
            <v-list-item-title>{{$t('Start Working Day')}}</v-list-item-title>
          </v-list-item>

          <v-list-item v-if="gv.cashierShift=='' && device_setting?.is_order_station==0" active-color="primary" @click="onOpenShift()">
            <template v-slot:prepend>
              <v-icon>mdi-clock</v-icon>
            </template>
            <v-list-item-title>{{$t('Start Shift')}}</v-list-item-title>
          </v-list-item>

          <v-list-item  v-if="gv.cashierShift !='' && device_setting?.is_order_station==0" active-color="primary" @click="onCloseShift()">
            <template v-slot:prepend>
              <v-icon>mdi-calendar-clock</v-icon>
            </template>
            <v-list-item-title>{{$t('Close Shift')}}</v-list-item-title>
          </v-list-item>

          <v-list-item active-color="primary" @click="onPOS()">
            <template v-slot:prepend>
              <v-icon>mdi-cart-outline</v-icon>
            </template>
            <v-list-item-title>{{$t('POS')}}</v-list-item-title>
          </v-list-item>

          <v-list-item active-color="primary" v-if="device_setting?.is_order_station==0" @click="onRoute('ClosedSaleList')">
            <template v-slot:prepend>
              <v-icon>mdi-file-document</v-icon>
            </template>
            <v-list-item-title>{{$t('Closed Receipt')}}</v-list-item-title>
          </v-list-item>

          <v-list-item active-color="primary" v-if="device_setting?.is_order_station==0" @click="onRoute('ReceiptList')">
            <template v-slot:prepend>
              <v-icon>mdi-file-chart</v-icon>
            </template>
            <v-list-item-title>{{$t('Receipt List')}}</v-list-item-title>
          </v-list-item>

          <v-list-item active-color="primary" v-if="device_setting?.is_order_station==0" @click="onRoute('Customer')">
            <template v-slot:prepend>
              <v-icon>mdi-account-multiple</v-icon>
            </template>
            <v-list-item-title>{{$t('Customer')}}</v-list-item-title>
          </v-list-item>

          <v-list-item active-color="primary" v-if="device_setting?.is_order_station==0" @click="onCashInCashOut">
            <template v-slot:prepend>
              <v-icon>mdi-currency-usd</v-icon>
            </template>
            <v-list-item-title>{{$t('Cash Drawer')}}</v-list-item-title>
          </v-list-item>

          <v-list-item active-color="primary" @click="onOpenCashDrawer" v-if="isWindow() && device_setting?.is_order_station==0">
            <template v-slot:prepend>
              <v-icon>mdi-cash-multiple</v-icon>
            </template>
            <v-list-item-title>{{$t('Open Cash Drawer')}}</v-list-item-title>
          </v-list-item>

          
          <v-list-item active-color="primary" @click="onRoute('Report')" v-if="device_setting?.is_order_station==0">
            <template v-slot:prepend>
              <v-icon>mdi-chart-bar</v-icon>
            </template>
            <v-list-item-title>{{$t('Report')}}</v-list-item-title>
          </v-list-item>
          
          <v-divider class="my-4"></v-divider>

          <v-list-item color="error" @click="onLogout()">
            <template v-slot:prepend>
              <v-icon>mdi-logout</v-icon>
            </template>
            <v-list-item-title>{{$t('Logout')}}</v-list-item-title>
          </v-list-item>
        </v-list>
    </div>
</template>
<script setup>
import { useRouter, inject, createResource,i18n} from '@/plugin';
import ComCurrentUserAvatar from './components/ComCurrentUserAvatar.vue';
import { createToaster } from '@meforma/vue-toaster';
 
const { t: $t } = i18n.global; 

const router = useRouter();
const auth = inject('$auth');
const gv  = inject("$gv");
const toaster = createToaster({position:'top'});

const device_setting = JSON.parse(localStorage.getItem("device_setting"))
 
function onRoute(page) {
    router.push({name:page});
}

function onLogout(){
  auth.logout().then((r) => {
        router.push({ name: 'Login' })
    })
}

function onPOS(){
    const setting = JSON.parse(localStorage.getItem('setting'))
    if(setting.table_groups.length > 0){
        router.push({ name: 'TableLayout' })
    }
    else{
      gv.authorize("open_order_required_password","make_order").then((v)=>{
          if(v){ 
              const make_order_auth = {"username":v.username,"name":v.user,discount_codes:v.discount_codes }; 
              localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));

              router.push({ name: 'AddSale' })
          }
      });                    
    }
}

function onStartWorkingDay() {
    gv.authorize("start_working_day_required_password", "start_working_day").then(async (v) => {
        if (v) {
            router.push({ name: "StartWorkingDay" });
        }
    })

}

async function onCloseWorkingDay() {
    gv.authorize("close_working_day_required_password", "close_working_day").then(async (v) => {
        if (v) {
            const cashierShiftResource = createResource({
                url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
                params: {
                    pos_profile: localStorage.getItem("pos_profile")
                }
            });

            await cashierShiftResource.fetch().then(async (v) => {
                if (v) {
                    toaster.warning($t("msg.Please close shift first"))
                } else {
                    router.push({ name: "CloseWorkingDay" });
                }

            })
        }
    });
}

function onOpenShift() {
    gv.authorize("start_cashier_shift_required_password", "start_cashier_shift").then(async (v) => {
        if (v) {
            router.push({ name: "OpenShift" });
        }
    });
}
function onCashInCashOut() {
    gv.authorize("cash_in_check_out_required_password", "cash_in_check_out").then(async (v) => {
        if (v) {
            router.push({ name: "CashDrawer" });
        }
    });
}
function onCloseShift() {
    gv.authorize("close_cashier_shift_required_password", "close_cashier_shift").then(async (v) => 
    {
      if (v) {
        router.push({ name: "CloseShift" });
      }
    });
}

function isWindow(){
    return localStorage.getItem('is_window') == 1;
}


//open cashdrawer
function onOpenCashDrawer(){
    window.chrome.webview.postMessage(JSON.stringify({action:"open_cashdrawer"}));
} 
 
</script>
