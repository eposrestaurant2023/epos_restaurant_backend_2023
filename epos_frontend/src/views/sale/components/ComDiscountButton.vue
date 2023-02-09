<template>
    <v-menu>
        <template v-slot:activator="{ props }">
            <div class="cursor-pointer p-2 grow bg-blue-600 hover:bg-blue-700 text-white" v-bind="props">
                Discount
            </div>
        </template>
        <v-list>
            <ComDiscountButtonList/>
            <!-- <v-list-item prepend-icon="mdi-percent" @click="onSaleDiscount('Percent')">
                <v-list-item-title>Percentage</v-list-item-title>
            </v-list-item>
            <v-list-item @click="onSaleDiscount('Amount')" prepend-icon="mdi-currency-usd">
                <v-list-item-title>Amount</v-list-item-title>
            </v-list-item>
            <v-list-item v-if="sale.sale.discount > 0" @click="onSaleCancelDiscount()">
                <template v-slot:prepend>
                    <v-icon icon="mdi-tag-multiple" color="error"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">Cancel Discount</v-list-item-title>
            </v-list-item> -->
        </v-list>
    </v-menu>
</template>
<script setup>
import ComDiscountButtonList from './ComDiscountButtonList.vue';
// import { inject, createToaster } from "@/plugin"
// const sale = inject('$sale')
// const gv = inject("$gv")
// const toaster = createToaster({ position: "top" });
// function onSaleDiscount(discount_type) {
//     if (sale.sale.sale_products.length == 0) {
//         toaster.warning("Please select a menu item to discount");
//         resolve(false);
//     }
//     else if (!sale.isBillRequested()) {
//         gv.authorize("discount_sale_required_password", "discount_sale", "discount_sale_required_note", "Discount Sale Note", "", true).then((v) => {
//             if (v) {
//                 sale.onDiscount(
//                     `${sale.sale.name ? sale.sale.name : "New Sale"} Discount`,
//                     sale.sale.sub_total,
//                     sale.sale.discount,
//                     discount_type,
//                     v.discount_codes,
//                     null,
//                     v.category_note_name
//                 );
//             }
//         });
//     }
// }
// function onSaleCancelDiscount() {
//     sale.sale.discount = 0;
//     sale.sale.discount_type = 'Amount'
//     sale.updateSaleSummary();
// }
</script>