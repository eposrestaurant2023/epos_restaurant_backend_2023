<template>
    <PageLayout title="Receipt List" icon="mdi-note-outline" full>
      <ComReceiptListCard :headers="headers" doctype="Sale" extra-fields="customer_name,sale_status_color" @callback="onCallback" v-if="mobile"/>
      <ComTable :headers="headers" doctype="Sale" extra-fields="customer_name,sale_status_color" business-branch-field="business_branch" @callback="onCallback" v-else/>
    </PageLayout>
</template>
<script setup>
import { ref, useRouter, saleDetailDialog, customerDetailDialog } from '@/plugin'
import PageLayout from '@/components/layout/PageLayout.vue';
import ComTable from '@/components/table/ComTable.vue';
import {useDisplay} from 'vuetify' 
import ComReceiptListCard from './components/ComReceiptListCard.vue';
const {mobile} = useDisplay()
const router = useRouter()
function onCallback(data) {
 
 if(data.fieldname=="name"){
  const name =  data.data.name;
    saleDetailDialog({
      name:name
    });
   
  }
  else if(data.fieldname == "customer"){
     customerDetailDialog({
        name: data.data.customer
    })
  }
}
const headers = ref([
  {
    title: 'No',
    align: 'start',
    key: 'name',
    callback: true
  },
  { title: 'Customer Name', align: 'center', key: 'customer', template: '{customer}-{customer_name}', callback: true },
  { title: 'Date', align: 'center', key: 'posting_date', fieldtype: "Date" },
  { title: 'QTY', align: 'end', key: 'total_quantity', },
  { title: 'Grand Total', align: 'end', key: 'grand_total', fieldtype: "Currency" },
  { title: 'Total Discount', align: 'end', key: 'total_discount', fieldtype: "Currency" },
  { title: 'Total Paid', align: 'end', key: 'total_paid', fieldtype: "Currency" },
  { title: 'Balance', align: 'end', key: 'balance', fieldtype: "Currency" },
  { title: 'Status', align: 'center', key: 'sale_status', fieldtype: "Status", color_field:"sale_status_color" },
])

 
</script>