<template>
    <v-list class="!p-0">
        <v-list-item v-for="sp, index in sale.getSaleProducts(groupKey)" :key="index" @click="sale.onSelectSaleProduct(sp)"
            class="!border-t !border-gray-300 !mb-0 !p-2 item-list"
            :class="{ 'selected': sp.selected, 'submitted relative': sp.sale_product_status == 'Submitted' }">
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
                            <div> {{ sp.product_name }}<v-chip size="x-small"
                                    color="error" variant="outlined" v-if="sp.portion">{{ sp.portion }}</v-chip> <v-chip
                                    v-if="sp.is_free" size="x-small" color="success" variant="outlined">Free</v-chip>
                            </div>
                            <div>
                                {{ sp.quantity }} x <CurrencyFormat :value="sp.price" />
                            </div>
                            <div class="text-xs pt-1"> 
                                <div v-if="sp.modifiers">
                                    <span>{{ sp.modifiers }} (<CurrencyFormat :value="sp.modifiers_price * sp.quantity" />)
                                    </span>
                                </div>
                                <div class="text-red-500" v-if="sp.discount > 0">
                                    Discount :
                                    <span v-if="sp.discount_type == 'Percent'">{{
                                        sp.discount
                                    }}%</span>
                                    <CurrencyFormat v-else :value="parseFloat(sp.discount)" />
                                </div>
                                <v-chip color="blue" size="x-small" v-if="sp.seat_number">Seat# {{
                                    sp.seat_number
                                }}</v-chip>
                                <div class="text-gray-500" v-if="sp.note">
                                    Note: <span>{{ sp.note }}</span>
                                </div>
                            </div>
                        </div>
                        <div class="flex-none text-right w-36">
                            <div class="text-lg">
                                
                                <CurrencyFormat :value="sp.amount" />
                            </div>
                            <ComQuantityInput :sale-product="sp" />
                        </div>
                    </div>

                    <div v-if="sp.selected" class="-mx-1 flex pt-1">
                        <v-chip color="teal" class="mx-1 grow text-center justify-center" variant="elevated"
                            size="small" @click="onChangePrice(sp)">Price</v-chip>
                        <v-chip
                            :disabled="sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status == 'Submitted'"
                            color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small"
                            @click="sale.onChangeQuantity(sp)">Qty</v-chip>
                        <v-chip
                            :disabled="sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status == 'Submitted'"
                            color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small"
                            @click="onEditSaleProduct(sp)">Edit</v-chip>

                        
                        <v-chip
                            color="teal" 
                            class="mx-1 grow text-center justify-center" 
                            variant="elevated"
                             size="small"
                            @click="onReorder(sp)">Reorder</v-chip>
                        
                  
                        <ComSaleProductButtonMore :sale-product="sp" />
                    </div>
                </div>
            </template>
        </v-list-item>
    </v-list>
</template>
<script setup>
import { inject, defineProps,createToaster } from '@/plugin'
import ComSaleProductButtonMore from './ComSaleProductButtonMore.vue';
import ComQuantityInput from '../../../components/form/ComQuantityInput.vue';
import Enumerable from 'linq';
const sale = inject('$sale')
const product = inject('$product')
const gv = inject('$gv')
const toaster = createToaster({position: 'top'})

const props = defineProps({
    groupKey: Object
})

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

function onChangePrice(sp) {
    if (!sale.isBillRequested()) {
        gv.authorize("change_item_price_required_password", "change_item_price", "change_item_price_required_note", "Change Item Price Note", sp.product_code).then((v) => {
            if (v) {
                sp.change_price_note = v.note
                sale.onChangePrice(sp);
            }
        });

    }
}

function onReorder(sp){
   
    if(!sale.isBillRequested()){
        if(sp.sale_product_status == "New" || sale.setting.pos_setting.allow_change_quantity_after_submit == 1){
            sale.updateQuantity(sp, sp.quantity + 1)
        }else{
          
                let strFilter = `$.product_code=='${sp.product_code}' && $.append_quantity ==1 && $.price==${sp.price} && $.portion=='${sp.portion}'  && $.modifiers=='${sp.modifiers}'  && $.unit=='${sp.unit}'  && $.is_free==0`
            
            if (!gv.setting?.pos_setting?.allow_change_quantity_after_submit) {
                strFilter = strFilter + ` && $.sale_product_status == 'New'`
            }
         
            const sale_product  = Enumerable.from(sale.sale.sale_products).where(strFilter).firstOrDefault()

            if (sale_product  != undefined) {
                sale_product.quantity = parseFloat(sale_product.quantity) + 1;
                sale.updateSaleProduct(sp);

            }else{
                setTimeout(() => {
                sale.cloneSaleProduct(sp,sp.quantity+1);   
            }, 100);
            }
              
           
            
         
        }
        
    }
}
</script>
<style scoped>
.selected,
.item-list:hover {
    background-color: #ffebcc !important;
}
 
.item-list.submitted::before{
    content: '';
    position: absolute;
    top: 1px; 
    bottom: 1px;
    left: 0px;
    width: 2px;
    background: #75c34a;
    border-radius: 12px;
}
</style>
