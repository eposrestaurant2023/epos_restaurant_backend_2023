<template>
    <div class="wrap">
        <template v-if="!show_thankyou">
            <v-row class="h-full !m-0">
                <v-col class="h-full !p-0 a" cols="hide" xs="12" sm="7" md="7" lg="7" xl="7">
                    <ComCustomerDisplaySliceshow />
                </v-col>
                <v-col class="h-100vh !p-0" cols="12" xs="12" sm="5" md="5" lg="5" xl="5    ">
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

socket.on("ShowOrderInCustomerDisplay", async (arg, show) => {
    data.value = arg;
    console.log(data.value)
    if (Object.entries(data.value).length > 0) {
        dataThankYou.value = JSON.parse(JSON.stringify(data.value))
    }

    if (show == 'paid') {
        show_thankyou.value = true
        await setTimeout(onHideThankYou, 5000)
    }
    else if (show == "new") {
        onHideThankYou()
    }
})

function onHideThankYou() {
    show_thankyou.value = false
}
</script>
<style scoped></style>