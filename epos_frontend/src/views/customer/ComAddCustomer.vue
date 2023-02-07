<template>
    <v-dialog v-model="open">
        <v-card width="960" class="mx-auto my-0">
            <ComToolbar @onClose="onClick">
                <template #title>
                    {{ params.title }}
                </template>
            </ComToolbar>
            <v-card-text>  
                <form @submit.prevent="onSave">
                
                    <v-row>
                        <v-col cols="10" sm="8"  md="6" >
                            <ComInput v-model="customer.customer_name_en" :required="true" keyboard label="Customer Name En"/>    
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
                            <ComAutoComplete v-model="customer.customer_group" doctype="Customer Group" variant="solo" class="mr-2"/> 
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <ComInput v-model="customer.customer_name_kh" keyboard label="Customer Name Kh"/>
                        </v-col>
                        <v-col pr="8">
                            <ComInput v-model="customer.date_of_birth" type="date" class="pr-3" label="Date of Birth"/>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput v-model="customer.company_name" keyboard label="Company Name"/>
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
 
                            <v-select 
                                density="compact"
                                v-model="customer.gender" 
                                placeholder="Gender"
                                :items="gender?.options?.split('\n')"
                                hide-no-data
                                hide-details 
                                clearable
                                variant="solo" class="mr-2"
                            ></v-select>
                        </v-col> 
                    </v-row>
                    <p class="font-weight-bold pt-6 pb-2">
                        Contact Information
                    </p>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput v-model="customer.phone_number" keyboard label="Phone Number 1"/>
                        </v-col>
                        <v-col cols="10" sm="8"  md="6">
                            <ComAutoComplete v-model="customer.country" feild="country_name" doctype="Country" variant="solo" class="mr-2"/>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="10" sm="8"  md="6">
                            <ComInput v-model="customer.phone_number_2" keyboard label="Phone Number 2"/>
                        </v-col>
                        <v-col> 
                            <ComAutoComplete v-model="customer.province" feild="province" doctype="Province" variant="solo" class="mr-2"/>
                       </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <ComInput v-model="customer.email_address" keyboard label="Email Address"/> 
                        </v-col>
                        <v-col>
                            
                        </v-col>
                    </v-row>
                    <p class="font-weight-bold pt-6 pb-2">
                        Address & Note
                    </p>
                    <v-row>
                        <v-col>
                            <ComInput v-model="customer.address" cols="10" sm="8"  md="6" title="Enter Note" keyboard label="Address" type="textarea"></ComInput>
                        </v-col>
                        <v-col>
                            <ComInput v-model="customer.note" cols="10" sm="8"  md="6" title="Enter Note" class="pr-3" keyboard label="Note" type="textarea"></ComInput>
                        </v-col>
                    </v-row>
            
                    <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" variant="flat" type="button" @click="onClose" color="error">
                            Close
                        </v-btn>
                        <v-btn variant="flat" type="sumbit" color="primary">
                            Save
                        </v-btn>
                    </div>
                </div>   
            </form>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<script setup>
import Enumerable from 'linq'
import { createResource,ref, inject, computed,createDocumentResource, onMounted } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import { createToaster } from "@meforma/vue-toaster";
import ComInput from '../../components/form/ComInput.vue';
 
const toaster = createToaster({ position: "top" });
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


const open = ref(true);
const customer = ref({})
const data = ref({})
const gender = computed(()=>{
    return Enumerable.from(gv.customerMeta?.fields).where("$.fieldname=='gender'").firstOrDefault()
})


if(gv.customerMeta==null){ 
 
    createResource({
        url: "epos_restaurant_2023.api.api.get_meta",
        params: {
         doctype: "Customer"
        },
        auto:true,
        // cache:["customer_meta_data"],
        onSuccess(doc){
            gv.customerMeta = doc;
           
        }

    })
} 
if(props.params.name){
    data.value = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Customer',
    name: props.params.name,
    auto:true
  })
  customer.value = data.value.doc 

} 

function onSave(){
    if(props.params.name){
        const saved = JSON.parse(JSON.stringify(customer.value))
        data.value.setValue.submit(saved);
        toaster.success("Edit Customer is Successful");
        emit('resolve',false);
    }else{
        onAddNew()
    }
}

function onAddNew() {
    customer.value.doctype = 'Customer'
    customerResource.submit({doc:customer.value}).then((res)=>{
        
        toaster.success("Add Customer is Successful");
        emit('resolve',res);
    })
    console.log(customer.value);
}
function onClose() {
    emit('resolve',false);
}
 
function onClick() {
    emit('resolve',false);
}
</script>

