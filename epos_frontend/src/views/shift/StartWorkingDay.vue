<template>
    <PageLayout title="Start Working Day" icon="mdi-calendar-clock">
        <v-row>
            <v-col md="6">
                <v-text-field label="Working Date" v-model="current_date" variant="solo" readonly></v-text-field>
            </v-col>
        <v-col md="6">
                <v-text-field label="POS Profile" v-model="pos_profile" variant="solo" readonly></v-text-field>
            </v-col>
        </v-row>
        <ComInput title="Enter Note" keyboard label="Open Note" v-model="note" type="textarea"></ComInput>
        <!-- <v-btn @click="addNewToDo">Add New Todo</v-btn> -->
        <v-btn @click="onStartWorking" color="primary">Start Working Day</v-btn>

    </PageLayout>
</template>
 
<script setup>
import moment from '@/utils/moment.js'
import { ref, createResource,useRouter,createToaster } from '@/plugin'
import PageLayout from '../../components/layout/PageLayout.vue';
import ComInput from '../../components/form/ComInput.vue';

const router = useRouter()
const pos_profile = localStorage.getItem("pos_profile");
const note = ref("")
const current_date = moment(new Date).format('DD-MM-YYYY');
const toaster = createToaster({ /* options */ });

 
createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(data){
        if(data){
            toaster.warning("Working day is already started",{position:"top"});
            router.push({name:"Home"});
        }
    }
    
})

function onStartWorking() {
    
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


</script>