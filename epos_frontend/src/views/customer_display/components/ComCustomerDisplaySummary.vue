<template lang="">
    <div class="px-2">
      <template v-if="(data.total_discount + data.total_tax) > 0">
        <div class="flex justify-between my-1">
          <div>
           {{$t('Sub Total')}}
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="data.sub_total" />
          </div>
        </div>
        
        <div class="flex justify-between my-1" v-if="data.sale_discount>0">
          <div>
            {{$t('Discountable Amount')}}
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="data.sale_discountable_amount" />
          </div>
        </div>
  
        <div class="flex justify-between mb-1" v-if="data.product_discount > 0">
          <div>{{$t('Items Discount')}}</div>
          <div class="font-bold">
            <CurrencyFormat :value="data.product_discount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="data.sale_discount > 0">
          <div>{{$t('Sale Discount')}}
            <span v-if="data.discount && data.discount_type == 'Percent'"> - {{ data.discount }}%</span>
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="data.sale_discount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="data.sale_discount > 0 && data.product_discount > 0">
          <div>{{$t('Total Discount')}}</div>
          <div class="font-bold">
            <CurrencyFormat :value="data.total_discount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="data.tax_1_amount > 0">
          <div>
            {{ setting.tax_1_name }}
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="data.tax_1_amount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="data.tax_2_amount > 0">
          <div>{{ setting.tax_2_name }}</div>
          <div class="font-bold">
            <CurrencyFormat :value="data.tax_2_amount" />
          </div>
        </div>
        <div class="flex justify-between mb-1" v-if="data.tax_3_amount > 0">
          <div>
            {{ setting.tax_3_name }}
          </div>
          <div class="font-bold">
            <CurrencyFormat :value="data.tax_3_amount" />
          </div>
        </div>
        <div class="flex justify-between" v-if="data.total_tax > 0">
          <div>{{$t('Total Tax')}}</div>
          <div class="font-bold">
            <CurrencyFormat :value="data.total_tax" />
          </div>
        </div>
      </template>
   
        <div class="flex justify-between my-1" v-if="data.note">
          <div>
            {{$t('Note')}}
          </div>
          <div>
            {{data.note}}
          </div>
        </div>
        <div class="flex justify-between my-1" v-if="(data.commission_amount || 0) >0">
          <div>
            {{$t('Commission')}}
          </div>
          <div>
            <CurrencyFormat :value="data.commission_amount" />
            
          </div>
        </div>
   
    </div>
  </template>
<script setup>
import { inject } from 'vue'
const props = defineProps({
    data: Object
})
const gv = inject("$gv")
const setting = gv.setting;
</script> 