<template> 
 
    <v-list-item prepend-icon="mdi-eye-outline" title="View Bill" @click="onViewBill()"/>
    <v-list-item prepend-icon="mdi-bulletin-board" title="Change Price Rule" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Open Cash Drawer" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Change Table" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Merge Table/Bill" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Split Bill" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Change Guest Cover" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Change Sale Type" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Tax Setting" @click="onViewInvoice()"/>
    <v-divider inset></v-divider>
    <v-list-item @click="onViewInvoice()">
        <template #prepend>
            <v-icon color="error" icon="mdi-delete"></v-icon>
        </template>
        <v-list-item-title class="text-red-700">Delete Bill</v-list-item-title>
    </v-list-item>
     
</template>
<script setup>
    import {viewBillModelModel} from "@/plugin"
    async function onViewBill(){
        const result = await viewBillModelModel({})
    }
</script>