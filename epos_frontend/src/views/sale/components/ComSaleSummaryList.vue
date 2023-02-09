<template lang="">
    <div class="px-2" v-if="(sale.sale.total_discount + sale.sale.total_tax) > 0">
      <div class="flex justify-between my-1">
        <div>
          Sub Total
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.sub_total" />
        </div>
      </div>
      
      <div class="flex justify-between my-1" >
        <div>
          Discountable Amount
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.sale_discountable_amount" />
        </div>
      </div>

      <div class="flex justify-between mb-1" v-if="sale.sale.product_discount > 0">
        <div>Product Discount</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.product_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.sale_discount > 0">
        <div>Sale Discount
          <span v-if="sale.sale.discount && sale.sale.discount_type == 'Percent'"> - {{ sale.sale.discount }}%</span>
        </div>
        <div class="font-bold">

          <CurrencyFormat :value="sale.sale.sale_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.sale_discount > 0 && sale.sale.product_discount > 0">
        <div>Total Discount</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.total_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_1_amount > 0">
        <div>
          {{ setting.tax_1_name }}
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_1_amount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_2_amount > 0">
        <div>{{ setting.tax_2_name }}</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_2_amount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_3_amount > 0">
        <div>
          {{ setting.tax_3_name }}
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_3_amount" />
        </div>
      </div>
      <div class="flex justify-between" v-if="sale.sale.total_tax > 0">
        <div>Total Tax</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.total_tax" />
        </div>
      </div>
    </div>
</template>
<script setup>
import { inject } from 'vue'
const sale = inject('$sale')
const gv = inject("$gv")
const setting = gv.setting;
</script> 