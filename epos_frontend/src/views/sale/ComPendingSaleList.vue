<template>
  <v-row v-if="data" align="start" no-gutters>
    <v-card class="ma-4" v-for="(s, index) in data" :key="index">
      <v-toolbar height="55">
        <v-toolbar-title class="text">
          #: {{ s.name }} - <Timeago   :long="long" :datetime="s.creation"  /> 
        </v-toolbar-title>
        <template v-slot:append>
          <v-chip class="ma-2" :color="s.sale_status_color" text-color="white">
            {{ s.sale_status }}
          </v-chip>
        </template>
      </v-toolbar>
      <v-card-title>


      </v-card-title>
      <v-card-text  class="pa-0">
        <v-list :lines="false"
          density="compact"
          class="pa-0"
          >
         <v-list-item title="Table #">
            <template v-slot:append>
              {{ s.tbl_number }} 
            </template>
          </v-list-item>
          <v-list-item title="Guest Cover" v-if="s.guest_cover">
            <template v-slot:append>
              {{ s.guest_cover }} 
            </template>
          </v-list-item>
          <v-list-item title="Customer Code" >
            <template v-slot:append>
              {{ s.customer }}
            </template>
          </v-list-item>
          
          <v-list-item title="Customer Name" >
            <template v-slot:append>
              {{ s.customer_name }}
            </template>
          </v-list-item>
          <v-list-item title="Total Qty">
            <template v-slot:append>
              {{ s.total_quantity }}
            </template>
          </v-list-item>
          <v-list-item title="Grand Total">
            <template v-slot:append>
              {{ s.grand_total }}
            </template>
          </v-list-item>

        </v-list>
         
      
      </v-card-text>
      <v-card-actions class="pt-0">
        <v-btn color="primary" @click="onViewSaleOrder(s.name)">
          View Sale Detail
        </v-btn>
        <v-btn color="success" @click="onOpenOrder(s.name)">
          Open Order
        </v-btn>


      </v-card-actions>
    </v-card>
  </v-row>
  <div v-else>
    No Pending order
  </div>
</template>
<script setup>
import { useRouter, defineProps } from "@/plugin"

import { Timeago } from 'vue2-timeago'
import { saleDetailDialog } from "@/utils/dialog";
const router = useRouter();
const props = defineProps({
  data: {
    type: Object
  }
})
function onOpenOrder(sale_id) {
  router.push({ name: "AddSale", params: { name: sale_id } });
}

async function onViewSaleOrder(sale_id){
  await saleDetailDialog( { name: sale_id });
}
</script>