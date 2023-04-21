<template>
    <ComModal fullscreen>
      <template #title>
        Scan Membership Card Number
      </template>
      <template #content>
       
     
        <ul>
          <li @click="onSelectProduct(sp)"  v-for="(sp, index) in sale.sale.sale_products" :key="index">{{ sp.product_name }} X {{ sp.quantity }} ({{ sp.selected_quantity }})</li>
        </ul>

        <div v-for="(b, index) in bill_list" :key="index">
          <v-btn @click="AddSelectedProductToBill(b)">Add Selected Product To This Bill</v-btn>
        <h1>{{b.bill_number}}</h1>
        <ul>
          <li v-for="(sp, index) in b.sale_products" :key="index">{{ sp.product_name }} x {{ sp.quantity }}</li>
        </ul>
        </div>
        <v-btn @click="onAddNewBill">Add New Bill</v-btn>
      </template> 
    </ComModal>
  </template>
<script setup>
   import { inject,ref } from '@/plugin';
   const sale = inject("$sale")
   const bill_list = ref([])


   function onSelectProduct(sp){
    if((sp.selected_quantity || 0) == sp.quantity){
      sp.selected_quantity = 0;
    }
    else {
      sp.selected_quantity = (sp.selected_quantity || 0) + 1;
    }
   }

   function onAddNewBill(){
    bill_list.value.push({
      bill_number : bill_list.value.length + 1,
      sale_products:[]
    })
   }
   function AddSelectedProductToBill(b){
    const selected = sale.sale.sale_products.filter((r)=>(r.selected_quantity || 0)>0 );
    selected.forEach(x => {
      b.sale_products.push({
        product_name:x.product_name,
        quantity:x.selected_quantity
      })
    });
   }
</script>