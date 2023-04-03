<template>
    <PageLayout title="Start Working Day" icon="mdi-calendar-clock" class="p-4">
        <div class="mb-3">
            <v-row>
                <v-col cols="12" sm="6">
                    <v-text-field label="Working Date" v-model="current_date" variant="solo" readonly :hide-details="true"></v-text-field>
                </v-col>
            <v-col cols="12" sm="6">
                    <v-text-field label="POS Profile" v-model="pos_profile" variant="solo" readonly :hide-details="true"></v-text-field>
                </v-col>
            </v-row>
            <div class="mt-4">
                <ComInput title="Enter Note" keyboard label="Open Note" v-model="note" type="textarea"></ComInput>
            </div>
        </div>
        <v-btn @click="onStartWorking" color="primary" class="mt-4">Start Working Day</v-btn>
    </PageLayout>
</template>
 
<script setup>
import moment from '@/utils/moment.js'
import { ref, createResource,useRouter,createToaster,inject,confirm } from '@/plugin'
import PageLayout from '../../components/layout/PageLayout.vue';
import ComInput from '../../components/form/ComInput.vue';
const gv = inject("$gv")
const router = useRouter()
const pos_profile = localStorage.getItem("pos_profile");
const note = ref("")
const current_date = moment(new Date).format('DD-MM-YYYY');
const toaster = createToaster({ /* options */ });

 
createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        business_branch: gv.setting.business_branch
    },
    auto: true,
    onSuccess(data){
        if(data){
            toaster.warning("Working day is already started",{position:"top"});
            router.push({name:"Home"});
        }
    }
    
})

async function onStartWorking() {
    
    if(await confirm({title:"Start Working Day", text:"Are sure you want to start working day?"})){
        createResource({
            url:"frappe.client.insert",
            params:{
                doc:{
                    doctype:"Working Day",
                    pos_profile:pos_profile,
                    posting_date:moment(new Date).format('YYYY-MM-DD'),
                    note:note.value
                }
            },
            onSuccess(data) {
                toaster.success("Start working day successfully",{position:"top"});
                router.push({name:"Home"});
            },

            auto:true
     })
    }
}


</script>