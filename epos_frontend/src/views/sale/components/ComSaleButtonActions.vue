<template>
  <div class="flex items-end" :class="mobile ? 'overflow-hidden':''">

    <div class="flex flex-wrap w-full">
      <ComButtonToTableLayout :is-mobile="false" @closeModel="closeModel()" />
      <template v-if="setting.table_groups && setting.table_groups.length > 0">
        <ComPrintBillButton v-if="sale.sale.sale_status != 'Bill Requested' && !mobile"
          :variant="mobile ? 'tonal' : 'elevated'" :stacked="!mobile" doctype="Sale" title="Print Bill" />
        <template v-else>
          <v-btn v-if="!mobile" color="error" size="small" class="m-0-1 grow" :variant="mobile ? 'tonal' : 'elevated'"
            :stacked="!mobile" :prepend-icon="mobile ? '' : 'mdi-printer'" @click="onCancelPrintBill">
            Cancel Print Bill
          </v-btn>
        </template>
      </template>
      <ComDiscountButton />
      
      <v-btn v-if="setting.table_groups && setting.table_groups.length > 0 && !mobile" :variant="mobile ? 'tonal' : 'elevated'"
        :color="mobile ? 'primary' : ''" :stacked="!mobile" size="small" class="m-0-1 grow"
        :prepend-icon="mobile ? '' : 'mdi-plus'" @click="onSubmitAndNew">
        Submit & New
      </v-btn>
      
      <v-btn v-if="setting.table_groups && setting.table_groups.length > 0 && mobile"  :height="mobile ? '35px' : undefined" :variant="mobile ? 'tonal' : 'elevated'"
        :color="mobile ? 'primary' : ''" :stacked="!mobile" size="small" class="m-0-1 grow" 
        :prepend-icon="mobile ? '' : 'mdi-plus'" @click="onSubmitAndNew">
        New Order
      </v-btn>


      <v-btn :stacked="!mobile" size="small" color="error" class="m-0-1 grow" :height="mobile ? '35px' : undefined" :variant="mobile ? 'tonal' : 'elevated'"
        :prepend-icon="mobile ? '' : 'mdi-currency-usd'" @click="onQuickPay">
        Quick Pay
      </v-btn>
      <template v-if="!mobile">
        <v-btn stacked size="small" color="error" class="m-0-1 grow" prepend-icon="mdi-note-outline" v-if="sale.sale.note"
          @click="sale.sale.note = ''">
          Remove Note
        </v-btn>
        <v-btn stacked size="small" class="m-0-1 grow" prepend-icon="mdi-note-outline" @click="sale.onSaleNote(sale.sale)"
          v-else>
          Note
        </v-btn>
      </template>
      <ComSaleButtonMore />
    </div>
  </div>
</template>
<script setup>
import { inject, useRouter } from '@/plugin';
import ComDiscountButton from './ComDiscountButton.vue';
import ComPrintBillButton from './ComPrintBillButton.vue';
import { createToaster } from '@meforma/vue-toaster';
import ComButtonToTableLayout from './ComButtonToTableLayout.vue';
import ComSaleButtonMore from './ComSaleButtonMore.vue';
import Enumerable from 'linq';
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const router = useRouter()
const sale = inject("$sale")
const socket = inject("$socket")
const product = inject("$product")
const gv = inject("$gv")
const setting = gv.setting;
const toaster = createToaster({ position: "top" })
const emit = defineEmits(["onSubmitAndNew", 'onClose'])
async function onQuickPay() {

  await sale.onSubmitQuickPay().then((value) => {
    if (value) {
      product.onClearKeyword()
      sale.newSale();
      if (sale.setting.table_groups.length > 0) {
        router.push({ name: "TableLayout" });
      } else {
        sale.getTableSaleList();
      }
      //this code is send message to modal saleproduct list in mobile view
      //we use this below code to send signal close modal when complete task
      window.postMessage("close_modal", "*");

    }
  });
}
function closeModel() {
  emit('onClose')
}
async function onCancelPrintBill() {
  gv.authorize("cancel_print_bill_required_password", "cancel_print_bill", "cancel_print_bill_required_note", "Cancel Print Bill Note").then((v) => {
    if (v) {
      sale.sale.sale_status = "Submitted";
      sale.sale.sale_status_color = setting.sale_status.find(r => r.name == 'Submitted').background_color;
      sale.auditTrailLogs.push({
        doctype: "Comment",
        subject: "Cancel Print Bill",
        comment_type: "Comment",
        reference_doctype: "Sale",
        reference_name: "New",
        comment_by: "cashier@mail.com",
        content: `User sengho cancel print bill. Amount:100$, Total Qty:5, Reason:Test Note`
      });
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
    toaster.warning("There's no item to submit");
    return;
  }

  if (sale.sale.sale_status != 'Bill Requested') {
    sale.action = "submit_order";
    sale.message = "Submit Order Successfully";
    sale.sale.sale_status = "Submitted";
    await sale.onSubmit().then((value) => {
      if (value) {
        router.push({ name: "AddSale" });
        newSale();
      }
    });
  } else {
    router.push({ name: "AddSale" });
    newSale();
  }

  sale.getTableSaleList();
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
        toaster.info("This table is discount " + table.default_discount + '%')
      } else {
        toaster.info("This table is discount " + table.default_discount + ' ' + sale.setting.pos_setting.main_currency_name)
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