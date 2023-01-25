<template>
    <div class="h-full">
        <v-row class="h-full ma-0">
            <v-col sm="8" class="pa-0">
                <div class="h-full bg-cover bg-no-repeat bg-center" v-bind:style="{ 'background-image': 'url(' + setting.pos_sale_order_background_image + ')' }">
                    <ComMenu/>
                </div>
            </v-col>
            <v-col sm="4">
                <div>
                    xxx
                </div>
                <div class="-mx-4">
                    <ComAutoComplete v-model="sale.customer" doctype="Customer" variant="outlined"/>
                </div>
                <div class="overflow-hidden">
                    <div class="-mx-4">
                        <v-list> 
                            <v-list-item
                                :class="{ 'on-hover': hover }"
                                prepend-avatar="https://cdn.vuetifyjs.com/images/john.png"
                                title="John Leider"
                                subtitle="john@google.com"
                                >
                                <template v-slot:default>
                                    dddd
                                </template>
                                <template v-slot:append>
                                <v-btn
                                    size="small"
                                    variant="text"
                                    color="error"
                                    icon="mdi-delete"
                                    @click="onRemove()"
                                ></v-btn>
                                </template>
                            </v-list-item>
                            <v-list-item
                                prepend-avatar="https://cdn.vuetifyjs.com/images/john.png"
                                title="John Leider"
                                subtitle="john@google.com"
                                >
                                <template v-slot:default>
                                    dddd
                                </template>
                                <template v-slot:append>
                                    <v-btn
                                        size="small"
                                        variant="text"
                                        color="error"
                                        icon="mdi-delete"
                                        @click="onRemove()"
                                    ></v-btn>
                                </template>
                            </v-list-item>
                        </v-list>
                        <!-- <table>
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
                        </table> -->
                    </div>
                </div>
            </v-col>
        </v-row>
        <div class="pa-4">
            <button @click="onSave()">Save</button>
        </div>
    </div>
</template>
<script setup>
import { computed, ref } from 'vue';
import ComMenu from './components/ComMenu.vue';
import { createResource } from '@/resource.js'
import { useStore } from 'vuex'

const store = useStore()
const sale = computed(() => {
  return store.state.sale.sale
})


const setting =JSON.parse( localStorage.getItem("setting"));
let model = ref(1)

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
if(!store.state.sale.posMenu){
    store.dispatch('sale/onGetPosMenu')
    
    
}

</script>