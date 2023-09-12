<template>
    <div class="bg-white"> 
  
        <div class="p-1">
          <v-text-field elevation="0" type="text"  :placeholder="$t('Enter Number')" density="compact" variant="solo" single-line hide-details 
          v-model="input" @input="onInput"> </v-text-field>
        </div> 
          <div class="flex">
            <div class="flex-grow">
              <div class="grid grid-cols-3 sale-keypad-btn-number">
                <v-btn  elevation="0" class="button-border-left" rounded="0" @click="onKeyPressed('7')" size="large">
                  7
                </v-btn>

                <v-btn  elevation="0" class="button-border-left-right" rounded="0" @click="onKeyPressed('8')" size="large">
                  8
                </v-btn>

                <v-btn  elevation="0" class="button-border-right" rounded="0" @click="onKeyPressed('9')" size="large">
                  9
                </v-btn>  
                <v-btn elevation="0" class="button-border-left" rounded="0" @click="onKeyPressed('4')" size="large">
                  4
                </v-btn>
                <v-btn elevation="0" class="button-border-left-right" rounded="0" @click="onKeyPressed('5')" size="large">
                  5
                </v-btn>
                <v-btn elevation="0" class="button-border-right" rounded="0" @click="onKeyPressed('6')" size="large">
                  6
                </v-btn> 
                <v-btn  elevation="0" class="button-border-left" rounded="0" @click="onKeyPressed('1')" size="large">
                  1
                </v-btn>
                <v-btn elevation="0" class="button-border-left-right" rounded="0" @click="onKeyPressed('2')" size="large">
                  2
                </v-btn>
                <v-btn elevation="0" class="button-border-right" rounded="0" @click="onKeyPressed('3')" size="large">
                  3
                </v-btn> 
                <v-btn  elevation="0" class="button-border-left" rounded="0" @click="onKeyPressed('0')" size="large">
                  0
                </v-btn> 
                <v-btn elevation="0" class="button-border-left" rounded="0" @click="onKeyPressed('.')" size="large">
                  .
                </v-btn> 
                <v-btn class=" btn-clear text-white button-border-right" color="#fe1c45" elevation="0" rounded="0"  @click="onKeyPressed('clear')" size="large">
                  {{ $t('Clear') }}
                </v-btn>
              </div>
            </div>
            <div  style="width: 120px;">
              <div class="grid grid-cols-1 sale-keypad-btn">
                <v-btn color="primary" class="text-sm button-border-right" elevation="0" rounded="0" :disabled="!is_allow_reorder" @click="onReOrderPressed()" size="large">
                  {{ $t('Re-Order') }}
                </v-btn> 
                <v-btn color="primary" class="text-sm button-border-right" elevation="0" rounded="0" :disabled="!allow_change_price" @click="onChangePricePressed()" size="large">
                  {{ $t('Price') }}
                </v-btn>
                <v-btn color="primary" class="text-sm button-border-right" elevation="0" rounded="0" :disabled="!is_allow_append_qty" @click="onChangeQuantityPressed()" size="large">
                  {{ $t('Qty') }}
                </v-btn>
               
                <v-btn color="error" class="text-sm button-border-right" elevation="0" rounded="0" :disabled="!allow_delete_item" @click="onDeleteItemPressed()" size="large">
                  {{ $t('Delete') }}
                </v-btn>
              </div>
            </div>
          </div>
    </div>
</template>

<script setup>
import {computed, inject,createToaster, ref,i18n } from '@/plugin';
import Enumerable from 'linq';
const toaster = createToaster({ position: "top" });

const numberFormat = inject('$numberFormat');
const sale = inject("$sale")
const gv = inject("$gv")
const { t: $t } = i18n.global;

const input = ref('');

function onKeyPressed(num){
    if(num=="clear"){
      input.value = "";
      return;
    }
    if(!input.value){
        input.value ="";
    }
    if(input.value=="" && num=='.'){
        input.value = "0";
    }
     
    if(input.value.includes('.') && num=='.'){
        return;
    }
    
    input.value += num;
}

function onInput(){
  input.value = input.value
}

function onValidate(){
    const sale_products = sale.sale.sale_products.filter((r)=>r.selected==true);
    if(sale_products.length<=0){
        toaster.warning($t("msg.Please select item to process"))
        return null;
    } 
    let value = parseFloat((input.value||"0"));
    return {sale_product: sale_products[0], inupt: value};
}

const is_allow_reorder = computed(() => {
  const sale_products = (sale.sale.sale_products??[]).filter((r)=>r.selected==true);
  if(sale_products.length<=0){
    return false;
  } 
  
  if(sale_products[0].is_require_employee){
    return false;
  }
  return true;
});

const is_allow_append_qty = computed(() => {
  const sale_products = (sale.sale.sale_products??[]).filter((r)=>r.selected==true);
  if(sale_products.length<=0){
    return false;
  }

  if(sale_products[0].is_require_employee){
    return false;
  }


  if(sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sale_products[0].append_quantity == 0 || sale_products[0].sale_product_status == 'Submitted'){
    return false;
  }
  return true;
});

const allow_change_price = computed(()=>{
    const sale_products = (sale.sale.sale_products??[]).filter((r)=>r.selected==true);
    if(sale_products.length<=0){
      return false;
    }

    if(gv.device_setting.is_order_station == 1 && gv.device_setting.show_button_change_price_on_order_station==1)
    {
        return true;
    }
    else if(gv.device_setting.is_order_station==0 ){
        return true;
    }
    return false;
});


const allow_delete_item = computed(()=>{
    // let value = parseFloat((input.value||"0"));
    // if(value<=0){
    //   return;
    // }
    const sale_products = (sale.sale.sale_products??[]).filter((r)=>r.selected==true);
    if(sale_products.length<=0){
      return false;
    } 
    return true;
}); 

function onReOrderPressed(){ 
    const validate= onValidate();
    if(!validate){
        return;
    }  

    let sp = validate.sale_product;
    const value = validate.inupt<=0?1:validate.inupt;

    if (!sale.isBillRequested()) {
        if (sp.sale_product_status == "New" || sale.setting.pos_setting.allow_change_quantity_after_submit == 1) {
            sale.updateQuantity(sp, sp.quantity + value)
        } else {
            let strFilter = `$.product_code=='${sp.product_code}' && $.append_quantity ==1 && $.price==${sp.price} && $.portion=='${sp.portion}'  && $.modifiers=='${sp.modifiers}'  && $.unit=='${sp.unit}'  && $.is_free==0`

            if (!gv.setting?.pos_setting?.allow_change_quantity_after_submit) {
                strFilter = strFilter + ` && $.sale_product_status == 'New'`
            }
            const sale_product = Enumerable.from(sale.sale.sale_products).where(strFilter).firstOrDefault();
            if (sale_product != undefined) {
                sale_product.quantity = parseFloat(sale_product.quantity) + value;
                sale.updateSaleProduct(sp);

            } else {
                setTimeout(() => {
                    sale.cloneSaleProduct(sp, sp.quantity + value);
                }, 100);
            }
        }
        input.value = "";
    }
}


function onChangeQuantityPressed(){
    const validate= onValidate();
    if(!validate){
        return;
    }  
    let sp = validate.sale_product;
    let quantity = validate.inupt<=0?1:validate.inupt;;
    

    if (sale.setting.pos_setting.allow_change_quantity_after_submit == 1 || sp.sale_product_status == "New") {
        if (quantity == 0) {
            quantity = 1
        }
        sale.updateQuantity(sp, quantity);
        input.value = "";
    } else {
        sp.selected = false;
        //do add record
        if (quantity > sp.quantity) {
          sale.cloneSaleProduct(sp, quantity);
          input.value = "";
        } else {
            if (sp.quantity - quantity > 0) {
                //do delete record
              gv.authorize("delete_item_required_password", "delete_item", "delete_item_required_note", "Delete Item Note", "", false).then(async (v) => {
                if (v) {
                    sp.deleted_item_note = v.note;
                    sale.onRemoveSaleProduct(sp, sp.quantity - quantity, v.user);
                    input.value = "";
                }
              });
            }
        } 
    }

}

function onChangePricePressed(){
  const validate= onValidate();
  if(!validate){
      return;
  }  
  let sp = validate.sale_product;
  const value = validate.inupt<=0?0:validate.inupt;
  input.value = "";
  sale.onChangePrice(sp,gv,numberFormat,value)
}

function onDeleteItemPressed(){
  const validate= onValidate();
  if(!validate){
      return;
  }  
  let sp = validate.sale_product;

  const value = validate.inupt<=0?sp.quantity :validate.inupt;
  input.value = "";
  sale.onRemoveItem(sp,gv,numberFormat,value);
}

 


</script>
<style>
  .sale-keypad-btn button ,.sale-keypad-btn-number button.btn-clear{
      font-size: 14px !important;
  }
  .sale-keypad-btn-number button {
    font-size: 18px !important;
  }

  .button-border-right{
    border-bottom: 1px rgb(155, 155, 155) solid !important;
    border-right: 1px rgb(155, 155, 155) solid !important;
  }
  .button-border-left{
    border-bottom: 1px rgb(155, 155, 155) solid !important;
    border-left: 1px rgb(155, 155, 155) solid !important;
  }
  .button-border-left-right{
    border-bottom: 1px rgb(155, 155, 155) solid !important;
    border-left: 1px rgb(155, 155, 155) solid !important;
    border-right: 1px rgb(155, 155, 155) solid !important;
  }
  
</style>