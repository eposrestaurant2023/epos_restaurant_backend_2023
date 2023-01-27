<template>
   
<div>
    Total Quantity: {{ sale.total_quantity }}
</div>
<div>
    Sub Total: <CurrencyFormat :value="sale.sub_total"/>
</div>
<div>
    Product Discount: <CurrencyFormat :value="sale.product_discount"/>
</div>
<div>
  Sale Discount: <CurrencyFormat :value="sale.sale_discount"/>
</div>
<div>
  Total Discount: <CurrencyFormat :value="sale.total_discount"/>
</div>
<div>
  {{ setting.tax_1_name }} : <CurrencyFormat :value="sale.tax_1_amount"/>
</div>
<div>
  {{ setting.tax_2_name }} : <CurrencyFormat :value="sale.tax_2_amount"/>
</div>
<div>
  {{ setting.tax_3_name }} : <CurrencyFormat :value="sale.tax_3_amount"/>
</div>
<div>
  Total Tax : <CurrencyFormat :value="sale.total_tax"/>
</div>
<div>
  Grand Total : <CurrencyFormat :value="sale.grand_total"/>
</div>



<div class="mt-4"> {{ sale }}</div>

</template>
<script setup>
import { useStore,computed } from '@/plugin';
const store = useStore();
const setting = store.state.setting;
 
const sale = computed(() => {
  return store.state.sale.sale
})

</script>