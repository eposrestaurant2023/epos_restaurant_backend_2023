<template>

    <PageLayout class="pb-4" title="Close Working Day:" icon="mdi-calendar-clock">
        <template #title>
            {{shiftInformation.data?.working_day?.name}}
        </template>
        <template #action>
            <v-btn
                prepend-icon="mdi-printer"
                @click="onOpenReport"
   >Report</v-btn>
        </template>

      <v-row v-if="shiftInformation.data?.working_day">
            <v-col md="6">
                <ComInput  title="POS Profile" label="POS Profile" v-model="shiftInformation.data.working_day.pos_profile" readonly></ComInput>

            </v-col>
            <v-col md="6">
                <ComInput  title="Working Date" label="Working Date" v-model="working_date" readonly></ComInput>
       
            </v-col>
            <v-col md="12">
                <ComInput class="mb-8" title="Enter Note" keyboard label="Closed Note" v-model="closed_note" type="textarea"></ComInput>
       
            </v-col>
        </v-row>
          
        <v-btn :loading="(workingDayResourceResource.setValue && workingDayResourceResource.setValue.loading) ? workingDayResourceResource.setValue.loading : false" @click="onCloseWorkingDay" color="primary">Close Working Day</v-btn>

        <v-btn @click="router.push({ name: 'Home' })" color="error" class="ml-4">Cancel</v-btn>

    </PageLayout>
</template>

<script setup>
import moment from '@/utils/moment.js';
import {inject, printPreviewDialog,confirm, onMounted, createDocumentResource, ref, createResource, useRouter, createToaster,computed  } from "@/plugin";
import PageLayout from '../../components/layout/PageLayout.vue';

const router = useRouter();
const toaster = createToaster();
const gv = inject('$gv')
const closed_note = ref("")
 
const working_date = computed(()=>{
    return moment(shiftInformation.data?.working_day?.posting_date).format('DD-MM-YYYY');
})
const workingDayResourceResource = ref({});

const shiftInformation = createResource({
        url: "epos_restaurant_2023.api.api.get_current_shift_information",
        params: {
            business_branch: gv.setting?.business_branch,
            pos_profile: localStorage.getItem("pos_profile")
        },
        onSuccess(data) { 
           
            if (data.cashier_shift != null) {
                toaster.warning("Please close cashier shift first");
                router.push({name:"Home"});
            } else if(data.working_day==null){
                toaster.warning("Please start working day first");
                router.push({name:"Home"});
            }
           
        }
    })



onMounted(async () => {

    await shiftInformation.fetch();



    workingDayResourceResource.value = createDocumentResource({
        url: 'frappe.client.get',
        doctype: 'Working Day',
        name: shiftInformation.data.working_day.name,
        setValue: {
            async onSuccess(doc) {
                toaster.success("Close working day successfully", {
                    position: "top",
                });
                //check from setting if system will need to print after close shift
                if (localStorage.getItem("is_window") == 1) {
                    if (setting.print_working_day_summary_after_close_working_day == 1) {
                        window.chrome.webview.postMessage("working_doc");
                    }
                    if (setting.print_working_day_sale_product_summary_after_close_working_day == 1) {
                        window.chrome.webview.postMessage("working_doc");
                    }
                }
                await printPreviewDialog(
                    { title: "Working day report #" + doc.name, doctype: "Working Day", name: doc.name }
                )
                
                router.push({ name: "Home" });
 

            },

        },

    });
})

async function onCloseWorkingDay() {
    if(await confirm({title:"Close Working Day", text:"Are sure you want to close working day?"})){
        workingDayResourceResource.value.setValue.submit({
            is_closed:1,
            closed_note:closed_note.value,
            closed_date:moment(new Date()).format('YYYY-MM-DD HH:mm:ss')
        })
    } 
}
function onOpenReport(){
    printPreviewDialog( { title: "Working Day Report #" + shiftInformation.data.working_day.name, doctype: "Working Day", name: shiftInformation.data.working_day.name});
}
</script>