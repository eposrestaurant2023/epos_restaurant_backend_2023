<template >
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-chip v-bind="props" variant="elevated" color="primary" class="mx-1 grow text-center justify-center"
                size="small">More</v-chip>
        </template>
        <v-list>
            <v-list-item prepend-icon="mdi-currency-usd-off" title="Free" v-if="!saleProduct.is_free"
                @click="onSaleProductFree()"></v-list-item>
            <v-list-item v-else @click="sale.onSaleProductCancelFree(saleProduct)">
                <template v-slot:prepend>
                    <v-icon icon="mdi-currency-usd-off" color="error"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">Cancel Free</v-list-item-title>
            </v-list-item>
            <template v-if="!saleProduct.is_free">
                <v-list-item prepend-icon="mdi-percent" title="Discount Percent"
                    @click="onSaleProductDiscount('Percent')"></v-list-item>
                <v-list-item prepend-icon="mdi-currency-usd" title="Discount Amount"
                    @click="onSaleProductDiscount('Amount')"></v-list-item>
                <v-list-item v-if="saleProduct.discount > 0" @click="onSaleProductCancelDiscount()">
                    <template v-slot:prepend>
                        <v-icon icon="mdi-tag-multiple" color="error"></v-icon>
                    </template>
                    <v-list-item-title class="text-red-700">Cancel Discount</v-list-item-title>
                </v-list-item>
            </template>
            <v-list-item v-if="tableLayout.table_groups && tableLayout.table_groups.length > 0" prepend-icon="mdi-chair-school" title="Seat #"
                @click="sale.onSaleProductSetSeatNumber(saleProduct)"></v-list-item>
            <v-list-item prepend-icon="mdi-note-outline" title="Note" v-if="!saleProduct.note"
                @click="sale.onSaleProductNote(saleProduct)"></v-list-item>
            <v-list-item v-else @click="onRemoveNote">
                <template v-slot:prepend>
                    <v-icon icon="mdi-note-outline" color="error"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">Remove Note</v-list-item-title>
            </v-list-item>
            <v-list-item @click="onRemoveSaleProduct()">
                <template v-slot:prepend>
                    <v-icon icon="mdi-delete" color="error"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">Delete</v-list-item-title>
            </v-list-item>
        </v-list>
    </v-menu>
</template>
<script setup>
import { defineProps, inject,keypadWithNoteDialog } from '@/plugin'
import {createToaster} from '@meforma/vue-toaster';
const sale = inject('$sale')
const gv = inject("$gv")
const tableLayout = inject("$tableLayout")
const props = defineProps({
    saleProduct: Object
})

const toaster = createToaster({ position: "top" })

function onRemoveNote() {
    props.saleProduct.note = "";
}

function onSaleProductFree() {
    if (!sale.isBillRequested()) {
        gv.authorize("free_item_required_password", "free_item", "free_item_required_note", "Free Item Note", props.saleProduct.product_code).then((v) => {
            if (v) {
                sale.onSaleProductFree(props.saleProduct);
            }
        });

    }
}

function onRemoveSaleProduct() {
 
    if (!sale.isBillRequested()) {
        if (props.saleProduct.sale_product_status == 'Submitted') {
            gv.authorize("delete_item_required_password", "delete_item","delete_item_required_note", "Delete Item Note", props.saleProduct.product_code, true).then(async (v) => {
                if (v) {
                    //props.saleProduct.delete_item_note = v.note 
                    const result = await keypadWithNoteDialog({ 
                        data: { 
                            title: `Delete Product ${props.saleProduct.product_name}`,
                            label_input: 'Enter Quantity',
                            note: "Delete Item Note",
                            category_note_name: v.category_note_name,
                            number: props.saleProduct.quantity,
                            product_code: props.saleProduct.product_code
                        } 
                    });
                  
                    if(result){ 
                        if(props.saleProduct.quantity < result.number)
                            result.number = props.saleProduct.quantity
                            props.saleProduct.deleted_item_note = result.note;
                        sale.onRemoveSaleProduct(props.saleProduct, result.number);

                    } 
                }
            });
        } else {
            sale.onRemoveSaleProduct(props.saleProduct, props.saleProduct.quantity);
        }

    }
}

function onSaleProductDiscount(discount_type) {
    if(props.saleProduct.allow_discount){

     
    if (!sale.isBillRequested()) {
        gv.authorize("discount_item_required_password", "discount_item", "discount_item_required_note","Discount Item Note","",true).then((v) => {
            if (v) {
                sale.onDiscount(
                    `${props.saleProduct.product_name} Discount`,
                    props.saleProduct.amount,
                    props.saleProduct.discount,
                    discount_type,
                    v.discount_codes,
                    props.saleProduct,
                    v.category_note_name
                );
            }
        });

    }
}
else{
    toaster.warning("This product is not allow to discount");
}
}
function onSaleProductCancelDiscount() {
    if (!sale.isBillRequested()) {
        props.saleProduct.discount = 0;
        props.saleProduct.discount_type = 'Amount'
        sale.updateSaleProduct(props.saleProduct)
        sale.updateSaleSummary();
    }
}

</script>