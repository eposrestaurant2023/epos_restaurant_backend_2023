<template>
    <div>
 
        <v-chip class="ml-1 mb-1" size="small" color="error" v-if="saleProduct.deleted_quantity > 0 && gv.device_setting.show_deleted_sale_product_in_sale_screen==1" variant="outlined">{{ `${$t('QTY Deleted')}: ${saleProduct.deleted_quantity}` }} </v-chip>
        

        <v-btn
            v-if="saleProduct.sale_product_status == 'New' || sale.setting.pos_setting.allow_change_quantity_after_submit == 1"
            color="error" size="x-small" variant="tonal" icon="mdi-arrow-down"
            @click="sale.updateQuantity(saleProduct, saleProduct.quantity - 1)"
            :disabled="saleProduct.quantity == 1"></v-btn>        
            
        <v-btn class="mx-1" size="small" variant="tonal" @click="onChangeQuantity">{{
            saleProduct.quantity }}</v-btn>

        <v-btn
            v-if="saleProduct.sale_product_status == 'New' || sale.setting.pos_setting.allow_change_quantity_after_submit == 1"
            color="success" size="x-small" variant="tonal" icon="mdi-arrow-up"
            @click="sale.updateQuantity(saleProduct, saleProduct.quantity + 1)"></v-btn>
    </div>
</template>
<script setup>
import { inject,i18n,computed } from '@/plugin'
import {createToaster} from '@meforma/vue-toaster';

const numberFormat = inject('$numberFormat');
const { t: $t } = i18n.global;  
const toaster = createToaster({ position: "top" })
const props = defineProps({
    saleProduct: Object
})
const sale = inject("$sale");
const gv = inject("$gv");

//Add Key stroke
sale.vue.$onKeyStroke('PageUp', (e) => {
    e.preventDefault()
    if (props.saleProduct.selected) {
        sale.updateQuantity(props.saleProduct, props.saleProduct.quantity + 1)
    }
})
sale.vue.$onKeyStroke('PageDown', (e) => {
    e.preventDefault()
    if (props.saleProduct.selected && props.saleProduct.quantity > 1) {
        sale.updateQuantity(props.saleProduct, props.saleProduct.quantity - 1)
    }
})

if (props.saleProduct.selected) {
    sale.vue.$onKeyStroke('F3', (e) => {
        e.preventDefault()
        if (props.saleProduct.selected && sale.dialogActiveState == false) {
            sale.dialogActiveState = true;
            sale.onChangeQuantity(props.saleProduct, gv)
        }
    })
}

const allow_change_price = computed(()=>{
    if(gv.device_setting.is_order_station == 1 && gv.device_setting.show_button_change_price_on_order_station==1)
    {
        return true;
    }
    else if(gv.device_setting.is_order_station==0 ){
        return true;
    }

    return false;
});

sale.vue.$onKeyStroke('F4', (e) => {
    e.preventDefault();
    if(!allow_change_price){
        return;
    }    
   
    if (props.saleProduct.selected && sale.dialogActiveState == false) {
        sale.dialogActiveState = true;
        sale.onChangePrice(props.saleProduct,gv, numberFormat);
    }
})

sale.vue.$onKeyStroke('F5', (e) => {
    e.preventDefault();

    if(gv.device_setting.is_order_station==1){
        return;
    }
    
    onDiscountClick("Percent")
})


sale.vue.$onKeyStroke('F6', (e) => {
    e.preventDefault();
    if(gv.device_setting.is_order_station==1){
        return;
    }

    onDiscountClick("Amount")
})

sale.vue.$onKeyStroke('F7', (e) => {
    e.preventDefault();  
    if(gv.device_setting.is_order_station==1){
        return;
    }
    
    if(props.saleProduct.selected && sale.dialogActiveState == false){
        
        if(!props.saleProduct.is_free){
            onSaleProductFree(props.saleProduct);
            
        }else{
            sale.onSaleProductCancelFree(props.saleProduct)
            toaster.warning($t('msg.This item is not allow to discount'));
        }
    }
    
})

function onDiscountClick(discount_type){
    if (props.saleProduct.selected && sale.dialogActiveState == false) {
        sale.dialogActiveState=true;
        if (props.saleProduct.allow_discount) {
            if (!sale.isBillRequested()) {
                gv.authorize("discount_item_required_password", "discount_item", "discount_item_required_note", "Discount Item Note", "", true).then((v) => {
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
        else {
            toaster.warning($t('msg.This item is not allow to discount'));
        }
    }
}

function onSaleProductFree() {
    if (!sale.isBillRequested()) {
        gv.authorize("free_item_required_password", "free_item", "free_item_required_note", "Free Item Note", props.saleProduct.product_code).then((v) => {
            if (v) {
                props.saleProduct.free_note = v.note
                sale.dialogActiveState = true;
                sale.onSaleProductFree(props.saleProduct);
            }
        });

    }
}


function onChangeQuantity(){
    if(sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status == 'Submitted'){
        return;
    }
    sale.onChangeQuantity(saleProduct, gv);
}

</script>
<style lang="">
    
</style>