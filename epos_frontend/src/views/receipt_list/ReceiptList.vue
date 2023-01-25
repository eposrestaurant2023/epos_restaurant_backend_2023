<template>
  
  <PageLayout title="Receipt List" icon="mdi-note-outline" full>
    <ComTable :headers="headers" doctype="Sale" extra-fields="customer_name" @callback="onCallback" />

  </PageLayout>
</template>
<script setup>
import { ref, useRouter } from '@/plugin'
import PageLayout from '../../components/layout/PageLayout.vue';

import ComTable from '../../components/table/ComTable.vue';
import { saleDetailDialog } from '../../utils/dialog';

const router = useRouter()
function onCallback(data) {
 if(data.fieldname=="name"){
  const name =  data.data.name;
    saleDetailDialog({
      name:name
    });

  }
}
const headers = ref([
  {
    title: 'No',
    align: 'start',
    key: 'name',
    callback: true
  },
  { title: 'Customer Name', align: 'center', key: 'customer', template: '<b>{customer}-{customer_name}</b>', callback: true },
  { title: 'Date', align: 'center', key: 'posting_date', fieldtype: "Date" },
  { title: 'QTY', align: 'end', key: 'total_quantity', },
  { title: 'Grand Total', align: 'end', key: 'grand_total', fieldtype: "Currency" },
  { title: 'Total Discount', align: 'end', key: 'total_discount', fieldtype: "Currency" },
  { title: 'Total Paid', align: 'end', key: 'total_paid', fieldtype: "Currency" },
  { title: 'Balance', align: 'end', key: 'balance', fieldtype: "Currency", callback: true },
])

 
function onCustomer(customer) {
  router.push({ name: "CustomerDetail", params: { name: customer } });
}


</script>