<template>
    <div class="bg-blue-100 rounded-tl-md rounded-tr-md text-xs w-full">
        <ComSaleSummaryList/>
        <div class="flex w-full">
          <div class="w-4/5 cursor-pointer bg-green-600 text-white p-1 hover:bg-green-700" @click="onPayment()">
            <div class="flex justify-between mb-1 text-sm">
              <div>Payment</div>
              <div>
                <CurrencyFormat :value="sale.sale.grand_total" />
              </div>
            </div>
            <div class="text-right"> 
              <div>
                <ComExchangeRate/>
                <CurrencyFormat :value="sale.sale.grand_total * sale.sale.exchange_rate"
                  :currency="setting.pos_setting.second_currency_name" />
              </div>
            </div>
          </div>
          <div class="w-1/5">
            <div
              class="w-full h-full cursor-pointer flex justify-center items-center bg-blue-600 text-white p-1 hover:bg-blue-700 text-center"
              @click="onSubmit()">
              <div>
                <v-icon icon="mdi-arrow-right-thick"></v-icon>
                <div>Submit Order</div>
              </div>
            </div>
          </div>
        </div> 
    </div>
  
  </template>
  <script setup>
  import { inject, useRouter,paymentDialog } from '@/plugin'; 
  import { createToaster } from '@meforma/vue-toaster';
    import ComExchangeRate from '../ComExchangeRate.vue';
import ComSaleSummaryList from '../ComSaleSummaryList.vue';
  
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
      await sale.onSubmit().then((value) => {
        if (value) {
          router.push({ name: "TableLayout" });
          sale.mobile_view_sale_product = false
        }
      });
    }
  }
  async function onPayment(){
    if(sale.sale.sale_products.length == 0){
      toaster.warning("Please select a menu item to submit order");
      return
    }
    else if(sale.onCheckPriceSmallerThanZero()){
      return;
    }
    const result = await paymentDialog({})
    if(result){
      router.push({ name: "TableLayout" });
      sale.mobile_view_sale_product = false
    }
    
  }
  </script>