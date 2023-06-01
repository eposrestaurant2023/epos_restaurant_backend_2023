<template>
  <div class="bg-blue-100 rounded-tl-md rounded-tr-md text-xs w-full">
    <ComSaleSummaryList />
    <div class="bg-white">
      <ComSaleButtonActions @onSubmitAndNew="onCloseSubmit()" @onClose="onClose()"/>
    </div>
    <div class="flex w-full">
      <div style="width: calc(100% - 100px);" class="cursor-pointer bg-green-600 text-white px-2 py-1 hover:bg-green-700" @click="onPayment()">
        <div class="flex justify-between mb-1 text-sm">
          <div>{{ $t('Payment') }}</div>
          <div style="font-size: 24px; font-weight: bold;">
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
      <div style="width: 130px;">
        <v-btn
          height="100%"
          width="100%"
          stacked 
          rounded="0"
          class="p-1"
          style="background-color: #2563eb; color: #fff;"
          @click="onSubmit()">
          <div>
            <v-icon icon="mdi-arrow-right"></v-icon>
            <div class="text-xs" >{{ $t('Submit Order') }}</div>
          </div>
        </v-btn>
      </div>
    </div>
  </div>
</template>
<script setup>
import { inject, useRouter, paymentDialog,i18n } from '@/plugin';
import { createToaster } from '@meforma/vue-toaster';
import ComExchangeRate from '../ComExchangeRate.vue';
import ComSaleButtonActions from '../ComSaleButtonActions.vue';
import ComSaleSummaryList from '../ComSaleSummaryList.vue';

const { t: $t } = i18n.global;  

const emit = defineEmits(["onClose",'onSubmitAndNew'])
const router = useRouter()
const sale = inject("$sale")
const gv = inject("$gv")
const setting = gv.setting;
const toaster = createToaster({ position: "top" })
const device_setting = JSON.parse(localStorage.getItem("device_setting"))
async function onSubmit() {
  if (!sale.isBillRequested()) {

    const action = sale.action
    const message = sale.message;
    const sale_status = sale.sale.sale_status;

    sale.action = "submit_order";
    sale.message = $t("msg.Submit order successfully");
    sale.sale.sale_status = "Submitted";

    await sale.onSubmit().then((doc) => {
      if (doc) {
        emit('onClose')
      }else{
        sale.action = action;
        sale.message = message;
        sale.sale.sale_status = sale_status;
      }
    });
  }
}
async function onPayment() {
  if (device_setting.show_option_payment==0){
    return
  }

  if (sale.sale.sale_products.length == 0) {
    toaster.warning($t('msg.Please select a menu item to submit order'));
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