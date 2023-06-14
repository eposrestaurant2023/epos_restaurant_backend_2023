<template >
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-chip v-bind="props" variant="elevated" color="primary" class="mx-1 grow text-center justify-center"
                size="small">{{ $t('More') }}</v-chip>
        </template>
        <v-list>
            <v-list-item prepend-icon="mdi-currency-usd-off" :title="$t('Free')" v-if="!saleProduct.is_free"
                @click="onSaleProductFree()"></v-list-item>
            <v-list-item v-else @click="sale.onSaleProductCancelFree(saleProduct)">
                <template v-slot:prepend>
                    <v-icon icon="mdi-currency-usd-off" color="error"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">{{ $t('Cancel Free') }}</v-list-item-title>
            </v-list-item>
            <template v-if="!saleProduct.is_free">
                <template v-if="!saleProduct.happy_hour_promotion">
                    <v-list-item prepend-icon="mdi-percent" :title="$t('Discount Percent')"
                        @click="onSaleProductDiscount('Percent')"></v-list-item>
                    <v-list-item prepend-icon="mdi-currency-usd" :title="$t('Discount Amount')"
                        @click="onSaleProductDiscount('Amount')"></v-list-item>
                </template>
                <v-list-item v-if="saleProduct.discount > 0" @click="onSaleProductCancelDiscount()">
                    <template v-slot:prepend>
                        <v-icon icon="mdi-tag-multiple" color="error"></v-icon>
                    </template>
                    <v-list-item-title class="text-red-700">{{ $t('Cancel Discount') }}</v-list-item-title>
                </v-list-item>
            </template>
            <v-list-item v-if="tableLayout.table_groups && tableLayout.table_groups.length > 0" prepend-icon="mdi-chair-school" :title="($t('Seat')+'#')"
                @click="sale.onSaleProductSetSeatNumber(saleProduct)"></v-list-item>
            <v-list-item prepend-icon="mdi-note-outline" :title="$t('Note')" v-if="!saleProduct.note"
                @click="sale.onSaleProductNote(saleProduct)"></v-list-item>
            <v-list-item v-else @click="onRemoveNote">
                <template v-slot:prepend>
                    <v-icon icon="mdi-note-outline" color="error"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">{{ $t('Remove Note') }}</v-list-item-title>
            </v-list-item>
   

            <v-list-item prepend-icon="mdi-cash-100" :title="$t('Tax Setting')" v-if="saleProduct.product_tax_rule"  @click="sale.onSaleProductChangeTaxSetting(saleProduct,gv)">
            </v-list-item>

            <v-list-item @click="onRemoveSaleProduct()">
                <template v-slot:prepend>
                    <v-icon icon="mdi-delete" color="error"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">{{ $t('Delete') }}</v-list-item-title>
            </v-list-item>
        </v-list>
    </v-menu>
</template>

<script setup>
import { defineProps, inject,keypadWithNoteDialog,i18n } from '@/plugin'
import {createToaster} from '@meforma/vue-toaster';
const { t: $t } = i18n.global;  
const sale = inject('$sale')
const numberFormat = inject('$numberFormat')
const gv = inject("$gv")
const tableLayout = inject("$tableLayout")
const props = defineProps({
    saleProduct: Object
})

const toaster = createToaster({ position: "top" });
function onRemoveNote() {
    props.saleProduct.note = "";
}

function onSaleProductFree() {
    if (!sale.isBillRequested()) {
        gv.authorize("free_item_required_password", "free_item", "free_item_required_note", "Free Item Note", props.saleProduct.product_code).then((v) => {
            if (v) {
                props.saleProduct.free_note = v.note
                sale.onSaleProductFree(props.saleProduct);
            }
        });

    }
}
sale.vue.$onKeyStroke('F8', (e) => {
    e.preventDefault()
    if(sale.dialogActiveState==false && props.saleProduct.selected == true){
        sale.onSaleProductNote(props.saleProduct)
    } 
})

function onRemoveSaleProduct() { 
    if (!sale.isBillRequested()) {        
        if (props.saleProduct.sale_product_status == 'Submitted') {
            gv.authorize("delete_item_required_password", "delete_item","delete_item_required_note", "Delete Item Note", props.saleProduct.product_code, true).then(async (v) => {
                if (v) {
                    //props.saleProduct.delete_item_note = v.note 
                    const result = await keypadWithNoteDialog({ 
                        data: { 
                            title: `${$t('Delete Item')} ${props.saleProduct.product_name}`,
                            label_input: $t('Enter Quantity'),
                            note: "Delete Item Note",
                            category_note_name: v.category_note_name,
                            number: props.saleProduct.quantity,
                            product_code: props.saleProduct.product_code
                        } 
                    });
                  
                    if(result){
                        if(props.saleProduct.quantity < result.number){
                            result.number = props.saleProduct.quantity;
                        }                       
                            
                        props.saleProduct.deleted_item_note = result.note;
                        sale.onRemoveSaleProduct(props.saleProduct, result.number);  

                        let msg = `User ${v.user} delete Item: ${props.saleProduct.product_code}-${props.saleProduct.product_name}.${props.saleProduct.portion} ${props.saleProduct.modifiers}`; 
                        msg += `, Qty: ${result.number}`;
                        msg += `, Amount: ${ numberFormat(gv.getCurrnecyFormat,props.saleProduct.amount)}`;
                        msg += `${result.note==""?'':', Reason: '+result.note }`;
                        sale.auditTrailLogs.push({
                            doctype:"Comment",
                            subject:"Delete Sale Product",
                            comment_type:"Comment",
                            reference_doctype:"Sale",
                            reference_name:"New",
                            comment_by:v.user,
                            content:msg
                        })  ;                    

                    } 
                }
            });
        } else {

            const u = JSON.parse(localStorage.getItem('make_order_auth')); 
            sale.onRemoveSaleProduct(props.saleProduct, props.saleProduct.quantity);

            let msg = `User ${u.name} delete Item: ${props.saleProduct.product_code}-${props.saleProduct.product_name}.${props.saleProduct.portion} ${props.saleProduct.modifiers}`; 
            msg += `, Qty: ${props.saleProduct.quantity}`;
            msg += `, Amount: ${ numberFormat(gv.getCurrnecyFormat,props.saleProduct.amount)}`;
            sale.auditTrailLogs.push({
                doctype:"Comment",
                subject:"Delete Sale Product",
                comment_type:"Comment",
                reference_doctype:"Sale",
                reference_name:"New",
                comment_by: u.name,
                content:msg
            }) ;
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
                        props.saleProduct.discount_note,
                        props.saleProduct,
                        v.category_note_name
                    );
                }
            });
        }
    }
    else{
        toaster.warning($t('msg.This item is not allow to discount'));
    }
}
function onSaleProductCancelDiscount() {
    if (!sale.isBillRequested()) {
        props.saleProduct.discount = 0;
        props.saleProduct.discount_type = 'Amount'
        props.saleProduct.happy_hour_promotion = ''
        props.saleProduct.happy_hours_promotion_title = ''
        sale.updateSaleProduct(props.saleProduct)
        sale.updateSaleSummary();
    }
}



</script>