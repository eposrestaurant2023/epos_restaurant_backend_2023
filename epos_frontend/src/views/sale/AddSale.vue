<template>
    <div class="pa-4">
        <v-row>
            <v-col sm="16">
                <ComMenu/>
            </v-col>
            <v-col sm="16">
                <ComSelectedCustomer/>
                <hr/>
                <table>
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th>Unit</th>
                            <th>Price</th>
                            <th>Amount</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(p, index) in sale.sale_products" :key="index">
                            <td>{{ p.product_code }} - {{ p.product_name }}</td>
                            <td><input type="text" v-model="p.unit"/></td>
                            <td><input type="number" v-model="p.quantity"/></td>
                            <td><input type="number" v-model="p.price"/></td>
                            <td>{{ p.amount }}</td>
                        </tr>
                    </tbody>
                </table>
            </v-col>
        </v-row>
        <div class="pa-4">
            <button @click="onSave()">Save</button>
        </div>
    </div>
</template>
<script setup>
import { computed } from 'vue';
import ComProductSearch from './components/ComProductSearch.vue';
import ComSelectedCustomer from './components/ComSelectedCustomer.vue';
import ComMenu from './components/ComMenu.vue';
import { createResource } from '@/resource.js'
import { useStore } from 'vuex'
const store = useStore()
const sale = computed(() => {
  return store.state.sale.sale
})

let todos = createResource({
  url: 'frappe.client.get',
  params: {
    doctype: 'Sale',
    name: 'SO2022-0005'
  },
  auto:true,
  onError(x) {
   
  },

})

</script>