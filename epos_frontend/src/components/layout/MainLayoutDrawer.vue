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
              <v-icon>mdi-home</v-icon>
            </template>

            <v-list-item-title>Home</v-list-item-title>
          </v-list-item>
          
          <v-list-item v-if="workingDayResource.data" active-color="primary" @click="onRoute('CloseWorkingDay')">
            <template v-slot:prepend>
              <v-icon>mdi-calendar-clock</v-icon>
            </template>
            <v-list-item-title>Close Working Day</v-list-item-title>
          </v-list-item>
          <v-list-item v-else active-color="primary" @click="onRoute('StartWorkingDay')">
            <template v-slot:prepend>
              <v-icon>mdi-calendar-clock</v-icon>
            </template>
            <v-list-item-title>Start Working Day</v-list-item-title>
          </v-list-item>

        

          


          <v-list-item v-if="!cashierShiftResource.data" active-color="primary" @click="onRoute('OpenShift')">
            <template v-slot:prepend>
              <v-icon>mdi-clock</v-icon>
            </template>
            <v-list-item-title>Start Cashier Shift</v-list-item-title>
          </v-list-item>
          <v-list-item  v-else active-color="primary" @click="onRoute('CloseShift')">
            <template v-slot:prepend>
              <v-icon>mdi-calendar-clock</v-icon>
            </template>
            <v-list-item-title>Close Cashier Shift</v-list-item-title>
          </v-list-item>

          <v-list-item active-color="primary" @click="onPOS()">
            <template v-slot:prepend>
              <v-icon>mdi-cart</v-icon>
            </template>
            <v-list-item-title>POS</v-list-item-title>
          </v-list-item>
          <v-list-item active-color="primary" @click="onRoute('ReceiptList')">
            <template v-slot:prepend>
              <v-icon>mdi-receipt</v-icon>
            </template>
            <v-list-item-title>Receipt List</v-list-item-title>
          </v-list-item>
          <v-list-item active-color="primary" @click="onRoute('Customer')">
            <template v-slot:prepend>
              <v-icon>mdi-account-multiple</v-icon>
            </template>
            <v-list-item-title>Customer</v-list-item-title>
          </v-list-item>
          <v-list-item active-color="primary" @click="onRoute('CashDrawer')">
            <template v-slot:prepend>
              <v-icon>mdi-currency-usd</v-icon>
            </template>
            <v-list-item-title>Cash Drawer</v-list-item-title>
          </v-list-item>
          
          <v-list-item active-color="primary" @click="onRoute('Report')">
            <template v-slot:prepend>
              <v-icon>mdi-chart-bar</v-icon>
            </template>
            <v-list-item-title>Report</v-list-item-title>
          </v-list-item>
          <v-list-item active-color="primary" @click="onRoute('Setting')">
            <template v-slot:prepend>
              <v-icon>mdi-wrench</v-icon>
            </template>
            <v-list-item-title>Setting</v-list-item-title>
          </v-list-item>
          <v-divider class="my-4"></v-divider>
          <v-list-item color="error" @click="onLogout()">
            <template v-slot:prepend>
              <v-icon>mdi-logout</v-icon>
            </template>
            <v-list-item-title>Logout</v-list-item-title>
          </v-list-item>
        </v-list>
    </div>
</template>
<script setup>
import { useRouter, inject, createResource} from '@/plugin'
import ComCurrentUserAvatar from './components/ComCurrentUserAvatar.vue'
const router = useRouter()
const auth = inject('$auth')
const gv  = inject("$gv")
function onRoute(page) {

    router.push({name:page})

}
function onLogout(){
    auth.logout()
}
function onPOS(){
    const setting = JSON.parse(localStorage.getItem('setting'))
    if(setting.table_groups.length > 0){
        router.push({ name: 'TableLayout' })
    }
    else{
        router.push({ name: 'AddSale'})
    }
}

let cashierShiftResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
})

const workingDayResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        business_branch: gv.setting.business_branch
    },
    auto: true
})


</script>
<style lang="">
    
</style>