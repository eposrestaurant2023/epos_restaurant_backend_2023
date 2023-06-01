<template> 
    <div v-if="sale.sale.total_paid > 0 || balance > 0 || sale.sale.changed_amount > 0" class="mt-auto bg-green-600 border border-gray-500 text-white rounded-sm px-1 pt-1" :class="mobile ? 'mx-1' : ''">
        <div class="mb-1 flex justify-between" v-if="sale.sale.total_paid > 0">
            <div>Total Payment:</div>
            <div>
                <CurrencyFormat :value="sale.sale.total_paid" />
            </div>
        </div>
        
        <div class="mb-1 flex justify-between" v-if="balance > 0">
            <div>Balance ({{sale.setting.pos_setting.main_currency_name}}):</div>
            <div>
                <CurrencyFormat :value="balance" />
            </div>
        </div>

        <div class="mb-1 flex justify-between" v-if="balance > 0">
            <div>Balance ({{sale.setting.pos_setting.second_currency_name}}):</div>
            <div>
                <CurrencyFormat :value="balance * sale.sale.exchange_rate" :currency="sale.setting.pos_setting.second_currency_name"/>                
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
import { inject,computed } from 'vue'
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const sale = inject('$sale')
const gv = inject('$gv')

const balance = computed(()=>{
    if(sale.sale?.balance>0){ 
    return Number(sale.sale.balance.toFixed(gv.setting.pos_setting.main_currency_precision));
    }else {
        return 0;
    }
})





</script>