<template>
    <div class="py-2 flex flex-wrap">
        <ComSaleTypeChip/>
        <ComChip tooltip="POS Profile" prepend-icon="mdi-desktop-classic">{{ sale.sale.pos_profile }}</ComChip>
        <ComChip v-if="sale.working_day_resource?.loading" tooltip="Working Day" prepend-icon="mdi-spin mdi-loading">loading...</ComChip>
        <ComChip v-else tooltip="Working Day" prepend-icon="mdi-calendar">{{ sale.sale.working_day }}</ComChip>
        <ComChip v-if="sale.cashier_shift_resource?.loading" tooltip="Cashier Shift" prepend-icon="mdi-spin mdi-loading">loading...</ComChip>
        <ComChip v-else tooltip="Cashier Shift" prepend-icon="mdi-calendar-clock">{{ sale.sale.cashier_shift }}</ComChip>
        <ComChip v-if="setting.table_groups && setting.table_groups.length > 0 && setting.use_guest_cover == 1" tooltip="Guest Cover" prepend-icon="mdi-account-multiple-outline" @onClick="onUpdateGuestCover()">{{ sale.sale.guest_cover }}</ComChip>
        <ComChip v-if="setting.table_groups && setting.table_groups.length > 0 && sale.sale.seat_number" tooltip="Seat Number" prepend-icon="mdi-chair-school" @onClick="onUpdateSeatNumber()">{{ sale.sale.seat_number }}</ComChip>
        <ComChip tooltip="Price Rule" prepend-icon="mdi-bulletin-board" @onClick="onChangePriceRule()">{{ sale.sale.price_rule }}</ComChip>   
    </div>
</template>
<script setup>
import ComSaleTypeChip from './ComSaleTypeChip.vue';
import { inject,keyboardDialog,changePriceRuleDialog, createToaster  } from '@/plugin';
const toaster = createToaster({position: 'top'})
const sale = inject("$sale")
const setting = JSON.parse(localStorage.getItem("setting"))

async function onUpdateGuestCover(){
    if (setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: "Guest Cover", type: 'number', value: sale.sale.guest_cover });
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
   
        const result = await keyboardDialog({ title: "Change Seat Number", type: 'number', value: sale.sale.seat_number });
        if(result){
            if (typeof result != 'boolean' && result != false) {
            sale.sale.seat_number = result;
            }
        }
}
async function onChangePriceRule() {
    if (sale.sale.sale_status != 'New') {
        toaster.warning("This sale order is not new order.");
        return;
    }
    if (!sale.isBillRequested()) {
        const result = await changePriceRuleDialog({})
        if (result == true) {
            product.loadPOSMenu()
            toaster.success("Price Rule Was Change Successfull");
        }
    }
}
</script>