<template>
    <ComLoadingDialog v-if="sale.newSaleResource?.loading || (sale.saleResource!=null && sale.saleResource?.get.loading) ||   (sale.saleResource!=null && sale.saleResource?.setValue.loading)"/>
    <div style="height: calc(100vh - 64px)" id="tst">
        <ComProductSearch v-if="mobile" />
        {{ sale.auditTrailLogs }}
        <v-row class="h-full ma-0">
            <v-col sm="7" md="7" lg="8" class="pa-0 h-full">
                <ComMenu :background-image="gv.setting.pos_sale_order_background_image"/>
            </v-col>
            <v-col sm="5" md="5" lg="4" class="h-full pa-0">
                <div class="h-full grid px-1" style="grid-template-rows: max-content;">
                        <div class="mb-1">
                            <ComSaleInformation/>
                            <ComSelectCustomer/>
                        </div>
                        <div class="overflow-auto"> 
                         
                            <ComPlaceholder :is-not-empty="sale.getSaleProducts().length > 0" icon="mdi-cart-outline" text="Empty Product">
                      
                                <div v-for="(g, index) in sale.getSaleProductGroupByKey()" :key="index">
                                    <div class="bg-red-700 text-white flex items-center justify-between" style="font-size: 10px; padding: 2px;">
                                        <div><v-icon icon="mdi-clock" size="small" class="mr-1"></v-icon>{{  moment(g.order_time).format('HH:mm:ss') }}</div>
                                        <div><v-icon icon="mdi-account-outline" size="small" class="mr-1"></v-icon>{{g.order_by }}</div>
                                    </div>
                                    <v-list class="!p-0">
                                        <v-list-item v-for="sp,index in sale.getSaleProducts(g)" :key="index" @click="sale.onSelectSaleProduct(sp)"
                                            class="!border-t !border-gray-300 !mb-0 !p-2 item-list"
                                            :class="{'selected' : sp.selected, 'submitted':sp.sale_product_status == 'Submitted'}">
                                            <template v-slot:prepend>
                                                <v-avatar v-if="sp.product_photo">
                                                    <v-img :src="sp.product_photo"></v-img>
                                                </v-avatar>
                                                <avatar v-else :name="sp.product_name" class="mr-4" size="40"></avatar>
                                            </template>
                                            <template v-slot:default>
                                            
                                                <div class="text-sm">
                                                    <div class="flex">
                                                        <div class="grow">
                                                            <div>  {{ sp.product_name }} {{ sp.sale_product_status }} <v-chip size="x-small" color="error" variant="outlined" v-if="sp.portion">{{ sp.portion }}</v-chip> <v-chip v-if="sp.is_free" size="x-small" color="success" variant="outlined">Free</v-chip></div>
                                                            <div>
                                                                {{ sp.quantity }} x <CurrencyFormat :value="sp.price"/>
                                                            </div>
                                                            <div class="text-xs pt-1">
                                                                <div v-if="sp.modifiers">
                                                                    <span>{{ sp.modifiers }} (<CurrencyFormat :value="sp.modifiers_price*sp.quantity" />)</span>
                                                                </div>
                                                                <div class="text-red-500" v-if="sp.discount > 0">
                                                                    Discount : 
                                                                    <span v-if="sp.discount_type == 'Percent'">{{ sp.discount }}%</span>
                                                                    <CurrencyFormat v-else :value="parseFloat(sp.discount)" />
                                                                </div>
                                                                <v-chip color="blue" size="x-small" v-if="sp.seat_number">Seat# {{ sp.seat_number }}</v-chip>
                                                                <div class="text-gray-500" v-if="sp.note">
                                                                    Note: <span>{{sp.note}}</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="flex-none text-right w-36">
                                                            <div class="text-lg"><CurrencyFormat :value="sp.amount"/></div>
                                                            <ComQuantityInput :sale-product="sp" />
                                                        </div>
                                                    </div>
                                                    
                                                    <div v-if="sp.selected" class="-mx-1 flex pt-1" >
                                                        <v-chip color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small" @click="onChangePrice(sp)">Price</v-chip>
                                                        <v-chip :disabled="sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status=='Submitted'" color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small" @click="sale.onChangeQuantity(sp)">Qty</v-chip>
                                                        <v-chip :disabled="sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status=='Submitted'" color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small" @click="onEditSaleProduct(sp)">Edit</v-chip>
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
import ComProductSearch from './components/ComProductSearch.vue';
import moment from '@/utils/moment.js';
import { useDisplay } from 'vuetify'

const { mobile,name,platform } = useDisplay()

const store = useStore()

const sale = inject("$sale")
const gv = inject("$gv")
const product = inject("$product")
const toaster = createToaster({position:"top"})

const route = useRoute()
sale.orderTime = null;
sale.deletedSaleProducts = []
if(sale.orderBy==null){
    sale.orderBy =JSON.parse(localStorage.getItem("current_user")).full_name;
}


sale.orderTime = "";


if(product.posMenuResource.data?.length==0){

    product.loadPOSMenu();
}

//check if new sale
if (route.params.name=="") {
    //
} else {
    sale.LoadSaleData(route.params.name);
    
}

if(!store.state.sale.posMenu){
    store.dispatch('sale/onGetPosMenu')
    
    
}
function onEditSaleProduct(sp) {
    if (!sale.isBillRequested()) {
        if (sp.sale_product_status == "New" || sale.setting.pos_setting.allow_change_quantity_after_submit == 1) {
            product.setSelectedProductByMenuID(sp.menu_product_name);

            product.setModifierSelection(sp)

            sale.OnEditSaleProduct(sp)
        } else {
            toaster.warning("Submitted order is not allow to edit.");
        }

    }
}

function onChangePrice(sp){
    if (!sale.isBillRequested()) {
        gv.authorize("change_item_price_required_password", "change_item_price", "change_item_price_required_note", "Change Item Price Note", sp.product_code).then((v) => {
            if (v) {
                sp.change_price_note = v.note
                sale.onChangePrice(sp);
            }
        });

    }
}
document.onkeydown = function (e) {
  if (e.keyCode === 116) {
    return false;
  }
}; 
</script>
<style scoped>
.selected,.item-list:hover {
    background-color: #ffebcc !important;
}
.item-list.submitted::before {
    content: '';
}
</style>