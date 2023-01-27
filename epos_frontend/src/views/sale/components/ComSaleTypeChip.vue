<template>
 
    <v-menu>
      <template v-slot:activator="{ props }">
        <v-btn
        :color="saleType?.color"
          v-bind="props"
        >
         {{ sale.sale.sale_type  }}
        </v-btn>
      </template>
      <v-list>
        <v-list-item
       v-for="(item, index) in saleTypeResource.data" :key="index"
        >
          <v-list-item-title @click="onChangeSaleType(item)">{{ item.name }}</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  
 
</template>
<script setup>
import {createResource,inject,computed} from "@/plugin";
const sale = inject("$sale")
let saleTypeResource = createResource({
    url:"frappe.client.get_list",
    params:{
        doctype:"Sale Type",
        fields:["name","color","is_order_use_table"],
       
    },
    cache:"sale_type",
    auto:true
})

const saleType = computed(()=>{
    if(saleTypeResource.data){
        return saleTypeResource.data.find(r=>r.name==sale.sale.sale_type);
    }
    return {"name":sale.sale.sale_type}
})


function onChangeSaleType(s){
sale.sale.sale_type = s.name;
}

</script>