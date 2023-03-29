<template>
  <div class="bg-blue-100 rounded-tl-md rounded-tr-md text-xs w-full">
    <ComSaleSummaryList />
    <div class="bg-white">
      <ComSaleButtonActions @onSubmitAndNew="onCloseSubmit()" @onClose="onClose()"/>
    </div>
    <div class="flex w-full">
      <div style="width: calc(100% - 100px);" class="cursor-pointer bg-green-600 text-white p-1 hover:bg-green-700" @click="onPayment()">
        <div class="flex justify-between mb-1 text-sm">
          <div>Payment</div>
          <div>
            <CurrencyFormat :value="sale.sale.grand_total" />
          </div>
        </div>
        <div class="text-right">
          <div>
            <ComExchangeRate />
            <CurrencyFormat :value="sale.sale.grand_total * sale.sale.exchange_rate"
              :currency="setting.pos_setting.second_currency_name" />
          </div>
        </div>
      </div>
      <div style="width: 100px;">
        <v-btn
          stacked 
          rounded="0"
          class="p-1"
          style="background-color: #2563eb; color: #fff;"
          @click="onSubmit()">
          <div>
            <v-icon icon="mdi-arrow-right"></v-icon>
            <div class="text-xs">Submit Order</div>
          </div>
        </v-btn>
      </div>
    </div>
  </div>
</template>
<script setup>
import { inject, useRouter, paymentDialog } from '@/plugin';
import { createToaster } from '@meforma/vue-toaster';
import ComExchangeRate from '../ComExchangeRate.vue';
import ComSaleButtonActions from '../ComSaleButtonActions.vue';
import ComSaleSummaryList from '../ComSaleSummaryList.vue';
const emit = defineEmits(["onClose",'onSubmitAndNew'])
const router = useRouter()
const sale = inject("$sale")
const gv = inject("$gv")
const setting = gv.setting;
const toaster = createToaster({ position: "top" })

async function onSubmit() {
  if (!sale.isBillRequested()) {
    sale.action = "submit_order";
    sale.message = "Submit Order Successfully";
    sale.sale.sale_status = "Submitted";
    await sale.onSubmit().then((doc) => {
      if (doc) {
        emit('onClose')
      }
    });
  }
}
async function onPayment() {
  if (sale.sale.sale_products.length == 0) {
    toaster.warning("Please select a menu item to submit order");
    return
  }
  else if (sale.onCheckPriceSmallerThanZero()) {
    return;
  }
  const result = await paymentDialog({})
  if (result) {
    emit('onClose')
  }
}
function onCloseSubmit(){
  emit('onSubmitAndNew')
}
function onClose(){
  emit('onClose')
}
</script>