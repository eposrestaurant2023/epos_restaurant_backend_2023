<template> 
    <div class="mt-auto bg-green-600 border border-gray-500 text-white rounded-sm px-1 pt-1" :class="mobile ? 'mx-1' : ''">
        <div class="mb-1 flex justify-between" v-if="sale.sale.total_paid > 0">
            <div>Total Payment:</div>
            <div>
                <CurrencyFormat :value="sale.sale.total_paid" />
            </div>
        </div>
        <div class="mb-1 flex justify-between" v-if="sale.sale.balance > 0">
            <div>Balance:</div>
            <div>
                <CurrencyFormat :value="sale.sale.balance" />
            </div>
        </div>
        <div class="mb-1 flex justify-between" v-if="sale.sale.changed_amount > 0">
            <div>Change Amount:</div>
            <div>
                <CurrencyFormat :value="sale.sale.changed_amount" />
            </div>
        </div>
        <div class="mb-1 flex justify-between" v-if="sale.sale.changed_amount > 0">
            <div>Change Amount {{ gv.setting.pos_setting.second_currency_name }}:</div>
            <div>
                <CurrencyFormat :value="sale.sale.changed_amount * sale.sale.exchange_rate"
                    :currency="gv.setting.pos_setting.second_currency_name" />
            </div>
        </div>
    </div> 
</template>
<script setup>
import { inject } from 'vue'
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const sale = inject('$sale')
const gv = inject('$gv')
</script>