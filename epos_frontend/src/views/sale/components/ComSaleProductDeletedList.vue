<template>
     <v-list class="!p-0">
    <v-list-item v-for="sp, index in getDeletedSaleProducts(groupKey)" :key="index"  class="!border-t !border-gray-300 !mb-0 !p-2" >
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
                            {{ sp.deleted_quantity }} x
                            <CurrencyFormat :value="sp.price" />
                        </div>
                        <div class="text-xs pt-1">
                            <div v-if="sp.modifiers">
                                <span>{{ sp.modifiers }} (
                                    <CurrencyFormat :value="sp.modifiers_price * sp.deleted_quantity" />)
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
                            <CurrencyFormat :value="0" />
                        </div> 
                        <div>
                            <v-chip class="ml-1 mb-1" size="small" color="error" variant="outlined">{{ `${$t('QTY Deleted')}: ${sp.deleted_quantity}` }} </v-chip>
                        </div>                         
                    </div>
                </div> 
            </div>
        </template>
    </v-list-item>
</v-list>
</template>


<script setup>
    import {computed, inject, defineProps, createToaster,keypadWithNoteDialog,i18n } from '@/plugin';
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



    function getDeletedSaleProducts(groupByKey) {    
        const sale_products = sale.deletedSaleProductsDisplay.filter((x)=>x.show_in_list==true);
        if (groupByKey) {
            return Enumerable.from(sale_products).where(`$.order_by=='${groupByKey.order_by}' && $.order_time=='${groupByKey.order_time}'`).orderByDescending("$.modified").toArray()
        } else {
            return Enumerable.from(sale_products).orderByDescending("$.modified").toArray();
        }
    }


</script>