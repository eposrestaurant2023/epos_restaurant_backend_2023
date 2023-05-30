<template>
  <div>
    <div>
      <div class="flex">
        <div class="w-4/5 cursor-pointer bg-green-600 text-white px-2 py-0  hover:bg-green-700" @click="onPayment()">
          <div style="margin-bottom: 0px!important;" class="flex justify-between mb-2 text-lg">
            <div>{{ $t("Payment") }}</div>
            <div style="margin: 0px; padding: 0px; font-size: 26px; font-weight: bold;">
              <CurrencyFormat :value="sale.sale.grand_total" />
            </div>
          </div>
          <div class="flex justify-between">
            <div>{{ $t('Total Qty') }} : <span>{{ sale.sale.total_quantity ||0}}</span></div>
            <div>
              <ComExchangeRate />
              <CurrencyFormat :value="sale.sale.grand_total * (sale.sale.exchange_rate || 1)"
                :currency="sale.setting.pos_setting.second_currency_name" />
            </div>
          </div>
        </div>
        <div class="w-1/5">
          <div
            class="w-full h-full cursor-pointer flex justify-center items-center bg-blue-600 text-white p-3 hover:bg-blue-700 text-center"
            @click="onSubmit()">
            <div v-if="gv.setting.table_groups && gv.setting.table_groups.length > 0">
              <v-icon icon="mdi-arrow-right-thick"></v-icon>
              <div>{{ $t('Submit Order') }}</div>
            </div>
            <div v-else>
              <v-icon icon="mdi-content-save"></v-icon>
              <div>{{ $t('Save Order') }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { inject, useRouter, paymentDialog,searchSaleDialog, createToaster,i18n } from '@/plugin';
import ComExchangeRate from './ComExchangeRate.vue';
import { whenever,useMagicKeys  } from '@vueuse/core';
const { t: $t } = i18n.global;  

const sale = inject("$sale")
const gv = inject("$gv")
const product = inject("$product")
const tableLayout = inject("$tableLayout");
const router = useRouter();
const toaster = createToaster({ position: 'top' });
const device_setting = JSON.parse(localStorage.getItem("device_setting"))

sale.vue.$onKeyStroke('F12', (e) => {
    e.preventDefault()
    if(sale.dialogActiveState==false){
      onPayment();
    } 
})

const { ctrl_o } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'o' && e.type === 'keydown')
        e.preventDefault()
    },
})
const { ctrl_s } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 's' && e.type === 'keydown')
        e.preventDefault()
    },
})

whenever(ctrl_o, () => onSearchSale())
whenever(ctrl_s, () => onSubmit())

// sale.vue.$onKeyStroke('o', (e) => {
//     if(sale.dialogActiveState==false && e.ctrlKey==true){
//       e.preventDefault()
//       onSearchSale();
//     }
// })




const setting = JSON.parse(localStorage.getItem("setting"))
async function onSearchSale(){
    sale.dialogActiveState=true;
    let msg = $t('msg.please save or submit your current order first',[(setting.table_groups && setting.table_groups.length > 0 ? $t( 'Submit') : $t('Save'))]);
    const isOrdered = sale.isOrdered(msg)    
    if(isOrdered == false) {
        const result = await searchSaleDialog({ })
        sale.dialogActiveState=false;
        if(result != false){ 
            router.push({
                name: "AddSale", params: {
                    name: result.name
                }
            });

            sale.LoadSaleData(result.name)
        }
    }
}

async function onSubmit() {
  if (!sale.isBillRequested()) {
    const action = sale.action;
    const message = sale.message;
    const sale_status = sale.sale.sale_status; 
    sale.action = "submit_order";
    sale.message = $t("Submit order successfully");
    sale.sale.sale_status = "Submitted"; 
    await sale.onSubmit().then((doc) => {
      product.onClearKeyword(); 
      if (doc) {
        if (onRedirectSaleType()) {
          if (tableLayout.table_groups.length > 0) {
            sale.sale = {};
            router.push({ name: 'TableLayout' })
          }
          else {
            sale.newSale()
            router.push({ name: "AddSale" });
            sale.tableSaleListResource.fetch();
          }
        }
      }else{ 
        sale.action =  action;
        sale.message = message;
        sale.sale.sale_status =  sale_status;
      }
    });
  }
}



async function onPayment() {
  sale.dialogActiveState=true
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
  sale.dialogActiveState=false
  if (result) {
    product.onClearKeyword()
    sale.newSale();
    if (onRedirectSaleType()) {
      if (sale.setting.table_groups.length > 0) {
        router.push({ name: "TableLayout" });
      } else {
        sale.tableSaleListResource.fetch();
        router.push({ name: "AddSale" });
      }
    }
  }
}

function onRedirectSaleType() {
  const redirect_sale_type = localStorage.getItem("redirect_sale_type") || null
  if (redirect_sale_type) {
    router.push({ name: 'AddSaleNoTable', params: { sale_type: redirect_sale_type } })
    return false
  }
  return true
}
</script>
<style lang="">

</style>