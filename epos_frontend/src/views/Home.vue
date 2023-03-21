<template>
    <div>
        <div class="h-60 bg-no-repeat bg-cover"
            v-bind:style="{ 'background-image': 'url(' + gv.setting.login_background + ')' }">
            <div class="wrap-overlay w-full h-full flex items-end justify-center">
                <div>
                    <div class="text-center text-white mb-3">
                        <img :src="gv.setting.logo" class="w-24 inline-block mb-2" />
                        <p class="text-xl">{{ gv.setting.business_branch }}</p>
                        <p>
                            <span class="font-bold">POS Profile</span> : {{ gv.setting.pos_profile }} /
                            <span class="font-bold">Outlet</span> : {{ gv.setting.outlet }} /
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
                        <ComButton @click="onViewPendingOrder()" title="Pending Order" icon="mdi-receipt"
                            icon-color="#e99417" />
                        <ComButton @click="onRoute('ReceiptList')" title="Closed Receipt" icon="mdi-receipt"
                            icon-color="#e99417" />
                        <ComButton @click="onRoute('Customer')" title="Customer" icon-color="#e99417"
                            icon="mdi-account-multiple-outline" />
                        <ComButton @click="onRoute('CashDrawer')" title="Cash Drawer" icon-color="#e99417"
                            icon="mdi-currency-usd" />
                        <ComButton @click="onRoute('Report')" title="Report" icon="mdi-chart-bar" icon-color="#e99417" />
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
import { useRouter, createResource, computed, createToaster,pendingSaleListDialog,inject } from '@/plugin'
import ComButton from '../components/ComButton.vue';
import WorkingDayButton from './shift/components/WorkingDayButton.vue';
import OpenShiftButton from './shift/components/OpenShiftButton.vue';
const toaster = createToaster({ position: "top" })
const auth = inject('$auth')
const gv = inject('$gv');

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
    }
});

const router = useRouter()

function onRoute(page) {
    router.push({ name: page })
}

async function onPOS() {
    await cashierShiftResource.fetch().then(async (v) => {
        if (v) {
            if (v.working_day == null) {
                toaster.warning("Please start working first.")
            } else if (v.cashier_shift == null) {
                toaster.warning("Please start cashier shift first.")
            } else {
                if (gv.setting.table_groups.length > 0) {
                    router.push({ name: 'TableLayout' })
                }
                else {
                    gv.authorize("open_order_required_password","make_order").then((v)=>
                    {
                        if(v){ 
                            router.push({ name: 'AddSale' })
                        }
                    })
                    
                }
            }


        }


    })
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
            toaster.warning("Please start working first.")
        }
        
    })

    
}



function onLogout() {

    auth.logout().then((r) => {
        router.push({ name: 'Login' })
    })
}
    




</script>


<style scoped>
.wrap-overlay {
    background: rgb(0, 0, 0);
    background: linear-gradient(7deg, rgba(0, 0, 0, 1) 0%, rgba(0, 212, 255, 0) 100%);
}
</style>