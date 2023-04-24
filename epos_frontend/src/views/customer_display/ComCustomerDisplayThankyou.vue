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
                        <div>THANK YOU </div>
                        <div>SEE YOU AGAIN!</div>
                    </div>
                    <div class="h-40 w-40 border-4 border-green-600 m-auto rounded-full">
                        <div class="h-10 w-10 m-auto bg-gray-300 rounded-full"></div>
                        <div class="text-center	text-small text-xs text-gray-400">{{ data?.customer_photo }}</div>
                    </div>
                    <div>
                        <div class="flex justify-evenly mt-6">
                            <div>
                                <div class="text-2xl">
                                    <div>
                                        <CurrencyFormat :value="data.grand_total" />
                                    </div>
                                    <div>
                                        <CurrencyFormat :value="data.grand_total * data.exchange_rate"
                                            :currency="gv.setting.pos_setting.second_currency_name" />
                                    </div>
                                </div>
                                <div class="text-xs text-gray-500">Bill amount</div>
                            </div>
                            <div class="border-l-2"></div>
                            <div>
                                <div class="text-2xl">
                                    <div>
                                        <CurrencyFormat :value="data.changed_amount" />
                                    </div>
                                    <div>
                                        <CurrencyFormat :value="data.changed_amount * data.exchange_rate"
                                            :currency="gv.setting.pos_setting.second_currency_name" />
                                    </div>
                                </div>
                                <div class="text-xs text-gray-500">Change</div>
                            </div>
                        </div>
                    </div>
                    <div class="text-center text-gray-500">Paid amount:
                        <CurrencyFormat :value="data.total_paid" /> /
                        <CurrencyFormat :value="data.total_paid * data.exchange_rate"
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
 