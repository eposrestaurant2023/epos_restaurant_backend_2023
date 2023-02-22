<template> 
 
    <v-list-item prepend-icon="mdi-eye-outline" title="View Bill" @click="onViewBill()"/>
    <v-list-item @click="onRemoveSaleNote()"  v-if="sale.sale.note">
        <template v-slot:prepend>
            <v-icon icon="mdi-note-outline" color="error"></v-icon>
        </template>
        <v-list-item-title class="text-red-700">Remove Note</v-list-item-title>
    </v-list-item>
    <v-list-item prepend-icon="mdi-note-outline" title="Note" @click="sale.onSaleNote(sale.sale)" v-else/>
    
    <v-list-item prepend-icon="mdi-bulletin-board" title="Change Price Rule" @click="onChangePriceRule()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Open Cash Drawer" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-account-multiple-outline" title="Change Table" @click="onChangeTable()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Merge Table/Bill" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Split Bill" @click="onViewInvoice()"/>
    <v-list-item prepend-icon="mdi-account-multiple-outline" :title="`Change Guest Cover (${sale.sale.guest_cover})`" @click="onUpdateGuestCover()"/>
    <v-list-item prepend-icon="mdi-cart" title="Change Sale Type" @click="onChangeSaleType()"/>
    <v-list-item prepend-icon="mdi-cash-100" title="Tax Setting" @click="onViewInvoice()"/>
    <v-divider inset></v-divider>
    <v-list-item v-if="sale.sale.name" @click="onViewInvoice()">
        <template #prepend>
            <v-icon color="error" icon="mdi-delete"></v-icon>
        </template>
        <v-list-item-title class="text-red-700">Delete Bill</v-list-item-title>
    </v-list-item>
     
</template>
<script setup>
    import {viewBillModelModel, inject,keyboardDialog, changeTableDialog, changePriceRuleDialog,changeSaleTypeModalDialog} from "@/plugin"

    const sale = inject('$sale')
    const setting = JSON.parse(localStorage.getItem("setting"))
    async function onViewBill(){
        const result = await viewBillModelModel({})
    }
    async function onUpdateGuestCover(){
    if (setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: "Guest Cover", type: 'number', value: sale.sale.guest_cover });
        if (typeof result != 'boolean' && result != false) {
            sale.sale.guest_cover = parseInt(result);
            if (sale.sale.guest_cover == undefined || isNaN(sale.sale.guest_cover)) {
                sale.sale.guest_cover = 0;
            }

        } else { 
            return;
        }
    }
}

async function onChangeTable(){
    if (!sale.isBillRequested()) {
        const result =await changeTableDialog({});
    }
}
async function onChangePriceRule(){
    if(!sale.isBillRequested()){
        const result = await changePriceRuleDialog({})
    }
}
function onRemoveSaleNote(){
    sale.sale.note = ''
}
async function onChangeSaleType(){
    const result = await changeSaleTypeModalDialog({})
}
</script>