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
      <ComPaymentCurrencyPrefine/>
    
    </div>
  </div>
</template>
<script setup>
import { inject, ref,onMounted } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";
import Enumerable from "linq";
import ComPaymentCurrencyPrefine from "./ComPaymentCurrencyPrefine.vue";
const sale = inject("$sale")
const gv = inject("$gv")
const screen = inject("$screen")
const toaster = createToaster({ position: "top" });
 
function numpad_click(n) {
  if (n == "." && sale.paymentInputNumber.includes(".")) {
    return;
  }
  if (!sale.paymentInputNumber) {
    sale.paymentInputNumber = "";
  }
  sale.paymentInputNumber = sale.paymentInputNumber + n;
}

</script>