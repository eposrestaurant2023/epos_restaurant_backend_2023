<template>
    <v-dialog v-model="open">
        <v-card width="960" class="mx-auto my-0">
            <ComToolbar @onClose="onClick">
                <template #title>
                    Add Customer 
                </template>
            </ComToolbar>
            <v-card-text>
                <v-container>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6" v-model="customer.customer_name_en">
                            <ComInput label="Customer Name En"/>    
                        </v-col> 
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput label="Customer Name Kh"/>    
                        </v-col>              
                    </v-row>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput label="Phone Number"/>
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput label="Company Name"/>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput label="Gender"/>
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput label="Company Group"/>
                        </v-col>
                    </v-row>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="blue-darken-1" variant="text">
                            Close
                        </v-btn>
                        <v-btn color="blue-darken-1" variant="text" @click="onSave">
                            Save
                        </v-btn>
                    </v-card-actions>
                </v-container>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
  
<script setup>

import { useStore,createResource, createDocumentResource,ref } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComInput from '../../components/form/ComInput.vue';

const props = defineProps({
    params: {
        type: Object,
        required: true,
    },
})
const emit = defineEmits(["resolve"])
let customerResource = createResource({
  url: 'frappe.client.insert', 
})

const store = useStore();

 
const open = ref(true);
const customer = ref({
    doctype:"Customer"
})

function onSave() {
 
    customerResource.submit({doc:customer.value})
}
 
function onClick() {
    emit('resolve',false);
}
</script>

