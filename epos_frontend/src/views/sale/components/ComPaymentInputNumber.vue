<template>
  <div class="h-full">
    <div class="overflow-auto h-full p-2">
      <div class="mb-4">
        <v-text-field 
        label="Enter number"
        variant="solo"
        v-model="sale.paymentInputNumber"
        clearable 
        maxlength="10"
        autofocus
        ></v-text-field>
        <div>
          <div class="grid grid-cols-3 gap-2">
            <v-btn @click="numpad_click('1')" size="large">
              1
            </v-btn>
            <v-btn @click="numpad_click('2')" size="large">
              2
            </v-btn>
            <v-btn @click="numpad_click('3')" size="large">
              3
            </v-btn>
            <v-btn @click="numpad_click('4')" size="large">
              4
            </v-btn>
            <v-btn @click="numpad_click('5')" size="large">
              5
            </v-btn>
            <v-btn @click="numpad_click('6')" size="large">
              6
            </v-btn>
            <v-btn @click="numpad_click('7')" size="large">
              7
            </v-btn>
            <v-btn @click="numpad_click('8')" size="large">
              8
            </v-btn>
            <v-btn @click="numpad_click('9')" size="large">
              9
            </v-btn>
            <v-btn @click="numpad_click('0')" size="large">
              0
            </v-btn>
            <v-btn @click="numpad_click('.')" size="large">
              .
            </v-btn>
            <v-btn color="error" @click="sale.paymentInputNumber = ''" size="large">
              Clear
            </v-btn>
          </div>
        </div>
      </div>
      <div class="grid grid-cols-4  mb-2 -m-1 border rounded-sm p-1">
        <!-- <v-btn 
          class="m-1 flex-grow"
          variant="tonal"
          v-for="(n, index) in gv.setting.pos_setting.main_currency_predefine_payment_amount.split(',')"
          :key="index" 
          @click="onMainCurrencyPrefineClick(n)">
            <CurrencyFormat :value="parseFloat(n)" />
        </v-btn> -->
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
          <CurrencyFormat :value="n" :currency="gv.setting?.pos_setting?.second_currency_name" />
        </div>
      </div>
      <!-- <div class="grid grid-cols-4 mb-2 -m-1 border rounded-sm p-1">
          <v-btn 
          class="m-1 flex-grow"
          v-for="(n, index) in gv.setting.pos_setting.second_currency_predefine_payment_amount.split(',')"
          :key="index"
          variant="tonal"
          @click="onSecondCurrencyPrefineClick(n)">
            <CurrencyFormat :value="n" :currency="gv.setting?.pos_setting?.second_currency_name" />
        </v-btn>
        <div 
          class="flex items-center justify-center cursor-pointer border border-stone-500 rounded-sm text-sm text-center hover:bg-slate-300"
          :class="screen.small ? 'text-sm p-2' : 'p-3'"
          style="margin: 1px;"
          :key="index"
          v-for="(n, index) in gv.setting.pos_setting.second_currency_predefine_payment_amount.split(',')"
          @click="onSecondCurrencyPrefineClick(n)">
          <CurrencyFormat :value="n" :currency="gv.setting?.pos_setting?.second_currency_name" />
        </div>
      </div>  -->
    </div>
  </div>
</template>
<script setup>
import { inject, ref,onMounted } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";
import Enumerable from "linq";
const sale = inject("$sale")
const gv = inject("$gv")
const screen = inject("$screen")
const toaster = createToaster({ position: "top" });
 
function numpad_click(n) {
  alert(n)
  if (n == "." && sale.paymentInputNumber.includes(".")) {
    return;
  }
  if (!sale.paymentInputNumber) {
    sale.paymentInputNumber = "";
  }
  sale.paymentInputNumber = sale.paymentInputNumber + n;
}

function onMainCurrencyPrefineClick(n) {
  //get exchange rate
  const paymentType = Enumerable.from(gv.setting.payment_types).where(`$.payment_method=='${gv.setting.default_payment_type}'`).firstOrDefault();
  sale.onAddPayment(paymentType,n);
}

function onSecondCurrencyPrefineClick(n) {
  //get exchange rate
  const secondCurrencyPaymentType = gv.setting.second_currency_payment_type;

  if (secondCurrencyPaymentType) {
    const paymentType = Enumerable.from(gv.setting.payment_types).where(`$.payment_method=='${secondCurrencyPaymentType}'`).firstOrDefault();
   
    if (paymentType) {
     
      sale.onAddPayment(paymentType,n);
    } else {
      toaster.warning("There is no default payment for second currency");
    }

  } else {
    toaster.warning("There is no default payment for second currency");
  }
}




</script>