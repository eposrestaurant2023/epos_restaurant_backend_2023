<template>

        <div>
    <div>
      <div class="flex">
        <div class="w-4/5 cursor-pointer bg-green-600 text-white p-2 hover:bg-green-700" @click="onPayment()">
          <div class="flex justify-between mb-2 text-lg">
            <div>Payment</div>
            <div>
              <CurrencyFormat :value="sale.sale.grand_total" />
            </div>
          </div>
          <div class="flex justify-between">
            <div>Total Qty : <span>{{ sale.sale.total_quantity }}</span></div>
            <div>
              <ComExchangeRate />
              <CurrencyFormat :value="sale.sale.grand_total * sale.sale.exchange_rate"
                :currency="setting.pos_setting.second_currency_name" />
            </div>
          </div>
        </div>
        <div class="w-1/5">
          <div
            class="w-full h-full cursor-pointer flex justify-center items-center bg-blue-600 text-white p-3 hover:bg-blue-700 text-center"
            @click="onSubmit()">
            <div>
              <v-icon icon="mdi-arrow-right-thick"></v-icon>
              <div>Submit Order</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { inject, useRouter, confirmBackToTableLayout, paymentDialog } from '@/plugin';
import ComExchangeRate from './ComExchangeRate.vue';
const sale = inject("$sale")
async function onSubmit() {
  if (!sale.isBillRequested()) {
    sale.action = "submit_order";
    sale.message = "Submit Order Successfully";
    sale.sale.sale_status = "Submitted";
    await sale.onSubmit().then((value) => {
      if (value) {
        if (sale.sale.table_id)
          router.push({ name: 'TableLayout' })
        else
          sale.newSale()
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
    router.push({ name: "TableLayout" });
  }

}
</script>
<style lang="">
    
</style>