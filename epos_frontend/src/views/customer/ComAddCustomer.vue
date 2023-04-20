<template>
    <ComModal @onPrint="onPrint" :mobileFullscreen="true" @onClose="onClose" @onOk="onSave">
        <template #title>
            {{ params.title }}
        </template>
        <template #content>
            <div>         
                <v-row>
                    <v-col cols="12" sm="6"  md="6" >
                        <ComInput v-model="customer.customer_name_en" :required="true" keyboard label="Customer Name En"/>    
                    </v-col>
                    
                    <v-col cols="12" sm="6"  md="6">
                        <ComInput v-model="customer.customer_name_kh" keyboard label="Customer Name Kh"/>
                    </v-col>
                    <v-col cols="12" sm="6"  md="6">
                        <ComAutoComplete v-model="customer.customer_group" doctype="Customer Group" variant="solo"/> 
                    </v-col>
                    <v-col cols="12" sm="6"  md="6">
                        <ComInput v-model="customer.date_of_birth" type="date" label="Date of Birth"/>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6"  md="6">
                        <ComInput v-model="customer.company_name" keyboard label="Company Name"/>
                    </v-col>
                    <v-col cols="12" sm="6"  md="6">

                        <v-select 
                            density="compact"
                            v-model="customer.gender" 
                            placeholder="Gender"
                            :items="gender?.options?.split('\n')"
                            hide-no-data
                            hide-details 
                            clearable
                            variant="solo"
                        ></v-select>
                    </v-col> 
                </v-row>
                <p class="font-weight-bold  pt-6 pb-2">
                    Contact Information
                </p>
                <v-row>
                    <v-col cols="12" sm="6"  md="6">
                        <ComInput v-model="customer.phone_number" keyboard label="Phone Number 1"/>
                    </v-col>
                    
                    <v-col cols="12" sm="6"  md="6">
                        <ComInput v-model="customer.phone_number_2" keyboard label="Phone Number 2"/>
                    </v-col>
                    <v-col>
                        <ComInput v-model="customer.email_address" keyboard label="Email Address"/> 
                    </v-col>
                    <v-col cols="12" sm="6"  md="6"> 
                        <ComAutoComplete v-model="customer.province" feild="province" doctype="Province" variant="solo"/>
                   </v-col>
                   <v-col cols="12" sm="6"  md="6">
                        <ComAutoComplete v-model="customer.country" feild="country_name" doctype="Country" variant="solo"/>
                    </v-col>
                </v-row>
                <p class="font-weight-bold pt-6 pb-2">
                    Address & Note
                </p>
                <v-row>
                    <v-col cols="12" sm="6"  md="6">
                        <ComInput v-model="customer.address" title="Enter Note" keyboard label="Address" type="textarea"></ComInput>
                    </v-col>
                    <v-col cols="12" sm="6"  md="6">
                        <ComInput v-model="customer.note" title="Enter Note" keyboard label="Note" type="textarea"></ComInput>
                    </v-col>
                </v-row>
                
                <div> 
            </div>
        </div>
        </template> 
    </ComModal>
</template>

<script setup>
import Enumerable from 'linq'
import { createResource,ref, inject, computed,createDocumentResource, onMounted } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import { createToaster } from "@meforma/vue-toaster";
import ComInput from '../../components/form/ComInput.vue';
import ComModal from '../../components/ComModal.vue';
 
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


let open = ref(true);
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
    if(!customer.value.date_of_birth){
        customer.value.date_of_birth = ''
    }
    customer.value.doctype = 'Customer'
    customerResource.submit({doc:customer.value}).then((res)=>{
        
        toaster.success("Add Customer is Successful");
        emit('resolve',res);
    })
}
function onClose() {
    emit('resolve',false);
}
</script>

