<template>
    <div class="overflow-hidden">
      <div class="button-group">
        <div class="d-flex text-center">
          <div class="cursor-pointer p-2 grow bg-red-600 text-white hover:bg-red-700" v-if="sale.sale.note" @click="sale.sale.note = ''">Remove Note</div>
          <div class="cursor-pointer p-2 grow bg-white hover:bg-gray-400" @click="sale.onSaleNote(sale.sale)" v-else>Note</div>
          <ComSaleButtonMore />
        </div>
      </div>
    </div>
  
</template>
<script setup>
import { inject, useRouter } from '@/plugin';
import ComSaleButtonMore from './ComSaleButtonMore.vue';
import { createToaster } from '@meforma/vue-toaster';

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
 


</script>