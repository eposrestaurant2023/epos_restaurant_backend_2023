<template>
  <div class="-mx-3">
    <div>
      Sub Total:
      <CurrencyFormat :value="sale.sub_total" />
    </div>
    <div>
      Product Discount:
      <CurrencyFormat :value="sale.product_discount" />
    </div>
    <div>
      Sale Discount:
      <CurrencyFormat :value="sale.sale_discount" />
    </div>
    <div>
      Total Discount:
      <CurrencyFormat :value="sale.total_discount" />
    </div>
    <div>
      {{ setting.tax_1_name }} : <CurrencyFormat :value="sale.tax_1_amount" />
    </div>
    <div>
      {{ setting.tax_2_name }} : <CurrencyFormat :value="sale.tax_2_amount" />
    </div>
    <div>
      {{ setting.tax_3_name }} : <CurrencyFormat :value="sale.tax_3_amount" />
    </div>
    <div>
      Total Tax : <CurrencyFormat :value="sale.total_tax" />
    </div>
    <v-btn :loading="sale.newSaleResource.loading" @click="onSubmit">Submit Order</v-btn>
    <div>
      <div class="cursor-pointer bg-green-600 text-white p-3 hover:bg-green-700" @click="onSubmit()">
          <div class="flex justify-between mb-2">
            <div>Grand Total</div>
            <div><CurrencyFormat :value="sale.grand_total" /></div>
          </div>
          <div class="flex justify-between">
            <div>Total Quantity</div>
            <div><CurrencyFormat :value="sale.total_quantity" /></div>
          </div>
        </div>
    </div>
  </div>
  

</template>
<script setup>
import { inject,useRouter } from '@/plugin';
const router = useRouter()
const sale = inject("$sale")
const gv = inject("$gv")
const setting = gv.setting;
 

async function onSubmit(){
  await sale.onSubmit();
  router.push({name:"TableLayout"});
}
function onNote(){
  alert('submit')
}
function onSave(){
  alert('submit')
}
function onDiscount(){
  alert('submit')
}

 

</script>