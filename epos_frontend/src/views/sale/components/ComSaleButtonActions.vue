<template>
    <div class="overflow-hidden absolute left-0 right-0 bottom-0 top-auto">
      <div class="button-group">
        <div class="d-flex text-center">
          <ComButtonToTableLayout :is-mobile="false" />
          <ComPrintBillButton v-if="sale.sale.sale_status != 'Bill Requested'" doctype="Sale" title="Print Bill" />
          <div class="bg-red-600 text-white cursor-pointer grow m-1 rounded-md p-2 hover:bg-red-700" v-else @click="onCancelPrintBill">
            Cancel Print Bill</div>
          <ComDiscountButton />
          <div class="m-1 rounded-md cursor-pointer p-2 grow bg-white hover:bg-gray-400" @click="onSubmitAndNew">Submit & New</div>
          <div class="m-1 rounded-md cursor-pointer p-2 grow bg-orange-600 text-white hover:bg-orange-700" @click="onQuickPay">Quick
            Pay</div>
        </div>
      </div>
    </div>
  
</template>
<script setup>
import { inject, useRouter } from '@/plugin';
import ComDiscountButton from './ComDiscountButton.vue';
import ComPrintBillButton from './ComPrintBillButton.vue';
import { createToaster } from '@meforma/vue-toaster';
import ComButtonToTableLayout from './ComButtonToTableLayout.vue';

const router = useRouter()
const sale = inject("$sale")
const gv = inject("$gv")
const setting = gv.setting;
const toaster = createToaster({ position: "top" })




// async function onPrintBill(r) {
//   if (sale.sale.sale_products.length == 0) {
//     toaster.warning("Please select a menu item to submit order");
//   } else {
//     sale.sale.sale_status = "Bill Requested";
//     sale.action = "print_bill";
//     sale.pos_receipt = r;

//     //add to auddit trail
//     sale.auditTrailLogs.push({
//         doctype:"Comment",
//         subject:"Print Bill",
//         comment_type:"Comment",
//         reference_doctype:"Sale",
//         reference_name:"New",
//         comment_by:"cashier@mail.com",
//         content:`User sengho print bill. Amount:100$, Total Qty:5`

//       });

//     await sale.onSubmit().then(async (value) => {
//       if (value) {
//         router.push({ name: "TableLayout" });
//       }
//     });
//   }
// }

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
  sale.sale.sale_products = [],
    sale.sale.name = "";
  sale.sale.creation = "";
  sale.sale.modified = "";
  sale.sale.sale_status = "New";
  sale.updateSaleSummary();

}


</script>