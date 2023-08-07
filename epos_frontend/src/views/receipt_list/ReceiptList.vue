<template>
    <PageLayout :title="$t('Receipt List')" icon="mdi-file-chart" full>
      <ComReceiptListCard :headers="headers" doctype="Sale" extra-fields="customer_name,sale_status_color" @callback="onCallback" v-if="mobile"/>
      <ComTable :headers="headers" doctype="Sale" extra-fields="customer_name,sale_status_color" business-branch-field="business_branch" pos-profile-field="pos_profile" @callback="onCallback" v-else/>
    </PageLayout>
</template>
<script setup>
import { ref, useRouter, saleDetailDialog, customerDetailDialog,i18n} from '@/plugin'
import PageLayout from '@/components/layout/PageLayout.vue';
import ComTable from '@/components/table/ComTable.vue';
import {useDisplay} from 'vuetify' 
import ComReceiptListCard from './components/ComReceiptListCard.vue';

const { t: $t } = i18n.global; 

const {mobile} = useDisplay()
const router = useRouter()
async function onCallback(data) {
 
 if(data.fieldname=="name"){
  const name =  data.data.name;
  const result = await  saleDetailDialog({
      name:name
    });
    if (result=="delete_order"){
      window.parent.postMessage('refresh', '*');
    }
   
  }
  else if(data.fieldname == "customer"){
     customerDetailDialog({
        name: data.data.customer
    })
  }
}
const headers = ref([
  { title: $t('No'), align: 'start',key: 'name',callback: true},
  { title: $t('Customer Name'), align: 'start', key: 'customer', template: '{customer}-{customer_name}', callback: true },
  { title: $t('Table'), align: 'start', key: 'tbl_number' },
  { title: $t('Date'), align: 'center', key: 'posting_date', fieldtype: "Date" },
  { title: $t('Qty'), align: 'center', key: 'total_quantity', },
  { title: $t('Grand Total'), align: 'end', key: 'grand_total', fieldtype: "Currency" },
  { title: $t('Total Discount'), align: 'end', key: 'total_discount', fieldtype: "Currency" },
  { title: $t('Total Paid'), align: 'end', key: 'total_paid_with_fee', fieldtype: "Currency" },
  { title: $t('Balance'), align: 'end', key: 'balance', fieldtype: "Currency" },
  { title: $t('Status'), align: 'center', key: 'sale_status', fieldtype: "Status", color_field:"sale_status_color" },
])

 
</script>