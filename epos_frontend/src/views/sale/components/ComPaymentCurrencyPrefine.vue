<template>
    <div
      v-if="gv.setting.pos_setting.main_currency_predefine_payment_amount"
      class="grid mb-2"
      :class="isMobile ? 'grid-cols-3 gap-2' : '-m-1 border rounded-sm p-1 grid-cols-4'">
 
        <div 
          class="flex items-center justify-center cursor-pointer border border-stone-500 rounded-sm text-center hover:bg-slate-300"
          :class="screen.small ? 'text-sm p-2' : 'p-3'"
          style="margin: 1px;"
          :key="index"
          v-for="(n, index) in gv.setting.pos_setting.main_currency_predefine_payment_amount.split(',')"
          @click="onMainCurrencyPrefineClick(n)">
          <CurrencyFormat :value="parseFloat(n)" />
        </div>
      
        <div 
        class="flex items-center justify-center cursor-pointer border border-stone-500 rounded-sm text-sm text-center hover:bg-slate-300"
        :class="screen.small ? 'text-sm p-2' : 'p-3'"
        style="margin: 1px;"
        :key="index"
        v-for="(n, index) in gv.setting.pos_setting.second_currency_predefine_payment_amount.split(',')"
        @click="onSecondCurrencyPrefineClick(n)">
        <CurrencyFormat :value="Number(n)" :currency="gv.setting?.pos_setting?.second_currency_name" />
        </div>
   
    </div>
  </template>
  <script setup>
  import { inject, defineEmits, defineProps } from "@/plugin"
  import { createToaster } from "@meforma/vue-toaster";
  import Enumerable from "linq";
  const sale = inject("$sale")
  const gv = inject("$gv")
  const screen = inject("$screen")
  const toaster = createToaster({ position: "top" });
  const emit = defineEmits('onSelected')
  const props = defineProps({
    isMobile: {
        type: Boolean,
        default: false
    }
  })
  function onMainCurrencyPrefineClick(n) {
    //get exchange rate
    const paymentType = Enumerable.from(gv.setting.payment_types).where(`$.payment_method=='${gv.setting.default_payment_type}'`).firstOrDefault();
    sale.onAddPayment(paymentType,n);
    emit('onSelected')
  }
  
  function onSecondCurrencyPrefineClick(n) {
    //get exchange rate
    const secondCurrencyPaymentType = gv.setting.second_currency_payment_type;
  
    if (secondCurrencyPaymentType) {
      const paymentType = Enumerable.from(gv.setting.payment_types).where(`$.payment_method=='${secondCurrencyPaymentType}'`).firstOrDefault();
     
      if (paymentType) {
        sale.onAddPayment(paymentType,n);
        emit('onSelected')
      } else {
        toaster.warning("There is no default payment for second currency");
      }
  
    } else {
      toaster.warning("There is no default payment for second currency");
    }
  }
  
  
  
  
  </script>