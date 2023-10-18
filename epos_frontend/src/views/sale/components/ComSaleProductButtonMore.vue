<template >
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-chip v-bind="props" variant="elevated" color="primary" class="mx-1 grow text-center justify-center"
                size="small">{{ $t('More') }}</v-chip>
        </template>
        <v-list>
            <v-list-item prepend-icon="mdi-pencil" :title="$t('Edit')" v-if="!(sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || saleProduct.sale_product_status == 'Submitted')"
                @click="onEditSaleProduct(saleProduct)"></v-list-item>

            <template v-if="gv.device_setting.is_order_station==0">
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
                
            </template>

            <template v-if="!(saleProduct.is_require_employee||false)">
                <v-list-item v-if="tableLayout.table_groups && tableLayout.table_groups.length > 0" prepend-icon="mdi-chair-school" :title="($t('Seat')+'#')"
                    @click="sale.onSaleProductSetSeatNumber(saleProduct)"></v-list-item>
            </template>

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
            <v-list-item prepend-icon="mdi-cash-100" :title="$t('Choose Printer')" @click="sale.onChoosePrinter(sale.sale)">
            </v-list-item>
        </v-list>
    </v-menu>
</template>

<script setup>
import { defineProps, inject,keypadWithNoteDialog,i18n } from '@/plugin'
import {createToaster} from '@meforma/vue-toaster';
const { t: $t } = i18n.global;  

const product = inject('$product');
const sale = inject('$sale');
const numberFormat = inject('$numberFormat');
const gv = inject("$gv");
const tableLayout = inject("$tableLayout");
const props = defineProps({
    saleProduct: Object
});

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

function onEditSaleProduct(sp) { 
    if (!sale.isBillRequested()) {
        if (sp.sale_product_status == "New" || sale.setting.pos_setting.allow_change_quantity_after_submit == 1) {
            const is_has_product = product.setSelectedProductByMenuID(sp.menu_product_name);
            if(is_has_product){                 
                product.setModifierSelection(sp);
                if(sp.is_combo_menu && sp.use_combo_group){
                    product.setComboGroupSelection(sp)
                }

                if ((sp.is_combo_menu && sp.use_combo_group) || product.modifiers.length > 0 || product.prices.filter(r => r.price_rule == sale.setting.price_rule && (r.branch == sale.setting.business_branch || r.branch == '')).length > 1){
                    sale.OnEditSaleProduct(sp)
                }
                else{
                    toaster.warning($t("msg.This item has no option to edit"))
                }
            }
        } else {
            toaster.warning($t("msg.Submitted order is not allow to edit"));
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