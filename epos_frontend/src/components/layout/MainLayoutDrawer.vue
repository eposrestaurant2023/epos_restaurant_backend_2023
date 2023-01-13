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
          <v-list-item active-color="primary" @click="onRoute('StartWorkingDay')">
            <template v-slot:prepend>
              <v-icon>mdi-calendar-clock</v-icon>
            </template>
            <v-list-item-title>Start Working Day</v-list-item-title>
          </v-list-item>
          <v-list-item active-color="primary" @click="onRoute('OpenShift')">
            <template v-slot:prepend>
              <v-icon>mdi-clock</v-icon>
            </template>
            <v-list-item-title>Start Cashier Shift</v-list-item-title>
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
          <v-list-item active-color="primary" @click="onRoute('Customer')">
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
import { useRouter, inject } from '@/plugin'
import ComCurrentUserAvatar from './components/ComCurrentUserAvatar.vue'
const router = useRouter()
const auth = inject('$auth')
function onRoute(page) {

    router.push({name:page})

}
function onLogout(){
    auth.logout()
}
function onPOS(){
    const setting = JSON.parse(localStorage.getItem('setting'))
    if(setting.table_groups.length > 0){
        router.push({ name: 'Table' })
    }
    else{
        router.push({ name: 'AddSale'})
    }
}
</script>
<style lang="">
    
</style>