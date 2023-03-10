<template>
    <div class="py-2 flex flex-wrap">
        <ComSaleTypeChip/>
        <ComChip tooltip="POS Profile" prepend-icon="mdi-desktop-classic">{{ sale.sale.pos_profile }}</ComChip>
        <ComChip v-if="sale.working_day_resource?.loading" tooltip="Working Day" prepend-icon="mdi-spin mdi-loading">loading...</ComChip>
        <ComChip v-else tooltip="Working Day" prepend-icon="mdi-calendar">{{ sale.sale.working_day }}</ComChip>
        <ComChip v-if="sale.cashier_shift_resource?.loading" tooltip="Cashier Shift" prepend-icon="mdi-spin mdi-loading">loading...</ComChip>
        <ComChip v-else tooltip="Cashier Shift" prepend-icon="mdi-calendar-clock">{{ sale.sale.cashier_shift }}</ComChip>
        <v-chip  
                v-if="sale.sale.table_id"
                style="margin: 1px;" 
                :size="screen.chipSize"
                rounded="pill" 
                variant="tonal"
                prepend-icon="mdi-account-multiple-outline"
                @click="onUpdateGuestCover()">
                {{ sale.sale.guest_cover }}
            </v-chip>
        <ComChip tooltip="Price Rule" prepend-icon="mdi-bulletin-board">{{ sale.sale.price_rule }}</ComChip>   
    </div>
</template>
<script setup>
import ComSaleTypeChip from './ComSaleTypeChip.vue';
import { inject,keyboardDialog } from '@/plugin';
const sale = inject("$sale")
const screen = inject("$screen")
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
</script>