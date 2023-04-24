<template>
    <div class="wrap">
        <template v-if="show_thankyou">
            <v-row class="h-full !m-0">
                <v-col class="h-full !p-0 a" cols="hide" xs="12" sm="8" md="8" lg="8" xl="8">
                    <ComCustomerDisplaySliceshow />
                </v-col>
                <v-col class="h-100vh !p-0" cols="12" xs="12" sm="4" md="4" lg="4" xl="4">
                    <div class="h-full flex-col flex">
                        <template v-if="data.customer">
                            <div class="profile  px-4 flex bg-stone-400 pt-3 pb-3 ">
                                <div class="avatar-profile">
                                    <v-avatar v-if="data?.customer_photo">
                                        <v-img :src="data?.customer_photo"></v-img>
                                    </v-avatar>

                                    <avatar v-else :name="data?.customer_name || 'No Customer'" class="mr-4" size="40">
                                    </avatar>
                                </div>
                                <div class="px-5 pt-2 text-xl">{{ data.customer_name }}</div>
                            </div>
                        </template>
                        <div class="product-list overflow-auto h-full">
                            <ul class="bg-violet-950" style=" padding: 12px;">
                                <li v-for="(p, index) in data.sale_products" :key="index" class="border-b">
                                    <div class="flex">
                                        <div class="avatar-profile">
                                            <v-avatar v-if="p.product_photo">
                                                <v-img :src="p.product_photo"></v-img>
                                            </v-avatar>
                                            <avatar v-else :name="p.product_code" class="mr-4" size="40"></avatar>
                                        </div>
                                        <div>
                                            <div>{{ p.product_code }}</div>
                                            <div>{{ p.price }} x {{ p.quantity }}</div>
                                            {{ p.product_name }}
                                        </div>
                                        <div style="position: absolute; right: 3%; font-size: 20px;">
                                            <CurrencyFormat :value="p.amount"></CurrencyFormat>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="summary bg-green-500 p-5">
                            <div class="flex">
                                <div>Total: </div>
                                <div class="b">
                                    <CurrencyFormat :value="data.grand_total" />
                                </div>
                            </div>
                            <div class=" flex">
                                <div> total Item: </div>
                                <div class="b">
                                    <CurrencyFormat :value="data.grand_total * data.exchange_rate"
                                        :currency="gv.setting.pos_setting.second_currency_name" />
                                </div>
                            </div>
                        </div>

                    </div>
                </v-col>
            </v-row>
        </template>
        <template v-else>
            <ComCustomerDisplayThankyou :data="data" />
        </template>

    </div>
</template>
<script setup>
import { inject } from '@/plugin';
import { ref } from 'vue';
import ComCustomerDisplaySliceshow from './ComCustomerDisplaySliceshow.vue';
import ComCustomerDisplayThankyou from './ComCustomerDisplayThankyou.vue';
const data = ref({})
const socket = inject("$socket")
const gv = inject("$gv")
const show_thankyou = ref(false)
const open = ref(true)

socket.on("ShowOrderInCustomerDisplay", async (arg, show) => {

    data.value = arg;
    if (show) {
        show_thankyou.value = true
        await setTimeout(onHideThankYou, 5000)
    }
})

function onHideThankYou() {
    show_thankyou.value = false
}
</script>
<style scoped>
.te-xt {
    line-height: 60px;
}

.a {
    background-image: radial-gradient(500px 350px at 20% bottom, rgb(0 0 0 / 34%), rgb(0 0 0 / 3%));
}

.b {
    position: absolute;
    right: 5%;
}
</style>