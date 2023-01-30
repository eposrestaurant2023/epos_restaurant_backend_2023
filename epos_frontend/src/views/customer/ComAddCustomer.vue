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
                        <v-col cols="10" sm="8"  md="6" >
                            <ComInput v-model="customer.customer_name_en" keyboard label="Customer Name En"/>    
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput v-model="customer.company_name" keyboard label="Company Name"/>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput v-model="customer.customer_name_kh" keyboard label="Customer Name Kh"/>
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
                            <ComAutoComplete v-model="customer.customer_group" :doctype="customerGroup.options" variant="solo" class="mr-2"/>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <v-select 
                                density="compact"
                                v-model="customer.gender" 
                                placeholder="Gender"
                                :items="gender.options.split('\n')"
                                hide-no-data
                                hide-details 
                                clearable
                                variant="solo" class="mr-2"
                            ></v-select>    
                        </v-col> 
                        <v-col>
                            <ComAutoComplete v-model="customer.province" feild="province" doctype="Province" variant="solo" class="mr-2"/>
                       </v-col> 
                    </v-row>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput v-model="customer.phone_number" keyboard label="Phone Number"/>
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
                            <ComAutoComplete v-model="customer.country" feild="country_name" doctype="Country" variant="solo" class="mr-2"/>
                        </v-col>
                    </v-row>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="blue-darken-1" variant="text" @click="onClose">
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
import Enumerable from 'linq'
import { createResource,ref, inject, computed } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComInput from '../../components/form/ComInput.vue';
const gv = inject('$gv')
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

const gender = computed(()=>{
    return Enumerable.from(gv.customerMeta.fields).where("$.fieldname=='gender'").firstOrDefault()
})

const customerGroup = computed(()=>{
    return Enumerable.from(gv.customerMeta.fields).where("$.fieldname=='customer_group'").firstOrDefault()
})

const open = ref(true);
const customer = ref({
    doctype:"Customer"
})

function onSave() {
    customerResource.submit({doc:customer.value})

}
function onClose() {
    emit('resolve',false);
}
 
function onClick() {
    emit('resolve',false);
}
</script>

