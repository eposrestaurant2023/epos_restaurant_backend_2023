<template lang="">
    <PageLayout class="pb-4" title="Cash Drawer" icon="mdi-currency-usd">
        <v-container class="!py-0">
        <div class="bg-gray-100 mb-2 rounded-sm" :class="mobile ? 'p-2' : 'p-4 text-lg'">
            <div class="text-center text-gray-400 mb-2 text-sm">Cash Drawer Balance</div>
            <div class="mb-5 text-center">
                <div class="mb-2">
                    $ 2,322,255
                    <!-- <CurrencyFormat :value="2555" /> -->
                </div>
                <div>
                    ៛ 120,200,000.00
                    <!-- <CurrencyFormat :value="0205" /> -->
                </div>
            </div>
            <div class="flex justify-around">
                <div>
                    <v-btn color="primary" @click="onCash('Cash In')">Cash In</v-btn>
                </div>
                <div>
                    <v-btn color="error" @click="onCash('Cash Out')">Cash Out</v-btn>
                </div>
            </div>
        </div>
        <div>
            <v-timeline>
                <v-timeline-item size="large">
                <template v-slot:icon>
                    <v-avatar v-if="false" image="https://i.pravatar.cc/64"></v-avatar>
                    <avatar v-else name="title" size="40"></avatar>
                </template>
                <template v-slot:opposite>
                    <span>12:00 AM</span>
                </template>
                <v-card class="elevation-2">
                    <v-card-title class="text-h5">
                    Lorem ipsum
                    </v-card-title>
                    <v-card-text>Lorem ipsum dolor sit amet, no nam oblique veritus. Commune scaevola imperdiet nec ut, sed euismod convenire principes at. Est et nobis iisque percipit, an vim zril disputando voluptatibus, vix an salutandi sententiae.</v-card-text>
                </v-card>
                </v-timeline-item>
            </v-timeline>
        </div>
        </v-container>
    </PageLayout>
    
</template>
<script setup>
import PageLayout from '@/components/layout/PageLayout.vue';
import {addCashDrawerModalDialog} from '@/plugin'
import {useDisplay} from 'vuetify'
const {mobile} = useDisplay()

function onCash(cash_type){
    const result = addCashDrawerModalDialog({name: cash_type})
}

</script> 