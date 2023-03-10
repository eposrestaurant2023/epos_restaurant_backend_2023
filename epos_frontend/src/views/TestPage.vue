<template>
  <div class="pa-8">
    {{ data }}
    <hr/>
     {{ data.posting_date }} - {{ data.phone_number }} - {{ data.total_quantity }}
     <ul>
      <li v-for="(p, index) in data.sale_products" :key="index">{{ p.product_name }} - {{ p.price }}</li>
     </ul>

     <v-btn @click="AddProduct()">Add Order</v-btn>
  </div>
</template>
<script setup> 
import { ref } from 'vue';
import Enumerable from 'linq';

const data = ref({})

 function AddProduct(){
  data.value.posting_date = "2023-01-01"
  data.value.name= "Dara vatety"
  data.value.phone_number = "0125487696"
  data.value.sale_products = [
    {
      product_name:"Test Product 001",
      price: 1.25 
    }
  ]
  data.value.sale_products.push({product_name:"New product",price:15.75});
 
 }
</script>