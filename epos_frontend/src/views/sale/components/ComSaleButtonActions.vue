<template>
  <div class="flex items-end" :class="mobile ? 'overflow-hidden':''">

    <div class="flex flex-wrap w-full">
      <ComButtonToTableLayout :is-mobile="false" @closeModel="closeModel()" />
        <template v-if="setting.table_groups && setting.table_groups.length > 0 ">

          <template v-if="sale.sale.sale_status != 'Bill Requested' && !mobile">
            <ComPrintBillButton  v-if="gv.device_setting.show_button_print_bill==1"  :variant="mobile ? 'tonal' : 'elevated'" :stacked="!mobile" doctype="Sale" :title="$t('Print Bill')" />
            
          </template>

          <template v-else>

            <template v-if="!mobile">
              <v-btn  v-if="gv.device_setting.show_button_cancel_print_bill==1" color="error" size="small" class="m-0-1 grow" :variant="mobile ? 'tonal' : 'elevated'"  :stacked="!mobile" :prepend-icon="mobile ? '' : 'mdi-printer'" @click="onCancelPrintBill">
                {{ $t('Cancel Print Bill') }}
              </v-btn> 
              
          </template>
          </template>
        </template>

      <ComDiscountButton v-if="gv.device_setting.is_order_station==0"/>
      
      <v-btn v-if="setting.table_groups && setting.table_groups.length > 0 && !mobile" :variant="mobile ? 'tonal' : 'elevated'"
        :color="mobile ? 'primary' : ''" :stacked="!mobile" size="small" class="m-0-1 grow"
        :prepend-icon="mobile ? '' : 'mdi-plus'" @click="onSubmitAndNew">
        {{ $t('Submit and New') }}
      </v-btn>
      
      <v-btn v-if="setting.table_groups && setting.table_groups.length > 0 && mobile"  :height="mobile ? '35px' : undefined" :variant="mobile ? 'tonal' : 'elevated'"
        :color="mobile ? 'primary' : ''" :stacked="!mobile" size="small" class="m-0-1 grow" 
        :prepend-icon="mobile ? '' : 'mdi-plus'" @click="onSubmitAndNew">
        {{$t('Submit and New')}}
      </v-btn>


      <v-btn v-if="device_setting.show_option_payment ==1" :stacked="!mobile" size="small" color="error" class="m-0-1 grow" :height="mobile ? '35px' : undefined" :variant="mobile ? 'tonal' : 'elevated'"
        :prepend-icon="mobile ? '' : 'mdi-currency-usd'" @click="onQuickPay">
        {{ $t('Quick Pay') }}
      </v-btn>
    
      <ComSaleButtonMore />
    </div>
  </div>
</template>
<script setup>
import { inject, useRouter,ref,changePriceRuleDialog,changeSaleTypeModalDialog,ComSaleReferenceNumberDialog,addCommissionDialog,i18n } from '@/plugin';
import ComDiscountButton from './ComDiscountButton.vue';
import ComPrintBillButton from './ComPrintBillButton.vue';
import { createToaster } from '@meforma/vue-toaster';
import ComButtonToTableLayout from './ComButtonToTableLayout.vue';
import ComSaleButtonMore from './ComSaleButtonMore.vue';
import Enumerable from 'linq';
import { useDisplay } from 'vuetify'
import { whenever,useMagicKeys  } from '@vueuse/core';

const { t: $t } = i18n.global;  

const { mobile } = useDisplay()
const router = useRouter()
const sale = inject("$sale")
const socket = inject("$socket")
const product = inject("$product")
const gv = inject("$gv")
const setting = gv.setting;
const toaster = createToaster({ position: "top" })
const emit = defineEmits(["onSubmitAndNew", 'onClose'])
const device_setting = JSON.parse(localStorage.getItem("device_setting"))

const { ctrl_p } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'p' && e.type === 'keydown')
        e.preventDefault()
    },
})

const { ctrl_r } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'r' && e.type === 'keydown')
        e.preventDefault()
    },
})
const { ctrl_u } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'u' && e.type === 'keydown')
        e.preventDefault()
    },
})
const { ctrl_q } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'q' && e.type === 'keydown')
        e.preventDefault()
    },
})

const { ctrl_t } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 't' && e.type === 'keydown')
        e.preventDefault()
    },
})

whenever(ctrl_p, () => onChangePriceRule())
whenever(ctrl_r, () => onChangeTaxSetting())
whenever(ctrl_u, () => {
  if(gv.device_setting.show_button_commission==0){
    return;
  }
  onAddCommission();
})
whenever(ctrl_q, () =>{
  if(gv.device_setting.show_option_payment==0){
    return;
  }
  onQuickPay();
})



sale.vue.$onKeyStroke('F10',(e)=>{ 
  e.preventDefault();
    if(gv.device_setting.is_order_station==1){
        return;
    }

  if(sale.dialogActiveState==false){
    onSaleDiscount('Percent')
  }
  
})

sale.vue.$onKeyStroke('F11',(e)=>{
  e.preventDefault();
    if(gv.device_setting.is_order_station==1){
        return;
    }

  if(sale.dialogActiveState==false){
    onSaleDiscount('Amount')
  }
})

async function onChangeSaleType() {
  const result = await changeSaleTypeModalDialog({})
}

async function onAddCommission(){
    if(!sale.isBillRequested()) {
      sale.dialogActiveState=true
        const result = await addCommissionDialog({ title: 'title', name: 'Sale Commission', data: sale.sale });
        if (result != false) { 
            sale.sale = result.data
        }
        sale.dialogActiveState=false
    }
}

async function onChangeTaxSetting(){
    await sale.onChangeTaxSetting($t('Change Tax Setting'),sale.sale.tax_rule,sale.sale.change_tax_setting_note,gv );     
}

async function onReferenceNumber(){
  sale.dialogActiveState=true;
    const reference_number = await ComSaleReferenceNumberDialog({
        data: sale.sale
    })
    sale.dialogActiveState=false;
    if(typeof(reference_number) != 'boolean')
        sale.sale.reference_number = reference_number
}

async function onChangePriceRule() {
    if (sale.sale.sale_status != 'New') {
        toaster.warning($t('msg.This bill is not new order'));
        return;
    }
    if (!sale.isBillRequested()) {
        sale.dialogActiveState=true;
        const result = await changePriceRuleDialog({})
        sale.dialogActiveState=false;
        if (result == true) {
            if(product.setting.pos_menus.length>0){
                product.loadPOSMenu()
            }else{
                product.getProductMenuByProductCategory(db,"All Product Categories")
            }
            
            window.postMessage("close_modal","*");
            toaster.success($t('msg.Change price rule successfully'));
        }
    }
}

function onSaleDiscount(discount_type) {
    sale.dialogActiveState=true;
    if (sale.sale.sale_products.length == 0) {
        toaster.warning($t('msg.Please order a item to disocunt'));
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


async function onQuickPay() {

  await sale.onSubmitQuickPay().then((value) => {
    if (value) {
      product.onClearKeyword()
      sale.newSale();
      if(onRedirectSaleType()){
        if (sale.setting.table_groups.length > 0) {
          router.push({ name: "TableLayout" });
        } else {
          sale.getTableSaleList();
        }
      }
      //this code is send message to modal saleproduct list in mobile view
      //we use this below code to send signal close modal when complete task
      window.postMessage("close_modal", "*");

    }
  });
}

sale.vue.$onKeyStroke('Insert', (e)=>{
  e.preventDefault();
  if (sale.dialogActiveState === false) {
    sale.onSaleNote(sale.sale);
  }
        
})

sale.vue.$onKeyStroke('Insert', (e)=>{
  e.preventDefault();
  if (sale.dialogActiveState === false) {
    sale.onChoosePrinter(sale.sale);
  }
        
})


function onRedirectSaleType(){
    const redirect_sale_type = localStorage.getItem("redirect_sale_type") || null
    if(redirect_sale_type){
        router.push({name: 'AddSaleNoTable', params: {sale_type: redirect_sale_type}})
        return false
    }
    return true
}
function closeModel() {
  emit('onClose')
}
async function onCancelPrintBill() {
  gv.authorize("cancel_print_bill_required_password", "cancel_print_bill", "cancel_print_bill_required_note", "Cancel Print Bill Note").then((v) => {
    if (v) {
      sale.sale.sale_status = "Submitted";
      sale.sale.sale_status_color = setting.sale_status.find(r => r.name == 'Submitted').background_color;

      let msg = `User ${v.user} was Cancelled Print Bill`; 
      msg += `${v.note==""?'':', Reason: '+v.note }`;
      sale.auditTrailLogs.push({
          doctype:"Comment",
          subject:"Cancel Print Bill",
          comment_type:"Comment",
          reference_doctype:"Sale",
          reference_name:"New",
          comment_by:v.user,
          content:msg
      })  ; 
    }
  })

}




async function onSubmitAndNew() {
  //check if newsale recource3 is null then 
  //reinitalize newsaleresxource
  // backup old sale
  sale.table_id = sale.sale.table_id
  sale.tbl_number = sale.sale.tbl_number
  sale.customer = sale.sale.customer
  sale.customer_photo = sale.sale.customer_photo
  sale.customer_name = sale.sale.customer_name
  sale.price_rule = sale.sale.price_rule
  sale.sale_type = sale.sale.sale_type
  sale.guest_cover = sale.sale.guest_cover
  if (sale.newSaleResource == null) {
    sale.createNewSaleResource();
  }

  if (sale.sale.sale_products.length == 0) {
    toaster.warning($t('msg.There is no item to submit order'));
    return;
  }

  if (sale.sale.sale_status != 'Bill Requested') {

    const action = sale.action;
    const message = sale.message;
    const sale_status = sale.sale.sale_status;

    sale.action = "submit_order";
    sale.message =$t('msg.Submit order successfully');
    sale.sale.sale_status = "Submitted";


    await sale.onSubmit().then((value) => {
      if (value) {
        router.push({ name: "AddSale" });
        newSale();
      }
      else{
        sale.action = action;
        sale.message =message;
        sale.sale.sale_status = sale_status;
      }
    });
  } else {
    router.push({ name: "AddSale" });
    newSale();
  }

  sale.getTableSaleList();

  if(mobile){
    window.postMessage("close_modal", "*");
  }
  emit('onSubmitAndNew')

 
}

function newSale() {


  if (sale.newSaleResource == null) {
    sale.createNewSaleResource();
  }
  sale.newSale()
  sale.sale.sale_products = [],
  sale.sale.name = "";
  sale.sale.creation = "";
  sale.sale.modified = "";
  sale.sale.sale_status = "New";
  sale.sale.discount_type = 'Percent';
  sale.sale.discount = 0;
  const tableGroups = JSON.parse(localStorage.getItem("table_groups"));
  const tables = (Enumerable.from(tableGroups).selectMany("$.tables").toArray())
  let table = tables.filter(r => r.id == sale.sale.table_id);
  if (table) {
    table = table[0];

    if (parseFloat(table.default_discount) > 0) {

      sale.sale.discount_type = table.discount_type;
      sale.sale.discount = parseFloat(table.default_discount);
      if (table.discount_type == "Percent") {
        toaster.info($t("msg.This table have discount",[table.default_discount + '%']) )
      } else {
        toaster.info($t("msg.This table have discount",[( table.default_discount + ' ' + sale.setting.pos_setting.main_currency_name)]))
      }
    }
  }



  sale.updateSaleSummary();
  
  socket.emit("ShowOrderInCustomerDisplay",sale, "new");

}


</script>
<style>
.m-0-1 {
  margin: 2px !important;
  padding: 0px !important;
}
</style>