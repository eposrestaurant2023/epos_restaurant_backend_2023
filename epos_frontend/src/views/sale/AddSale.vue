<template>
    <ComLoadingDialog v-if="sale.newSaleResource?.loading || (sale.saleResource!=null && sale.saleResource?.get.loading) ||   (sale.saleResource!=null && sale.saleResource?.setValue.loading)"/>
    <div class="h-full">
        <v-row class="h-full ma-0">
            <v-col sm="7" md="7" lg="8" class="pa-0">
                <ComMenu :background-image="gv.setting.pos_sale_order_background_image"/>
            </v-col>
            <v-col sm="5" md="5" lg="4" class="h-100 py-0">
                <div class="h-100 grid" style="grid-template-rows: max-content;">
                        <div class="mb-2 ">
                            <ComSaleInformation/>
                            <ComSelectCustomer/>
                        </div>
                        <div class="overflow-auto"> 
                            <ComPlaceholder :is-not-empty="sale.getSaleProducts().length > 0" icon="mdi-cart-outline" text="Empty Product">
                                <div> 
                                    <v-list>
                                        <v-list-item v-for="sp,index in sale.getSaleProducts()" :key="index" @click="sale.onSelectSaleProduct(sp)"
                                            class="!border-t !border-gray-300 !mb-1"
                                            :class="sp.selected ? '!bg-gray-100' : ''"
                                            >
                                            <template v-slot:prepend>
                                                <v-avatar v-if="sp.product_photo">
                                                    <v-img :src="sp.product_photo"></v-img>
                                                </v-avatar>
                                                <avatar v-else :name="sp.product_name" class="mr-4" size="40"></avatar>
                                            </template>
                                            <template v-slot:default>
                                                <div class="text-sm">
                                                    <div class="flex justify-between">
                                                        <div>
                                                            <div>{{ sp.product_name }} {{ sp.sale_product_status }} <v-chip size="x-small" color="error" variant="outlined" v-if="sp.portion">{{ sp.portion }}</v-chip> <v-chip v-if="sp.is_free" size="x-small" color="success" variant="outlined">Free</v-chip></div>
                                                            <div>
                                                                {{ sp.quantity }} x <CurrencyFormat :value="sp.price"/>
                                                            </div>
                                                            <div class="text-xs pt-1">
                                                            <div v-if="sp.modifiers">
                                                                    <span>{{ sp.modifiers }} (<CurrencyFormat :value="sp.modifiers_price*sp.quantity" />)</span>
                                                                </div>
                                                                <div class="text-red-500">
                                                                    Discount: <span>(15%) = 100$</span>
                                                                </div>
                                                                <v-chip color="orange" size="x-small" v-if="sp.seat_number">Seat# {{ sp.seat_number }}</v-chip>
                                                                <div class="text-gray-500" v-if="sp.note">
                                                                    Note: <span>{{sp.note}}</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="text-right w-36">
                                                            <div class="text-lg"><CurrencyFormat :value="sp.amount"/></div>
                                                            <ComQuantityInput :sale-product="sp" />
                                                        </div>
                                                    </div>
                                                    
                                                    <div v-if="sp.selected" class="my-2 -mx-1 flex border-t border-gray-600 pt-2" >
                                                        <v-chip color="teal" class="mx-1 grow text-center justify-center" size="small" @click="sale.onChangePrice(sp)">Price</v-chip>
                                                        <v-chip color="teal" class="mx-1 grow text-center justify-center" size="small" @click="sale.onChangeQuantity(sp)">Qty</v-chip>
                                                        <v-chip color="teal" class="mx-1 grow text-center justify-center" size="small" @click="onEditSaleProduct(sp)">Edit</v-chip>
                                                        <ComSaleProductButtonMore :sale-product="sp"/>
                                                    </div>
                                                
                                                </div>
                                            </template>
                                        </v-list-item>
                                    </v-list>
                                </div>
                            </ComPlaceholder>
                        </div>
                    <div class="mt-auto">
                        <ComAddSaleSummary/>
                    </div>
                </div>
            </v-col>
        </v-row>
    </div>
</template>
<script setup>
import { useStore, inject ,useRoute } from '@/plugin';
import ComMenu from './components/ComMenu.vue';
import ComSelectCustomer from './components/ComSelectCustomer.vue';
import ComAddSaleSummary from './components/ComAddSaleSummary.vue';
import ComQuantityInput from '../../components/form/ComQuantityInput.vue';
import ComSaleInformation from '@/views/sale/components/ComSaleInformation.vue';
import { createToaster } from "@meforma/vue-toaster";
import ComSaleProductButtonMore from './components/ComSaleProductButtonMore.vue';
import ComLoadingDialog from '../../components/ComLoadingDialog.vue';

const store = useStore()

const sale = inject("$sale")
const gv = inject("$gv")
const product = inject("$product")
const toast = createToaster({position:"top"})

const route = useRoute()
//check if new sale
if (route.params.name=="") {
    
    
} else {
    sale.LoadSaleData(route.params.name);
    
}

 
 

if(!store.state.sale.posMenu){
    store.dispatch('sale/onGetPosMenu')
    
    
}
function onEditSaleProduct(sp){
    if (!sale.isBillRequested()){ 
    product.setSelectedProductByMenuID(sp.menu_product_name);
 
    product.setModifierSelection(sp)
 
    sale.OnEditSaleProduct(sp)
    }
}

</script>