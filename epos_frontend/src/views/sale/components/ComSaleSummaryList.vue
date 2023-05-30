<template lang="">
  <div class="px-2">
    <template v-if="(sale.sale.total_discount + sale.sale.total_tax) > 0">
      <div class="flex justify-between my-1">
        <div>
          {{$t('Sub Total')}}
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.sub_total" />
        </div>
      </div>
      
      <div class="flex justify-between my-1" v-if="sale.sale.sale_discount>0">
        <div>
          {{$t('Discountable Amount')}}
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.sale_discountable_amount" />
        </div>
      </div>

      <div class="flex justify-between mb-1" v-if="sale.sale.product_discount > 0">
        <div>{{$t('Items Discount')}}</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.product_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.sale_discount > 0">
        <div>{{$t('Sale Discount')}}
          <span v-if="sale.sale.discount && sale.sale.discount_type == 'Percent'"> - {{ sale.sale.discount }}%</span>
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.sale_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.sale_discount > 0 && sale.sale.product_discount > 0">
        <div>{{$t('Total Discount')}}</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.total_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_1_amount > 0">
         
          <div v-if="sale.sale.percentage_of_price_to_calculate_tax_1==100">{{ setting.tax_1_name }}{{sale.sale.tax_1_rate>0?"("+sale.sale.tax_1_rate+"%)":""}}</div>
          <div v-else>{{ setting.tax_1_name }} ({{sale.sale.tax_1_rate+"%"}} {{$t('of')}} {{sale.sale.percentage_of_price_to_calculate_tax_1+"% "+$t('Revenue')}})</div>
     
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_1_amount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_2_amount > 0">        
        <div v-if="sale.sale.percentage_of_price_to_calculate_tax_2==100">{{ setting.tax_2_name }} {{sale.sale.tax_2_rate>0?"("+sale.sale.tax_2_rate+"%)":""}}</div>
        <div v-else>{{ setting.tax_2_name }} ({{sale.sale.tax_2_rate+"%"}} {{$t('of')}} {{sale.sale.percentage_of_price_to_calculate_tax_2+"% "+$t('Revenue')}})</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_2_amount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_3_amount > 0">
        <div v-if="sale.sale.percentage_of_price_to_calculate_tax_3==100">{{ setting.tax_3_name }} {{sale.sale.tax_3_rate>0?"("+sale.sale.tax_3_rate+"%)":""}}</div>
          <div v-else>{{ setting.tax_3_name }} ({{sale.sale.tax_3_rate+"%"}} {{$t('of')}} {{sale.sale.percentage_of_price_to_calculate_tax_3+"% "+$t('Revenue')}})</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_3_amount" />
        </div>
      </div>
      <div class="flex justify-between" v-if="sale.sale.total_tax > 0">
        <div>{{$t('Total Tax')}}</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.total_tax" />
        </div>
      </div>
    </template>
 
      <div class="flex justify-between my-1" v-if="sale.sale.note">
        <div>
          {{$t('Note')}}
        </div>
        <div>
          {{sale.sale.note}}
        </div>
      </div>
      <div class="flex justify-between my-1" v-if="(sale.sale.commission_amount || 0) >0">
        <div>
          {{$t('Commission')}}
        </div>
        <div>
          <CurrencyFormat :value="sale.sale.commission_amount" />
        </div>
      </div>
      <div class="flex justify-between my-1" v-if="sale.sale.reference_number">
        <div>
          {{$t('Reference')}} #
        </div>
        <div>
          {{sale.sale.reference_number}}
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