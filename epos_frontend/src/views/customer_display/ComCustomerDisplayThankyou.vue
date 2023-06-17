<template lang="">
    <div> 
        <v-row class="h-full !m-0">
                <v-col class="h-full !p-0 a" cols="hide" xs="12" sm="8" md="7" lg="7" xl="8">
                    <v-carousel height="100vh" cycle :show-arrows="false" hide-delimiters :interval="6000"
                        vertical-delimiters>
                        <v-carousel-item :src="item.photo" cover
                            v-for="(item, index) in gv.setting.pos_setting.customer_display_slideshow"
                            :key="index"></v-carousel-item>
                    </v-carousel>
                </v-col>
                <v-col class="h-100vh !p-0" cols="12" xs="12" sm="4" md="5" lg="5" xl="4">
                    <div class="te-xt text-center text-4xl mt-6 mb-6">
                        <div>{{$t('THANK YOU')}}</div>
                        <div>{{$t('SEE YOU AGAIN')}}!</div>
                    </div>
                    <div class="h-40 w-40 border-4 border-green-600 m-auto rounded-full sqare">
                        <div class="customer-profile">
                            <div class="font-bold text-lg">{{data.customer_name}}</div>
                            <div class="font-bold">{{data.phone_number}}</div>
                        </div>
                        <v-icon class="mdi mdi-check te-xtt"></v-icon>
                    </div>
                    <div>
                        <div class="flex justify-evenly mt-6">
                            <div>
                                <div class="text-2xl">
                                    <div>
                                        <CurrencyFormat :value="data.grand_total" />
                                    </div>
                                    <div>
                                        <CurrencyFormat :value="(data.grand_total || 0) * (data.exchange_rate || 0)"
                                            :currency="gv.setting.pos_setting.second_currency_name" />
                                    </div>
                                </div>
                                <div class="text-xs text-gray-500">{{$t('Bill Amount')}}</div>
                            </div>
                            <div class="border-l-2"></div>
                            <div>
                                <div class="text-2xl">
                                    <div>
                                        <CurrencyFormat :value="data.changed_amount" />
                                    </div>
                                    <div>
                                        <CurrencyFormat :value="(data.changed_amount || 0) * (data.exchange_rate || 0) "
                                            :currency="gv.setting.pos_setting.second_currency_name" />
                                    </div>
                                </div>
                                <div class="text-xs text-gray-500">{{$t('Change Amount')}}</div>
                            </div>
                        </div>
                    </div>
                    <div class="text-center text-gray-500">{{$t('Paid Amount')}}:
                        <CurrencyFormat :value="data.total_paid" /> /
                        <CurrencyFormat :value="(data.total_paid || 0) * (data.exchange_rate || 0)"
                            :currency="gv.setting.pos_setting.second_currency_name" />
                    </div>

                </v-col>
            </v-row>
    </div>
</template>
<script setup>
import { inject } from '@/plugin';
const props = defineProps({
    data: Object,
})
const gv = inject("$gv")
</script>
<style scoped>
.te-xt {
    line-height: 60px;
}

.a {
    background-image: radial-gradient(500px 350px at 20% bottom, rgb(0 0 0 / 34%), rgb(0 0 0 / 3%));
}

.sqare {
    position: relative;
}

.customer-profile {
    font-size: 11px;
    text-align: center;
    color: gray;
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
}

.te-xtt {
    transform: translate(-50%, -50%);
    position: absolute;
    top: 80%;
    left: 50%;
    font-size: 50px;
    color: #139313;
}
</style>
 