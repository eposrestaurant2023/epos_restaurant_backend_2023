<template lang="">
    <div class="px-2">
      <template v-if="(sale.total_discount + sale.total_tax) > 0">
        <div class="flex justify-between my-1">
          <div>
            Sub Total
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.sub_total" />
          </div>
        </div>
        
        <div class="flex justify-between my-1" v-if="sale.sale_discount>0">
          <div>
            Discountable Amount
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.sale_discountable_amount" />
          </div>
        </div>
  
        <div class="flex justify-between mb-1" v-if="sale.product_discount > 0">
          <div>Product Discount</div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.product_discount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="sale.sale_discount > 0">
          <div>Sale Discount
            <span v-if="sale.discount && sale.discount_type == 'Percent'"> - {{ sale.discount }}%</span>
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.sale_discount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="sale.sale_discount > 0 && sale.product_discount > 0">
          <div>Total Discount</div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.total_discount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="sale.tax_1_amount > 0">
          <div>
            {{ setting.tax_1_name }}
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.tax_1_amount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="sale.tax_2_amount > 0">
          <div>{{ setting.tax_2_name }}</div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.tax_2_amount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="sale.tax_3_amount > 0">
          <div>
            {{ setting.tax_3_name }}
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.tax_3_amount" />
          </div>
        </div>
        <div class="flex justify-between" v-if="sale.total_tax > 0">
          <div>Total Tax</div>
          <div class="font-bold">
            <CurrencyFormat :value="sale.total_tax" />
          </div>
        </div>
      </template>
   
        <div class="flex justify-between my-1" v-if="sale.note">
          <div>
            Note
          </div>
          <div>
            {{sale.note}}
          </div>
        </div>
        <div class="flex justify-between my-1" v-if="(sale.commission_amount || 0) >0">
          <div>
            Commission
          </div>
          <div>
            <CurrencyFormat :value="sale.commission_amount" />            
          </div>
        </div>
   
    </div>
  </template>
  <script setup>
        import { inject } from 'vue' 
        const gv = inject("$gv")
        const setting = gv.setting;
        const props = defineProps({
            sale: Object
        })
  </script> 