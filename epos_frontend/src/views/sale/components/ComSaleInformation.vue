<template>
    <div class="py-2 flex flex-wrap">
        <ComSaleTypeChip/>
    <ComChip tooltip="POS Profile" prepend-icon="mdi-desktop-classic">{{ sale.sale.pos_profile }}</ComChip>
        <ComChip tooltip="Working Day" prepend-icon="mdi-calendar">{{ sale.sale.working_day }}</ComChip>
        <ComChip tooltip="Cashier Shift" prepend-icon="mdi-calendar-clock">{{ sale.sale.cashier_shift }}</ComChip>
        <!-- <ComChip tooltip="Guest Cover" prepend-icon="mdi-account-multiple-outline" @onClick="">{{ sale.sale.guest_cover }}</ComChip> -->
        <v-chip  
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
import { inject,useRouter,createResource,keyboardDialog } from '@/plugin';
import { createToaster } from '@meforma/vue-toaster';
const sale = inject("$sale")
const screen = inject("$screen")
const router = useRouter();
const toaster = createToaster({ position: "top" });
const setting = JSON.parse(localStorage.getItem("setting"))

const working_day = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(data) {
        if (data == undefined) {
            toaster.warning("Please start working day first");
            router.push({ name: "Home" });
        } else{
            if(!sale.name){  
                sale.sale.working_day = data.name;
               
            }
        }
    }
})

createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(data) {
        if (data ==undefined) {
            toaster.warning("Please start cashier shift first.", { position: "top" });
            router.push({ name: "Home" });
        }else{
            if(!sale.name){ 
            sale.sale.cashier_shift = data.name;
            }
        }
    }

})

async function onUpdateGuestCover(){
    if (setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: "Guest Cover", type: 'number', value: sale.sale.guest_cover });
        if (typeof result != 'boolean' && result == false) {
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