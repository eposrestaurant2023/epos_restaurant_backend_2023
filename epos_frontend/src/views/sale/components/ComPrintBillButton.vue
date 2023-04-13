<template> 
  <template v-if="sale.sale.sale_status != 'Bill Requested'">
    <template v-if="gv.setting.reports.filter(r => r.doc_type == doctype && r.show_in_pos == 1).length == 1">
      <v-btn v-if="mobile" style="width:64px" icon="mdi-printer" @click="onPrintReport(gv.setting.reports.filter(r => r.doc_type == doctype && r.show_in_pos == 1)[0])"></v-btn>
      <v-btn v-else :stacked="!mobile" color="info" size="small" class="m-0-1 grow"
        :prepend-icon="mobile ? '' : 'mdi-printer'" :variant="mobile ? 'tonal' : 'elevated'"
        @click="onPrintReport(gv.setting.reports.filter(r => r.doc_type == doctype && r.show_in_pos == 1)[0])">
        Print Bill</v-btn>
    </template>
    <v-menu v-else>
      <template v-slot:activator="{ props }">
        <v-btn v-if="mobile" :stacked="!mobile" color="default" @click="$emit('onClose')" icon="mdi-printer"
          v-bind="props" />
        <v-btn v-else stacked color="info" size="small" class="m-0-1 grow" prepend-icon="mdi-printer"
          @click="$emit('onClose')" v-bind="props">
          Print Bill
        </v-btn>
      </template>
      <v-list>
        <v-list-item v-for="(r, index) in gv.setting.reports.filter(r => r.doc_type == doctype && r.show_in_pos == 1)"
          :key="index" @click="onPrintReport(r)">
          <v-list-item-title>{{ r.name }}</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </template>
  <template v-else> 
    <v-btn v-if="!mobile" color="error" size="small" class="m-0-1 grow" :variant="mobile ? 'tonal' : 'elevated'"
      :stacked="!mobile" :prepend-icon="mobile ? '' : 'mdi-printer'" @click="onCancelPrintBill">
      Cancel Print Bill
    </v-btn>
    <v-btn v-else @click="onCancelPrintBill">
      <svg xmlns="http://www.w3.org/2000/svg" height="24px" viewBox="0 0 24 24" width="24px" fill="#FFFFFF"><path d="M0 0h24v24H0V0z" fill="none"/><path d="M19.1 17H22v-6c0-1.7-1.3-3-3-3h-9l9.1 9zm-.1-7c.55 0 1 .45 1 1s-.45 1-1 1-1-.45-1-1 .45-1 1-1zm-1-3V3H6v1.1L9 7zM1.2 1.8L0 3l4.9 5C3.3 8.1 2 9.4 2 11v6h4v4h11.9l3 3 1.3-1.3-21-20.9zM8 19v-5h2.9l5 5H8z"/></svg>
    </v-btn>
  </template>
</template>
<script setup>
import { defineProps, defineEmits, createToaster, inject, useRouter, confirm } from "@/plugin"
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const router = useRouter();
const gv = inject("$gv")
const sale = inject('$sale')
const props = defineProps({
  doctype: String,
  title: {
    type: String,
    default: ""
  },
  isMobile: {
    type: Boolean,
    default: false
  }
});
const emit = defineEmits(["onPrint"])
const toaster = createToaster({ position: 'top' })
async function onPrintReport(r) {
  if (sale.sale.sale_products?.length == 0) {
    toaster.warning("Please select a menu item to submit order");
  } else {
    if (gv.setting.reports.filter(r => r.doc_type == props.doctype && r.show_in_pos == 1).length == 1) {
      if (await confirm({ title: 'Print Request Bill', text: 'Are you sure you want to print request bill?' }) == false) {
        return;
      }
    }
    sale.sale.sale_status = "Bill Requested";
    sale.action = "print_bill";
    sale.pos_receipt = r;

    //add to auddit trail
    sale.auditTrailLogs.push({
      doctype: "Comment",
      subject: "Print Bill",
      comment_type: "Comment",
      reference_doctype: "Sale",
      reference_name: "New",
      comment_by: "cashier@mail.com",
      content: `Print request bill`

    });

    await sale.onSubmit().then(async (value) => {
      if (value) {

        router.push({ name: "TableLayout" });
        window.postMessage("close_modal", "*");
      }
    });
  }
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
</script>