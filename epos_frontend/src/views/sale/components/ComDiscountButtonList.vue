<template>
    <v-list-item prepend-icon="mdi-percent" @click="onSaleDiscount('Percent')">
        <v-list-item-title>{{ $t('Discount Percent') }}</v-list-item-title>
    </v-list-item>
    <v-list-item  @click="onSaleDiscount('Amount')" prepend-icon="mdi-currency-usd">
        <v-list-item-title>{{ $t('Discount Amount') }}</v-list-item-title>
    </v-list-item>
    <v-list-item v-if="sale.sale.discount > 0" @click="onSaleCancelDiscount()">
        <template v-slot:prepend>
            <v-icon icon="mdi-tag-multiple" color="error"></v-icon>
        </template>
        <v-list-item-title class="text-red-700">{{ $t('Cancel Discount')}}</v-list-item-title>
    </v-list-item>
</template>

<script setup>
import { inject, createToaster } from "@/plugin";

const { t: $t } = i18n.global;  

const sale = inject('$sale')
const gv = inject("$gv")
const toaster = createToaster({ position: "top" });



function onSaleDiscount(discount_type) {
    sale.dialogActiveState=true;
    if (sale.sale.sale_products.length == 0) {
        toaster.warning("Please select a menu item to discount");
        resolve(false);
    }
    else if (!sale.isBillRequested()) { 
        gv.authorize("discount_sale_required_password", "discount_sale", "discount_sale_required_note", "Discount Sale Note", "", true).then((v) => {
            if (v) {
                sale.onDiscount(
                    `Discount`,
                    sale.sale.sale_discountable_amount,
                    sale.sale.discount,
                    discount_type,
                    v.discount_codes,
                    sale.sale.discount_note,
                    null,
                    v.category_note_name
                );
            }
        });

    }
}
function onSaleCancelDiscount() {
    sale.sale.discount = 0;
    sale.sale.discount_type = 'Amount'
    sale.updateSaleSummary();
}
</script>