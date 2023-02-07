<template>
    <PageLayout title="Customer" icon="mdi-account-multiple" full>
        <template #action>
            <v-btn  prepend-icon="mdi-account-plus" type="button" @click="onAddCustomer">
               Add Customer
            </v-btn>
        </template>
    <ComTable :headers="headers" doctype="Customer" extra-fields="name" @callback="onCallback"/>
  </PageLayout>
  <!-- <CustomerDetail :params="{name:'C2022-0003'}"/> -->
</template>
<script setup>
import { ref, useRouter, customerDetailDialog, addCustomerDialog } from '@/plugin'
import PageLayout from '@/components/layout/PageLayout.vue';
import ComTable from '@/components/table/ComTable.vue';
import ComButton from '@/components/ComButton.vue';
import CustomerDetail from './CustomerDetail.vue';
const router = useRouter()

async function onCallback(data) {
    if (data.fieldname == "customer_code_name") {
        const result = await customerDetailDialog({
            name: data.data.name
        });
    }
}
async function onAddCustomer() {
    await addCustomerDialog ({title: "New Customer", value:  ''});
}
const headers = ref([
    { title: 'Photo', align: 'center', key: 'photo', fieldtype: 'Image', placeholder: 'customer_name_en'},
    { title: 'Customer', align: 'start', key: 'customer_code_name', callback: true },
    { title: 'Name En', align: 'start', key: 'customer_name_en' },
    { title: 'Name Kh', align: 'start', key: 'customer_name_kh' },
    { title: 'Gender', align: 'center', key: 'gender'},
    { title: 'Group', align: 'start', key: 'customer_group' },
    { title: 'Date of Birth', align: 'start', key: 'date_of_birth', fieldtype: "Date"},
    { title: 'Phone Number', align: 'start', key: 'phone_number'},
    { title: 'Company Name', align: 'start', key: 'company_name'},
    { title: 'Location', align: 'start', key: 'province'},
    
    
])

 

</script>