<template> 
    <v-list class="!p-0">
        <v-list-item v-for="sp, index in (readonly == true ? getSaleProducts(groupKey) : sale.getSaleProducts(groupKey))"
            :key="index" @click="!readonly ? { click: sale.onSelectSaleProduct(sp) } : {}"
            class="!border-t !border-gray-300 !mb-0 !p-2"
            :class="{ 'selected': (sp.selected && !readonly), 'submitted relative': sp.sale_product_status == 'Submitted', 'item-list': !readonly }">

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
                            <div> {{ sp.product_name }}<v-chip class="ml-1" size="x-small" color="error" variant="outlined" v-if="sp.portion">{{ sp.portion }}</v-chip>
                                <v-chip v-if="sp.is_free" size="x-small" color="success" variant="outlined">{{ $t('Free') }}</v-chip> 
                                <ComChip :tooltip="sp.happy_hours_promotion_title" v-if="sp.happy_hour_promotion && sp.discount > 0" size="x-small" variant="outlined" color="orange" text-color="white" prepend-icon="mdi-tag-multiple">
                                    <span>{{ sp.discount }}%</span>        
                                </ComChip>                   
                                <ComHappyHour :saleProduct="sp" v-if="sp.is_render"/>
                            </div>
                            <div>
                                {{ sp.quantity }} x
                                <CurrencyFormat :value="sp.price" />
                            </div>
                            <div class="text-xs pt-1">
                                <div v-if="sp.modifiers">
                                    <span>{{ sp.modifiers }} (
                                        <CurrencyFormat :value="sp.modifiers_price * sp.quantity" />)
                                    </span>
                                </div>
                                    <div v-if="sp.is_combo_menu">
                                        <div v-if="sp.use_combo_group && sp.combo_menu_data">
                                            <ComSaleProductComboMenuGroupItemDisplay :combo-menu-data="sp.combo_menu_data"/>
                                        </div>
                                        <span v-else>{{ sp.combo_menu }}</span>
                                    </div>  
                                    <div v-if="sp.discount > 0 && !sp.is_free">
                                        <span  class="text-red-500">
                                            {{ $t('Discount') }} :
                                            <span v-if="sp.discount_type == 'Percent'">{{ sp.discount }}%</span>
                                            <CurrencyFormat v-else :value="parseFloat(sp.discount)" />
                                        </span>
                                    </div>
                                <v-chip color="blue" size="x-small" v-if="sp.seat_number"> {{$t('Seat')+"# "+ sp.seat_number  }}</v-chip>
                                
                                <div class="text-gray-500" v-if="sp.note">
                                {{ $t('Note') }}: <span>{{ sp.note }}</span>
                                </div>
                            </div>
                        </div>
                        
                        <div class="flex-none text-right w-36">
                            <div class="text-lg">
                                <CurrencyFormat :value="(sp.amount - sp.total_tax)" />
                            </div>
                            <span v-if="sp.product_tax_rule && sp.total_tax >0" class="text-xs">
                                {{ $t('Tax') }}: 
                                <CurrencyFormat :value="sp.total_tax" />
                            </span> 
                                <ComQuantityInput v-if="!readonly" :sale-product="sp" />                          
                        </div>
                    </div>


                    <div v-if="sp.selected && !readonly" class="-mx-1 flex pt-1"> 
                        <v-chip v-if="show_button_change_price"  color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small"  @click="sale.onChangePrice(sp,gv,numberFormat)">{{ $t('Price') }}</v-chip>
                       
                        <v-chip
                            :disabled="sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status == 'Submitted'"
                            color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small"
                            @click="sale.onChangeQuantity(sp)">{{ $t('Qty') }}</v-chip>

                        <!-- <v-chip
                            :disabled="sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status == 'Submitted'"
                            color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small"
                            @click="onEditSaleProduct(sp)">{{ $t('Edit') }}</v-chip> -->
                        
                        <v-chip color="teal" class="mx-1 grow text-center justify-center" variant="elevated" size="small"
                            @click="onReorder(sp)">{{ $t('Re-Order') }}</v-chip>   
                        
                            <v-chip color="red" class="mx-1 grow text-center justify-center" variant="elevated" size="small"
                            @click="sale.onRemoveItem(sp,gv,numberFormat)">{{ $t('Delete') }}</v-chip>

                        <ComSaleProductButtonMore :sale-product="sp" />
                    </div>

                </div>
            </template>
        </v-list-item>
    </v-list>
</template>
<script setup>
import {computed, inject, defineProps, createToaster,keypadWithNoteDialog,i18n } from '@/plugin'

import ComSaleProductButtonMore from './ComSaleProductButtonMore.vue';
import ComQuantityInput from '../../../components/form/ComQuantityInput.vue';
import Enumerable from 'linq';
import ComSaleProductComboMenuGroupItemDisplay from './combo_menu/ComSaleProductComboMenuGroupItemDisplay.vue'; 
import ComHappyHour from './happy_hour_promotion/ComHappyHour.vue';

const { t: $t } = i18n.global;  
const numberFormat = inject('$numberFormat');
const sale = inject('$sale');
const product = inject('$product');
const gv = inject('$gv');
const moment = inject('$moment');
const toaster = createToaster({ position: 'top' });

const props = defineProps({
    groupKey: Object,
    readonly: Boolean,
    saleCustomerDisplay: Object
});

function onEditSaleProduct(sp) { 
    if (!sale.isBillRequested()) {
        if (sp.sale_product_status == "New" || sale.setting.pos_setting.allow_change_quantity_after_submit == 1) {
            const is_has_product = product.setSelectedProductByMenuID(sp.menu_product_name);
            if(is_has_product){                 
                product.setModifierSelection(sp);
                if(sp.is_combo_menu && sp.use_combo_group){
                    product.setComboGroupSelection(sp)
                }

                if ((sp.is_combo_menu && sp.use_combo_group) || product.modifiers.length > 0 || product.prices.filter(r => r.price_rule == sale.setting.price_rule && (r.branch == sale.setting.business_branch || r.branch == '')).length > 1){
                    sale.OnEditSaleProduct(sp)
                }
                else{
                    toaster.warning("msg.This item has no option to edit")
                }
            }
        } else {
            toaster.warning("msg.Submitted order is not allow to edit");
        }
    }
}


const show_button_change_price = computed(()=>{
    if(gv.device_setting.is_order_station == 1 && gv.device_setting.show_button_change_price_on_order_station==1)
    {
        return true;
    }
    else if(gv.device_setting.is_order_station==0 ){
        return true;
    }

    return false;
});  

function onReorder(sp) {
    if (!sale.isBillRequested()) {
        if (sp.sale_product_status == "New" || sale.setting.pos_setting.allow_change_quantity_after_submit == 1) {
            sale.updateQuantity(sp, sp.quantity + 1)
        } else {
            let strFilter = `$.product_code=='${sp.product_code}' && $.append_quantity ==1 && $.price==${sp.price} && $.portion=='${sp.portion}'  && $.modifiers=='${sp.modifiers}'  && $.unit=='${sp.unit}'  && $.is_free==0`

            if (!gv.setting?.pos_setting?.allow_change_quantity_after_submit) {
                strFilter = strFilter + ` && $.sale_product_status == 'New'`
            }
            const sale_product = Enumerable.from(sale.sale.sale_products).where(strFilter).firstOrDefault();
            if (sale_product != undefined) {
                sale_product.quantity = parseFloat(sale_product.quantity) + 1;
                sale.updateSaleProduct(sp);

            } else {
                setTimeout(() => {
                    sale.cloneSaleProduct(sp, sp.quantity + 1);
                }, 100);
            }
        }
    }
}

function getSaleProducts(groupByKey) {
    if (props.saleCustomerDisplay && props.saleCustomerDisplay.sale_products) {
        if (groupByKey) {
            return Enumerable.from(props.saleCustomerDisplay.sale_products).where(`$.order_by=='${groupByKey.order_by}' && $.order_time=='${groupByKey.order_time}'`).orderByDescending("$.modified").toArray()
        } else {
            return Enumerable.from(props.saleCustomerDisplay.sale_products).orderByDescending("$.modified").toArray();
        }
    }
    return [];
} 

</script>


<style scoped>
    .selected,
    .item-list:hover {
        background-color: #ffebcc !important;
    }

    .submitted::before {
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
 