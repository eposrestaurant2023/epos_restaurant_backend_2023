<template>
  <div class="overflow-hidden flex items-end">

    <div class="flex flex-wrap w-full">
      <ComButtonToTableLayout  :is-mobile="false" />
      <template v-if="sale.sale.table_id">
        <ComPrintBillButton v-if="sale.sale.sale_status != 'Bill Requested'" doctype="Sale" title="Print Bill" />
        <v-btn v-else stacked color="error" size="small" class="m-1 grow" prepend-icon="mdi-printer" @click="onCancelPrintBill">
          Cancel Print Bill
        </v-btn>
      </template>
      <ComDiscountButton />
      <v-btn v-if="sale.sale.table_id" stacked size="small" class="m-1 grow" prepend-icon="mdi-plus" @click="onSubmitAndNew">
        Submit & New
      </v-btn>
      <v-btn stacked size="small" color="error" class="m-1 grow" prepend-icon="mdi-currency-usd" @click="onQuickPay">
        Quick Pay
      </v-btn>
      <v-btn stacked size="small" color="error" class="m-1 grow" prepend-icon="mdi-note-outline" v-if="sale.sale.note" @click="sale.sale.note = ''">
        Remove Note
      </v-btn>
      <v-btn stacked size="small" color="info" class="m-1 grow" prepend-icon="mdi-note-outline" @click="sale.onSaleNote(sale.sale)" v-else>
        Note
      </v-btn>
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


const router = useRouter()
const sale = inject("$sale")
const gv = inject("$gv")
const setting = gv.setting;
const toaster = createToaster({ position: "top" })

async function onQuickPay() {

  await sale.onSubmitQuickPay().then((value) => {
    if (value) {
      router.push({ name: "TableLayout" });
    }
  });
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

}

function newSale() {
  if (sale.newSaleResource == null) {
    sale.createNewSaleResource();
  }
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

}


</script>