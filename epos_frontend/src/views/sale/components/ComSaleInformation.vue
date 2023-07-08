<template>
    <div class="py-2 flex flex-wrap">
        <ComSaleTypeChip v-if="product.setting.pos_menus.length>0"/>
        <ComChip v-if="!sale.load_menu_lang" :tooltip="$t('Menu Language')" prepend-icon="mdi-translate"   @onClick="onChangeMenuLanguage()"></ComChip>
        <ComChip :tooltip="$t('POS Profile')" prepend-icon="mdi-desktop-classic">{{ sale.sale.pos_profile }}</ComChip>
        <ComChip v-if="sale.working_day_resource?.loading" :tooltip="$t('Working Day')" prepend-icon="mdi-spin mdi-loading">{{ $t('Loading') }}...</ComChip>
        <ComChip v-else :tooltip="$t('Working Day')" prepend-icon="mdi-calendar">{{ sale.sale.working_day }}</ComChip>
        <ComChip v-if="sale.cashier_shift_resource?.loading" :tooltip="$t('Cashier Shift')" prepend-icon="mdi-spin mdi-loading">{{ $t('Loading') }}...</ComChip>
        <ComChip v-else :tooltip="$t('Cashier Shift')" prepend-icon="mdi-calendar-clock">{{ sale.sale.cashier_shift }}</ComChip>
        <ComChip v-if="setting.table_groups && setting.table_groups.length > 0 && setting.use_guest_cover == 1" :tooltip="$t('Guest Cover')" prepend-icon="mdi-account-multiple-outline" @onClick="onUpdateGuestCover()">{{ sale.sale.guest_cover }}</ComChip>
        <ComChip v-if="setting.table_groups && setting.table_groups.length > 0 && sale.sale.seat_number" :tooltip="($t('Seat')+' #')" prepend-icon="mdi-chair-school" @onClick="onUpdateSeatNumber()">{{ sale.sale.seat_number }}</ComChip>
        <ComChip :tooltip="$t('Price Rule')" prepend-icon="mdi-bulletin-board" @onClick="onChangePriceRule()">{{ sale.sale.price_rule }}</ComChip>
        <ComSaleInformationHappyHourPromotionChip/>
    </div>
</template>
<script setup>
import ComSaleTypeChip from './ComSaleTypeChip.vue';
import ComSaleInformationHappyHourPromotionChip from './happy_hour_promotion/ComSaleInformationHappyHourPromotionChip.vue';
import { inject,keyboardDialog,changePriceRuleDialog, createToaster,i18n ,computed } from '@/plugin';
 

const { t: $t } = i18n.global;   


const toaster = createToaster({position: 'top'})
const sale = inject("$sale")
const product = inject("$product")
const setting = JSON.parse(localStorage.getItem("setting"))



async function onChangeMenuLanguage(){
  sale.onChangeMenuLanguage()   ;
  await  setTimeout(function() {
            sale.load_menu_lang = false;
    },1);        
}

async function onUpdateGuestCover(){
    if (setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: $t('Guest Cover'), type: 'number', value: sale.sale.guest_cover });
        if (typeof result != 'boolean' && result != false) {
            sale.sale.guest_cover = parseInt(result);
            if (sale.sale.guest_cover == undefined || isNaN(sale.sale.guest_cover)) {
                sale.sale.guest_cover = 0;
            }

        } else { 
            return;
        }
    }
}
async function onUpdateSeatNumber(){   
        const result = await keyboardDialog({ title: $t('Change Seat Number'), type: 'number', value: sale.sale.seat_number });
        if(result){
            if (typeof result != 'boolean' && result != false) {
            sale.sale.seat_number = result;
            }
        }
}
async function onChangePriceRule() {
    if (sale.sale.sale_status != 'New') {
        toaster.warning($t('msg.This bill is not new order'));
        return;
    }
    if (!sale.isBillRequested()) {
        const result = await changePriceRuleDialog({})
        if (result == true) {
            if(product.setting.pos_menus.length>0){
                product.loadPOSMenu()
            }else{
                product.loadPOSMenu()
                product.getProductMenuByProductCategory(db,"All Product Categories")
            }
            
            toaster.success("msg.Change price rule successfully");
        }
    }
}
</script>