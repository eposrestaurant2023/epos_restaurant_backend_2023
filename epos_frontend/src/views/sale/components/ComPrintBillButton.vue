<template>
  <v-btn v-if="gv.setting.reports.filter(r => r.doc_type == doctype && r.show_in_pos == 1).length == 1" :stacked="!mobile" color="info"
    size="small" class="m-1 grow" :prepend-icon="mobile ? '' : 'mdi-printer'" :variant="mobile ? 'tonal':'elevated'"
    @click="onPrintReport(gv.setting.reports.filter(r => r.doc_type == doctype && r.show_in_pos == 1)[0])">
    Print Bill
  </v-btn>

  <v-menu v-else>
    <template v-slot:activator="{ props }">
      <v-btn icon v-if="mobile" @click="$emit('onClose')" v-bind="props">
        <v-icon>mdi-printer</v-icon>
      </v-btn>
      <v-btn v-else stacked color="info" size="small" class="m-1 grow" prepend-icon="mdi-printer"
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
<script setup>
import { defineProps, defineEmits, createToaster, inject, useRouter,confirm } from "@/plugin"

const router = useRouter();
const gv = inject("$gv")
const sale = inject('$sale')
const props = defineProps({
  doctype: String,
  title: {
    type: String,
    default: ""
  },
  mobile: {
    type: Boolean,
    default: false
  }
});
const emit = defineEmits(["onPrint"])
const toaster = createToaster({ poisition: 'top' })




async function onPrintReport(r) {
  if (sale.sale.sale_products?.length == 0) {
    toaster.warning("Please select a menu item to submit order");
  } else {
    if(gv.setting.reports.filter(r => r.doc_type == props.doctype && r.show_in_pos == 1).length==1){
      if(await confirm({title:'Print Request Bill', text:'Are you sure you want to print request bill?'})==false){
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
      }
    });
  }
}
</script>