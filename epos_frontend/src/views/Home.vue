<template>
    <div>
        <div class="h-60 bg-no-repeat bg-cover"
            v-bind:style="{ 'background-image': 'url(' + setting.login_background + ')' }">
            <div class="wrap-overlay w-full h-full flex items-end justify-center">
                <div>
                    <div class="text-center text-white mb-3">
                        <img :src="setting.logo" class="w-24 inline-block mb-2" />
                        <p class="text-xl">{{ setting.business_branch }}</p>
                        <p>
                            <span class="font-bold">POS Profile</span> : {{ setting.pos_profile }} /
                            <span class="font-bold">Outlet</span> : {{ setting.outlet }} /
                            <span class="font-bold">Device Name</span> :{{ device_name }}
                        </p>
                         
                    </div>
                </div>
            </div>
        </div>
        <v-container>
            <div class="pb-16">
                <div class="mx-auto mt-4 mb-0 md:w-[600px]">
                    <div class="grid xs:grid-cols-2 md:grid-cols-4 grid-cols-2" style="grid-gap: 20px;">
                        <WorkingDayButton />
                        <OpenShiftButton />
                        <ComButton @click="onPOS()" title="POS" icon="mdi-cart" class="bg-green-600 text-white"
                            icon-color="#fff" />
                        <ComButton @click="onRoute('ReceiptList')" title="Recept List" icon="mdi-receipt"
                            icon-color="#e99417" />
                        <ComButton @click="onRoute('Customer')" title="Customer" icon-color="#e99417"
                            icon="mdi-account-multiple-outline" />
                        <ComButton @click="onRoute('CashDrawer')" title="Cash Drawer" icon-color="#e99417"
                            icon="mdi-currency-usd" />
                        <ComButton title="Report" icon="mdi-chart-bar" icon-color="#e99417" />
                        <ComButton @click="onLogout()" text-color="#fff" icon-color="#fff" title="Logout" icon="mdi-logout"
                            background-color="#b00020" />
                        <ComButton @click="onRoute('TestPage')" title="Test Page" icon-color="#e99417"
                            icon="mdi-calendar-clock" />
                    </div>
                </div>
            </div>
        </v-container>
    </div>
</template>
<script setup>
import { useRouter, createResource, inject, computed } from '@/plugin'
import ComButton from '../components/ComButton.vue';
import WorkingDayButton from './shift/components/WorkingDayButton.vue';
import OpenShiftButton from './shift/components/OpenShiftButton.vue';

const auth = inject('$auth')
let setting = JSON.parse(localStorage.getItem("setting"))
 

const device_name = computed(() => {
    return localStorage.getItem('device_name')
})


const router = useRouter()
function onRoute(page) {
    router.push({ name: page })
}
function onPOS() {
    const setting = JSON.parse(localStorage.getItem('setting'))
    if (setting.table_groups.length > 0) {
        router.push({ name: 'TableLayout' })
    }
    else {
        router.push({ name: 'AddSale' })
    }
}
function onLogout() {

    auth.logout().then((r)=>{
        router.push({name: 'Login'})
    })
}

</script>


<style scoped>
.wrap-overlay {
    background: rgb(0, 0, 0);
    background: linear-gradient(7deg, rgba(0, 0, 0, 1) 0%, rgba(0, 212, 255, 0) 100%);
}
</style>