<template> 
    <v-menu>
        <template v-slot:activator="{ props }">
            <div class="cursor-pointer p-2 grow bg-gray-400 hover:bg-gray-500 text-white" v-bind="props">
                More
            </div>
        </template>
        <v-list>
            <v-list-item prepend-icon="mdi-eye-outline" title="View Bill" @click="onViewInvoice()"/>
            <v-list-item prepend-icon="mdi-bulletin-board" title="Change Price Rule" @click="onViewInvoice()"/>
            <v-list-item prepend-icon="mdi-cash-100" title="Open Cash Drawer" @click="onViewInvoice()"/>
            <v-divider inset></v-divider>
            <v-list-item @click="onViewInvoice()">
                <template #prepend>
                    <v-icon color="error" icon="mdi-delete"></v-icon>
                </template>
                <v-list-item-title class="text-red-700">Delete Bill</v-list-item-title>
            </v-list-item>
        </v-list>
    </v-menu>       
</template>
<script setup>
    import {defineProps,defineEmits, createResource} from "@/plugin"
</script>