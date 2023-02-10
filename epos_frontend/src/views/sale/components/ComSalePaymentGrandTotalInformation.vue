<template>
    <div class="bg-red-200 mb-2 rounded-sm" :class="mobile ? 'p-2 font-bold' : 'p-4 text-lg'" @click="emit('onClick')">
        <div class="text-center">Total Amount</div>
        <div class="flex justify-around">
            <div>
                <CurrencyFormat :value="sale.sale.grand_total" />
            </div>
            <div>
                <CurrencyFormat :value="sale.sale.grand_total * sale.sale.exchange_rate"
                    :currency="gv.setting?.pos_setting?.second_currency_name" />
            </div>
        </div>
    </div>
</template>
<script setup>
import { inject, defineEmits } from 'vue'
import { useDisplay} from 'vuetify'
const {mobile} = useDisplay()
const sale = inject('$sale')
const gv = inject('$gv')
const emit = defineEmits()
</script>