<template>
    <div class="p-2 border border-gray-600 rounded-md">
        <div class="flex">
            <div class="flex-auto cursor-pointer" @click="onSearchCustomer">
                <div class="flex">
                    <v-avatar v-if="sale.sale.customer_photo">
                        <v-img :src="sale.sale.customer_photo"></v-img>
                    </v-avatar>
                    <template v-if="sale.sale.customer_name != undefined">
                        <avatar v-if="sale.sale.customer_photo == undefined" :name="sale.sale.customer_name" class="mr-4" size="40"></avatar>
                    </template>
                    <div class="px-2">
                        <div class="font-bold">{{ sale.sale.customer_name }}</div>
                        <div class="text-gray-400 text-sm">{{ subTitle }}</div>
                    </div>
                </div>
            </div>
            <div class="flex-none"  v-if="sale.sale.customer != setting.customer">
                <v-btn size="small" variant="text" color="primary" icon="mdi-account-edit" @click="onSearchCustomer"></v-btn>
                <v-btn size="small" variant="text" color="error" icon="mdi-delete" @click="onRemove()"></v-btn>
            </div>
        </div>
    </div>
</template>
<script setup>
import { computed, inject, searchCustomerDialog } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";
const sale = inject("$sale")

const toaster = createToaster({ position: "top" });
async function onSearchCustomer() {
    if (!sale.isBillRequested()) {
        const result = await searchCustomerDialog({});
        if (result) {
            sale.sale.customer = result.name;
            sale.sale.customer_name = result.customer_name_en;
            sale.sale.customer_photo = result.photo;
            sale.sale.phone_number = result.phone_number;

            if (parseFloat(result.default_discount)) {
                sale.sale.discount = parseFloat(result.default_discount);
                sale.updateSaleSummary();
                toaster.info("This customer has default discount " + sale.sale.discount + '%');
            }
        }
    }
}
const setting = computed(()=>{
    return JSON.parse(localStorage.getItem('setting'))
})
const subTitle = computed(() => {
    let title = sale.sale.customer;
    if (sale.sale.phone_number != "" && sale.sale.phone_number != undefined) {
        title = title + " / " + sale.sale.phone_number
    }

    return title;
})

function onRemove() {
    sale.sale.customer = setting.value.customer
    sale.sale.customer_name = setting.value.customer_name
    sale.sale.customer_photo = setting.value.customer_photo
}


</script>