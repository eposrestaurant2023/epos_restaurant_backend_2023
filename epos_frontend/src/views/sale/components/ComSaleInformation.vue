<template>
    <ComSaleTypeChip/>
    <v-chip>{{ sale.sale.pos_profile }}</v-chip>
    <v-chip>{{ sale.sale.working_day }}</v-chip>
    <v-chip>{{ sale.sale.cashier_shift }}</v-chip>
    <ComExchangeRate/>
    <v-chip>Guest Cover: {{ sale.sale.guest_cover }}</v-chip>
    <v-chip> Price Rule: {{ sale.sale.price_rule }}</v-chip>
</template>
<script setup>
import ComSaleTypeChip from './ComSaleTypeChip.vue';
import ComExchangeRate  from './ComExchangeRate.vue';
import { inject,useRouter,createResource } from '@/plugin';
import { createToaster } from '@meforma/vue-toaster';
const sale = inject("$sale")
const router = useRouter();
const toaster = createToaster({ position: "top" });


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
            if(sale.name){  
                sale.sale.working_day = data.name;
                alert("set workind aday to sale")
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
            sale.sale.cashier_shift = data.cashier_shift;
        }
    }

})
</script>