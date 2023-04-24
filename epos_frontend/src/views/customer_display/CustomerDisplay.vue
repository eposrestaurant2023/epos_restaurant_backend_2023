<template>
    <div class="wrap">
        <template v-if="!show_thankyou">
            <v-row class="h-full !m-0">
                <v-col class="h-full !p-0 a" cols="hide" xs="12" sm="8" md="8" lg="8" xl="8">
                    <ComCustomerDisplaySliceshow />
                </v-col>
                <v-col class="h-100vh !p-0" cols="12" xs="12" sm="4" md="4" lg="4" xl="4">
                    <ComCustomerDisplayOrderList :data="data" />
                </v-col>
            </v-row>
        </template>
        <template v-else>
            <ComCustomerDisplayThankyou :data="dataThankYou" />
        </template>

    </div>
</template>
<script setup>
import { inject } from '@/plugin';
import { ref } from 'vue';
import ComCustomerDisplaySliceshow from './ComCustomerDisplaySliceshow.vue';
import ComCustomerDisplayThankyou from './ComCustomerDisplayThankyou.vue';
import ComCustomerDisplayOrderList from './ComCustomerDisplayOrderList.vue';
const data = ref({})
const dataThankYou = ref({})
const socket = inject("$socket")
const show_thankyou = ref(false)
const open = ref(true)

socket.on("ShowOrderInCustomerDisplay", async (arg, show) => {
    data.value = arg; 
    if(Object.entries(data.value).length > 0){ 
        dataThankYou.value = JSON.parse(JSON.stringify(data.value))
    }
    
    if(Object.entries(data.value).length > 0 && show != true){
        onHideThankYou()
    }
    else if (show) {
        show_thankyou.value = true
        await setTimeout(onHideThankYou, 5000)
    }
})

function onHideThankYou() {
    show_thankyou.value = false
}
</script>
<style scoped></style>