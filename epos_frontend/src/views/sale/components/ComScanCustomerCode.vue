<template>
    <v-dialog v-model="open">
      <v-card width="700" class="mx-auto my-0">
            <ComToolbar @onClose="onClose" >
                <template #title>
                    Scan Membership Card Number
                </template>
            </ComToolbar>

           <v-card-text>
            <form @submit.prevent="onSeachCustomer">
            <v-alert
            icon="mdi-cards-outline"
            prominent
            type="success"
            variant="outlined"
            class="mb-2"
            >
            Please enter or scan customer membership card number.
            </v-alert>
            <ComInput v-model="customerCode" autofocus placeholder="Scan membership Card"  variant="outlined" density="default" />

            <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" variant="flat" @click="onClose(false)" color="error">
                            Close
                        </v-btn>
                        <v-btn :loading="resource?.loading" variant="flat"  color="primary">
                            OK
                        </v-btn>
                    </div>
                </div>
                </form>
           </v-card-text>
               
    
      </v-card>
      </v-dialog>
    
    </template>
    <script setup>
      import { ref, defineProps,defineEmits, createDocumentResource,onUnmounted,watch} from '@/plugin'
      import ComToolbar from '@/components/ComToolbar.vue';
import ComInput from '../../../components/form/ComInput.vue';

      let customerCode = ref("");
      let resource = ref(createDocumentResource({  url: 'frappe.client.get',
            doctype: 'Customer',}));
      const props = defineProps({
        params:{
          type:Object,
          required: true,
        }
      })
    
watch(() => resource.value?.doc, (d) => {
  if(d!=null){
    emit("resolve", d);
  }
});

      const emit = defineEmits(["resolve","reject"]) 
    
      const open = ref(true);
   
    
      function onClose(){
        emit("resolve",false);
      }
     
      async function onSeachCustomer(){
     
        if(customerCode!=""){ 
          resource.value =  createDocumentResource({
            url: 'frappe.client.get',
            doctype: 'Customer',
            name: customerCode.value,
            auto:true,
             
        })
        

    
    }
        
      }
    
    </script>