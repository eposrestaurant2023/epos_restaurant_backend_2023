<template lang="">
        <div class="h-full flex-col flex">
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
            <div class="product-list overflow-auto h-full">
                <div class="">
                    <ul class="bg-violet-950 items-group " style=" padding: 12px;">
                        <li  v-for="(p, index) in data.sale_products" :key="index" class="border-b item">
                            <div class="flex">
                                <div class="avatar-profile b">
                                    <v-avatar v-if="p.product_photo">
                                        <v-img :src="p.product_photo"></v-img>
                                    </v-avatar>
                                    <avatar v-else :name="p.product_code" size="40"></avatar>
                                </div>
                                <div style="width: 400px ">
                                    <div>{{ p.product_name }}</div>
                                    <div>{{ p.quantity }} x <CurrencyFormat :value="p.price"></CurrencyFormat></div>
                                    <!-- {{ p.product_name }} -->
                                </div>
                                <div style="right: 3%; font-size: 20px;width:100%;text-align:right;">
                                    <CurrencyFormat :value="p.amount"></CurrencyFormat>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="summary bg-green-500 p-5 text-white">
                <div class="flex justify-between">
                    <div class="">Total: </div>
                    <div>
                        <CurrencyFormat :value="data.grand_total" />
                    </div>
                </div>
                <div class=" flex justify-between">
                    <div> total Item: </div>
                    <div class="b">
                        <CurrencyFormat :value="data.grand_total * data.exchange_rate"
                            :currency="gv.setting.pos_setting.second_currency_name" />
                    </div>
                </div>
            </div>
        </div> 

</template>
<script setup>
import { inject } from '@/plugin';
const gv = inject("$gv")
const props = defineProps({
    data: Object,
})
</script>
<style scoped>
.b {
    display: grid;
    align-items: center;
    margin-right: 20px;


}

.items-group {
    overflow-y: scroll;
    scroll-snap-type: y mandatory;
}

.items-group,
.item {
    flex: 0 0 100%;
    scroll-snap-align: start;
}

.product-list {
    height: 76.4vh;
}
</style>