<template>
    <PageLayout :title="$t('Customer')" icon="mdi-account-multiple" full>
        <template #action>
            <v-btn  prepend-icon="mdi-account-plus" type="button" @click="onAddCustomer">
              {{ $t("Add New") }}
            </v-btn>
        </template>
        <ComCustomerCard :headers="headers" doctype="Customer" extra-fields="name,photo,customer_name_en" @callback="onCallback" v-if="mobile"/>
        <ComTable :headers="headers" doctype="Customer" extra-fields="name" @callback="onCallback" v-else/>
  </PageLayout>
</template>
<script setup>
import { ref, customerDetailDialog, addCustomerDialog,i18n } from '@/plugin'
import PageLayout from '@/components/layout/PageLayout.vue';
import ComTable from '@/components/table/ComTable.vue';
import ComCustomerCard from './ComCustomerCard.vue'
import {useDisplay} from 'vuetify';

const { t: $t } = i18n.global;  

const {mobile} = useDisplay()
async function onCallback(data) { 
    if (data.fieldname == "customer_code_name") {
        const result = await customerDetailDialog({
            name: data.data.name
        });
    }
}
async function onAddCustomer() {
    await addCustomerDialog ({title: $t("Add New"), value:  ''});
}
const headers = ref([
    // { title: 'Photo', align: 'center', key: 'photo', fieldtype: 'Image', placeholder: 'customer_name_en'},
    { title: $t('Customer'), align: 'start', key: 'customer_code_name', callback: true },
   
    { title: $t('Name Kh'), align: 'start', key: 'customer_name_kh' },
    { title: $t('Gender'), align: 'center', key: 'gender'},
    { title: $t('Group'), align: 'start', key: 'customer_group' },
    { title: $t('Date of Birth'), align: 'start', key: 'date_of_birth', fieldtype: "Date"},
    { title: $t('Phone Number'), align: 'start', key: 'phone_number'},
    { title: $t('Company Name'), align: 'start', key: 'company_name'},
    { title: $t('Location'), align: 'start', key: 'province'},
    
    
])

 

</script>