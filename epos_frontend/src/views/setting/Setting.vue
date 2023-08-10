<template lang="">
    <PageLayout class="pb-4" :title="$t('Setting')" icon="mdi-wrench">
        <v-container class="!py-0">
            <v-card
            class="mx-auto"
          >
            <v-card-item>
              <div>
               
                <div class="text-h6 mb-1">
                  Reset Invoice Number
                </div>
                <div class="text-caption">Your current invoice number counter is {{counter}}. To reset counter, click on button <strong>Reset Counter</strong> below </div>
              </div>
            </v-card-item>
        
            <v-card-actions>
              <v-btn variant="outlined" color="error" @click="onResetCounter">
                Reset Counter
              </v-btn>
            </v-card-actions>
            <v-card-item>
              <div>
               
                <div class="text-h6 mb-1">
                  Reset Invoice Number All Station
                </div>
                <div class="text-caption">To reset counter, click on button <strong>Reset Counter</strong> below </div>
              </div>
            </v-card-item>
        
            <v-card-actions>
              <v-btn variant="outlined" color="error" @click="onResetCounterAllStation">
                Reset Counter
              </v-btn>
            </v-card-actions>
          </v-card>
          
        </v-container>
    </PageLayout>
    
</template>
<script setup>


import PageLayout from '@/components/layout/PageLayout.vue';

import { keyboardDialog, i18n, inject, onMounted, ref ,createToaster} from '@/plugin'
const { t: $t } = i18n.global;
const frappe = inject('$frappe');
const call = frappe.call();
const counter = ref(123)
const toaster = createToaster({ position: 'top' }); 

async function onResetCounter() {



    const result = await keyboardDialog({ title: $t('Reset Counter'), type: 'number', value: 0 });
    if (result) {

        call
            .post('epos_restaurant_2023.api.api.update_customer_bill_counter', {
                 pos_profile: localStorage.getItem("pos_profile"),
                 counter:result
                })
            .then((d) => {
                 console.log(d)
                toaster.success($t('msg.Reset counter successfully'));
            }).catch((err)=>{
             
                let message = err._server_messages
          
                if (message){
                    message = JSON.parse(message)
                    
                    message.forEach(r => {
                        const error = JSON.parse(r)
                        toaster.warning($t(error.message));        
                    });
                }
                
            })

    }
}
async function onResetCounterAllStation() {



const result = await keyboardDialog({ title: $t('Reset Counter'), type: 'number', value: 0 });
if (result) {

    call
        .post('epos_restaurant_2023.api.api.update_customer_bill_counter', {
             pos_profile: localStorage.getItem("pos_profile"),
             counter:result
            })
        .then((d) => {
             console.log(d)
            toaster.success($t('msg.Reset counter successfully'));
        }).catch((err)=>{
         
            let message = err._server_messages
      
            if (message){
                message = JSON.parse(message)
                
                message.forEach(r => {
                    const error = JSON.parse(r)
                    toaster.warning($t(error.message));        
                });
            }
            
        })

}
}

onMounted(() => {
    call
        .get('epos_restaurant_2023.api.api.get_current_customer_bill_counter', { pos_profile: localStorage.getItem("pos_profile") })
        .then((result) => {
            counter.value = result.message

        })

})

</script> 