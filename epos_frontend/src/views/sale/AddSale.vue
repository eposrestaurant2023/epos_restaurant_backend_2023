<template>
    <div class="h-full">
        <v-row class="h-full ma-0">
            <v-col sm="7" md="7" lg="8" class="pa-0">
                <div class="h-full bg-cover bg-no-repeat bg-center" v-bind:style="{ 'background-image': 'url(' + setting.pos_sale_order_background_image + ')' }">
                    <ComMenu/>
                </div>
            </v-col>
            <v-col sm="5" md="5" lg="4">
                <div class="mb-2">
                  
                    <ComSelectCustomer/>
                </div>
                <div class="overflow-hidden">
                    <div> 
                        <v-list> 
                            <v-list-item v-for="sp in sale.getSaleProducts()" :key="sp.product_code" @click="sale.onSelectSaleProduct(sp)"
                                class="!border-t !border-gray-300 !mb-1"
                                :class="sp.selected ? '!bg-gray-100' : ''"
                                :prepend-avatar="sp.product_photo ? sp.product_photo : 'https://i1.wp.com/www.slntechnologies.com/wp-content/uploads/2017/08/ef3-placeholder-image.jpg?ssl=1'">
                                <template v-slot:default>
                                    <div class="text-sm">
                                        <div class="flex justify-between">
                                            <div>
                                                <div>{{ sp.product_name }} <v-chip size="x-small" color="error" variant="outlined" v-if="sp.portion">{{ sp.portion }}</v-chip> <v-chip v-if="sp.free" size="x-small" color="success" variant="outlined">Free</v-chip></div>
                                                <div>
                                                    {{ sp.quantity }} x <CurrencyFormat :value="sp.price"/>
                                                </div>
                                                <div class="text-xs pt-1">
                                                   <div v-if="sp.modifiers">
                                                        <!-- <span class="text-blue-600 mr-1">{{sp.portion}}</span> -->
                                                        <span>{{ sp.modifiers }} (<CurrencyFormat :value="sp.modifiers_price*sp.quantity" />)</span>
                                                    </div>
                                                    <div class="text-red-500">
                                                        Discount: <span>(15%) = 100$</span>
                                                    </div>
                                                    <div class="text-gray-500" v-if="sp.note">
                                                        Note: <span>{{sp.note}}</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="w-36">
                                                <div class="text-right text-lg"><CurrencyFormat :value="sp.amount"/></div>
                                                <ComQuantityInput :sale-product="sp" />
                                            </div>
                                        </div>
                                        
                                        <div v-if="sp.selected" class="my-2 -mx-1 flex border-t border-gray-600 pt-2" >
                                            <v-chip color="teal" class="mx-1 grow text-center justify-center" size="small" @click="sale.onChangePrice(sp)">Price</v-chip>
                                            <v-chip color="teal" class="mx-1 grow text-center justify-center" size="small" @click="sale.onChangeQuantity(sp)">Qty</v-chip>
                                            <v-chip color="teal" class="mx-1 grow text-center justify-center" size="small" @click="sale.OnEditSaleProduct(sp)">Edit</v-chip>
                                            <ComSaleButtonMore :sale-product="sp"/>
                                        </div>
                                    </div>
                                </template>
                            </v-list-item>
                            
                        </v-list>
                        {{ sale }}
                        <!-- <ComAddSaleSummary/> -->
                       
                    </div>
                </div>
            </v-col>
        </v-row>
        <div class="pa-4">
            <button @click="sale.submit()">Save</button>
        </div>
    </div>
</template>
<script setup>
import { computed, ref,createResource,useStore, mapGetters, mapState,inject } from '@/plugin';
import ComMenu from './components/ComMenu.vue';
import ComSaleButtonMore from './components/ComSaleButtonMore.vue';
import ComSelectCustomer from './components/ComSelectCustomer.vue';
import ComAddSaleSummary from './components/ComAddSaleSummary.vue';
import ComQuantityInput from '../../components/form/ComQuantityInput.vue';

const store = useStore()
const setting = store.state.setting;
const sale = inject("$sale")

 
//check if new sale or edit sale
if (1==1){ //case new sale
    sale.newSale();
}



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